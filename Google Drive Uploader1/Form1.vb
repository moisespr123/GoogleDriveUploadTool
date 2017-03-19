Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Drive.v3
Imports Google.Apis.Services
Imports Google.Apis.Util.Store
Imports System.IO
Imports System.Threading
Imports Google.Apis.Upload
Imports Google.Apis.Download
Imports System.Security.Cryptography

Public Class Form1
    Private FileIdsListBox As New ListBox
    Private FileSizeListBox As New ListBox
    Public pageToken As String = ""
    ' If modifying these scopes, delete your previously saved credentials
    ' at ~/.credentials/drive-dotnet-quickstart.json
    Shared Scopes As String() = {DriveService.Scope.DriveFile, DriveService.Scope.Drive}
    Shared ApplicationName As String = "Drive API .NET Quickstart"
    Public service As DriveService
    Private ResumeUpload As Boolean = False
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim credential As UserCredential

        Using stream = New FileStream("client_secret.json", FileMode.Open, FileAccess.Read)
            Dim credPath As String = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
            credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json")
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, New FileDataStore(credPath, True)).Result
            Console.WriteLine(Convert.ToString("Credential file saved to: ") & credPath)
        End Using
        ' Create Drive API service.
        Dim Initializer As New BaseClientService.Initializer()
        Initializer.HttpClientInitializer = credential
        Initializer.ApplicationName = ApplicationName
        service = New DriveService(Initializer)

        ' Define parameters of request.
        Dim listRequest As FilesResource.ListRequest = service.Files.List()
        listRequest.PageSize = 10
        listRequest.Fields = "nextPageToken, files(id, name, size)"

        ' List files.
        Dim files = listRequest.Execute()
        If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
            For Each file In files.Files
                ListBox1.Items.Add(file.Name)
                FileIdsListBox.Items.Add(file.Id)
                Try
                    FileSizeListBox.Items.Add(file.Size)
                Catch
                    FileSizeListBox.Items.Add("0")
                End Try
            Next
        End If
        pageToken = files.NextPageToken
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim listRequest As FilesResource.ListRequest = service.Files.List()
        listRequest.PageSize = 10
        listRequest.Fields = "nextPageToken, files(id, name, size)"
        listRequest.PageToken = pageToken
        ' List files.
        Dim files = listRequest.Execute()
        If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
            For Each file In files.Files
                ListBox1.Items.Add(file.Name)
                FileIdsListBox.Items.Add(file.Id)
                Try
                    FileSizeListBox.Items.Add(file.Size)
                Catch
                    FileSizeListBox.Items.Add("0")
                End Try
            Next
        End If
        pageToken = files.NextPageToken
    End Sub
    Private starttime As DateTime
    Private timespent As TimeSpan
    Private secondsremaining As Integer = 0
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Label3.Text = String.Format("{0:F2} MB", My.Computer.FileSystem.GetFileInfo(TextBox1.Text).Length / 1024 / 1024)
        ProgressBar1.Maximum = My.Computer.FileSystem.GetFileInfo(TextBox1.Text).Length / 1024 / 1024
        Dim FileMetadata As New Data.File
        FileMetadata.Name = My.Computer.FileSystem.GetName(TextBox1.Text)
        Dim FileFolder As New List(Of String)
        If String.IsNullOrEmpty(TextBox2.Text) = False Then
            FileFolder.Add(TextBox2.Text)
        Else
            FileFolder.Add("root")
        End If
        FileMetadata.Parents = FileFolder
        Dim UploadStream As New FileStream(TextBox1.Text, System.IO.FileMode.Open, System.IO.FileAccess.Read)
        Dim UploadFile As FilesResource.CreateMediaUpload = service.Files.Create(FileMetadata, UploadStream, "")
        UploadFile.ChunkSize = ResumableUpload.MinimumChunkSize * 4
        AddHandler UploadFile.ProgressChanged, New Action(Of IUploadProgress)(AddressOf Upload_ProgressChanged)
        AddHandler UploadFile.ResponseReceived, New Action(Of Data.File)(AddressOf Upload_ResponseReceived)
        AddHandler UploadFile.UploadSessionData, AddressOf Upload_UploadSessionData
        UploadCancellationToken = New System.Threading.CancellationToken
        Dim uploadUri As Uri = GetSessionRestartUri()
        starttime = DateTime.Now
        If uploadUri = Nothing Then
            UploadFile.UploadAsync(UploadCancellationToken)
        Else
            Console.WriteLine("Restarting prior upload session.")
            UploadFile.ResumeAsync(uploadUri, UploadCancellationToken)
        End If

    End Sub
    Private ErrorMessage As String = ""
    Private UploadCancellationToken As System.Threading.CancellationToken
    Shared BytesSentText As Long
    Shared UploadStatusText As String
    Private Sub Upload_ProgressChanged(uploadStatusInfo As IUploadProgress)
        Select Case uploadStatusInfo.Status
            Case UploadStatus.Completed
                UploadStatusText = "Completed!!"
                BytesSentText = My.Computer.FileSystem.GetFileInfo(TextBox1.Text).Length
                UpdateBytesSent()
            Case UploadStatus.Starting
                BytesSentText = "0"
                UploadStatusText = "Starting..."
                UpdateBytesSent()
            Case UploadStatus.Uploading
                BytesSentText = uploadStatusInfo.BytesSent
                UploadStatusText = "Uploading..."
                timespent = DateTime.Now - starttime
                Try
                    secondsremaining = (timespent.TotalSeconds / ProgressBar1.Value * (ProgressBar1.Maximum - ProgressBar1.Value))
                Catch
                    secondsremaining = 0
                End Try
                UpdateBytesSent()
            Case UploadStatus.Failed
                Dim APIException As Google.GoogleApiException = TryCast(uploadStatusInfo.Exception, Google.GoogleApiException)
                If (APIException Is Nothing) OrElse (APIException.Error Is Nothing) Then
                    MsgBox(uploadStatusInfo.Exception.Message)
                Else
                    MsgBox(APIException.Error.ToString())
                    ' Do not retry if the request is in error
                    Dim StatusCode As Int32 = CInt(APIException.HttpStatusCode)
                    ' See https://developers.google.com/youtube/v3/guides/using_resumable_upload_protocol
                    If ((StatusCode / 100) = 4 OrElse ((StatusCode / 100) = 5 AndAlso Not (StatusCode = 500 Or StatusCode = 502 Or StatusCode = 503 Or StatusCode = 504))) Then
                        MsgBox("Cannot retry upload...")
                    End If
                End If
                UploadStatusText = "Uploading..."
                UpdateBytesSent()
        End Select
    End Sub
    Private Sub Upload_ResponseReceived(file As Data.File)
        UploadStatusText = "Completed!!"
        BytesSentText = My.Computer.FileSystem.GetFileInfo(TextBox1.Text).Length
        UpdateBytesSent()
        RefreshFileList()
    End Sub
    Private Sub Upload_UploadSessionData(ByVal uploadSessionData As IUploadSessionData)
        ' Save UploadUri.AbsoluteUri and FullPath Filename values for use if program faults and we want to restart the program
        My.Settings.ResumeUri = uploadSessionData.UploadUri.AbsoluteUri
        My.Settings.ResumeFilename = TextBox1.Text
        ' Saved to a user.config file within a subdirectory of C:\Users\<yourusername>\AppData\Local
        My.Settings.Save()

    End Sub
    Private Function GetSessionRestartUri() As Uri
        If My.Settings.ResumeUri.Length > 0 AndAlso My.Settings.ResumeFilename = TextBox1.Text Then
            ' An UploadUri from a previous execution is present, ask if a resume should be attempted
            If MsgBox(String.Format("Resume previous upload?{0}{0}{1}", vbNewLine, TextBox1.Text), MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Resume Upload") = MsgBoxResult.Yes Then
                Return New Uri(My.Settings.ResumeUri)
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function
    Private Sub UpdateBytesSent()
        If Label4.InvokeRequired Then
            Dim method As MethodInvoker = New MethodInvoker(AddressOf UpdateBytesSent)
            Invoke(method)
        End If
        If Label8.InvokeRequired Then
            Dim method As MethodInvoker = New MethodInvoker(AddressOf UpdateBytesSent)
            Invoke(method)
        End If
        If Label14.InvokeRequired Then
            Dim method As MethodInvoker = New MethodInvoker(AddressOf UpdateBytesSent)
            Invoke(method)
        End If
        If ProgressBar1.InvokeRequired Then
            Dim method As MethodInvoker = New MethodInvoker(AddressOf UpdateBytesSent)
            Invoke(method)
        End If
        Label4.Text = String.Format("{0:F2} MB", BytesSentText / 1024 / 1024)
        Label8.Text = UploadStatusText
        Try
            ProgressBar1.Value = BytesSentText / 1024 / 1024
        Catch

        End Try
        Label10.Text = String.Format("{0:F2}%", ((ProgressBar1.Value / ProgressBar1.Maximum) * 100))
        Dim timeFormatted As TimeSpan = TimeSpan.FromSeconds(secondsremaining)
        Label14.Text = String.Format("{0}:{1:mm}:{1:ss}", CInt(Math.Truncate(timeFormatted.TotalHours)), timeFormatted)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        OpenFileDialog1.Title = "Select a file to Upload"
        OpenFileDialog1.FileName = My.Computer.FileSystem.GetName(TextBox1.Text)
        OpenFileDialog1.ShowDialog()
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub
    Private Shared FileToSave As FileStream
    Private Shared MaxFileSize
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        FileIdsListBox.SelectedIndex = ListBox1.SelectedIndex
        FileSizeListBox.SelectedIndex = ListBox1.SelectedIndex
        SaveFileDialog1.Title = "Browse for a location to save the file:"
        SaveFileDialog1.FileName = ListBox1.SelectedItem
        Dim SFDResult As MsgBoxResult = SaveFileDialog1.ShowDialog()
        If SFDResult = MsgBoxResult.Ok Then
            starttime = DateTime.Now
            Label3.Text = String.Format("{0:F2} MB", FileSizeListBox.SelectedItem / 1024 / 1024)
            ProgressBar1.Maximum = FileSizeListBox.SelectedItem / 1024 / 1024
            MaxFileSize = FileSizeListBox.SelectedItem
            FileToSave = New FileStream(SaveFileDialog1.FileName, FileMode.Create, FileAccess.Write)
            Dim DownloadRequest As FilesResource.GetRequest = service.Files.Get(FileIdsListBox.SelectedItem.ToString)
            AddHandler DownloadRequest.MediaDownloader.ProgressChanged, New Action(Of IDownloadProgress)(AddressOf Download_ProgressChanged)
            DownloadRequest.DownloadAsync(FileToSave)
        End If
    End Sub
    Private Sub Download_ProgressChanged(progress As IDownloadProgress)
        Select Case progress.Status
            Case DownloadStatus.Completed
                UploadStatusText = "Completed!!"
                FileToSave.Close()
                BytesSentText = MaxFileSize
                UpdateBytesSent()

            Case DownloadStatus.Downloading
                BytesSentText = progress.BytesDownloaded
                UploadStatusText = "Downloading..."
                UpdateBytesSent()
            Case UploadStatus.Failed
                UploadStatusText = "Failed..."
                UpdateBytesSent()
        End Select
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        RefreshFileList()
    End Sub

    Private Sub RefreshFileList()
        ListBox1.Items.Clear()
        FileIdsListBox.Items.Clear()
        FileSizeListBox.Items.Clear()
        Dim listRequest As FilesResource.ListRequest = service.Files.List()
        listRequest.PageSize = 10
        listRequest.Fields = "nextPageToken, files(id, name, size)"
        listRequest.PageToken = ""
        ' List files.
        Dim files = listRequest.Execute()
        If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
            For Each file In files.Files
                ListBox1.Items.Add(file.Name)
                FileIdsListBox.Items.Add(file.Id)
                Try
                    FileSizeListBox.Items.Add(file.Size)
                Catch
                    FileSizeListBox.Items.Add("0")
                End Try
            Next
        End If
    End Sub
    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        Dim filepath() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each path In filepath
            TextBox1.Text = path
        Next
    End Sub
    Private Sub Form1_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub
End Class
