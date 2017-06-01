Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Drive.v3
Imports Google.Apis.Services
Imports Google.Apis.Util.Store
Imports System.IO
Imports System.Threading
Imports Google.Apis.Upload
Imports Google.Apis.Download
Imports System.Collections.Specialized
Imports Microsoft.VisualBasic.Devices

Public Class Form1
    Private FileIdsListBox As New ListBox
    Private FileSizeListBox As New ListBox
    Private FileMIMEListBox As New ListBox
    Private FolderToUploadFileListBox As New ListBox
    Private FileModifiedTimeListBox As New ListBox
    Private FileCreatedTimeListBox As New ListBox
    Private FileMD5ListBox As New ListBox
    Private FolderIdsListBox As New ListBox
    Private PreviousFolderId As New ListBox
    Public pageToken As String = ""
    Private CurrentFolder As String = "root"
    ' If modifying these scopes, delete your previously saved credentials
    ' at ~/.credentials/drive-dotnet-quickstart.json
    Shared Scopes As String() = {DriveService.Scope.DriveFile, DriveService.Scope.Drive}
    Shared ApplicationName As String = "Google Drive Uploader Tool"
    Public service As DriveService
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load        'Initialize Upload Queue Collection
        Button10.Enabled = False
        If My.Settings.UploadQueue Is Nothing Then
            My.Settings.UploadQueue = New Specialized.StringCollection
        End If
        If My.Settings.UploadQueueFolders Is Nothing Then
            My.Settings.UploadQueueFolders = New Specialized.StringCollection
        End If
        If My.Settings.FoldersCreated Is Nothing Then
            My.Settings.FoldersCreated = New Specialized.StringCollection
        End If
        If My.Settings.FoldersCreatedID Is Nothing Then
            My.Settings.FoldersCreatedID = New Specialized.StringCollection
        End If
        'Checks whether the language was set. If not, apply English by default
        If String.IsNullOrEmpty(My.Settings.Language) Then
            My.Settings.Language = "English"
            My.Settings.Save()
            RadioButton1.Checked = True
            EnglishLanguage()
        Else
            If My.Settings.Language = "English" Then
                EnglishLanguage()
                RadioButton1.Checked = True
            ElseIf My.Settings.Language = "Spanish" Then

                SpanishLanguage()
                RadioButton2.Checked = True
            Else
                TChineseLanguage()
                RadioButton3.Checked = True
            End If
        End If
        'Checks if there are items to upload and if there are, we add them to the list box

        If My.Settings.UploadQueue.Count > 0 Then
            For Each item In My.Settings.UploadQueue
                ListBox2.Items.Add(item)
            Next
        End If
        If My.Settings.UploadQueueFolders.Count > 0 Then
            For Each item In My.Settings.UploadQueueFolders
                FolderToUploadFileListBox.Items.Add(item)
            Next
        End If
        'Loads the last used Folder ID
        TextBox2.Text = My.Settings.LastFolder
        'Gets Folder name from the Folder ID

        'Checks if the Preserve Modified Date checkbox was checked in the last run.
        If My.Settings.PreserveModifiedDate = True Then CheckBox1.Checked = True Else CheckBox1.Checked = False
        'Google Drive initialization
        Dim credential As UserCredential
        Try
            Using stream = New FileStream("client_secret.json", FileMode.Open, FileAccess.Read)
                Dim credPath As String = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
                Debug.WriteLine(System.Environment.SpecialFolder.Personal)
                credPath = Path.Combine(credPath, ".credentials/GoogleDriveUploaderTool.json")
                Debug.WriteLine(credPath)
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, New FileDataStore(credPath, True)).Result
                Debug.WriteLine(credential)
            End Using
        Catch
            If My.Settings.Language = "English" Then
                MsgBox("client_secret.json file not found. Please follow Step 1 in this guide: https://developers.google.com/drive/v3/web/quickstart/dotnet" & vbCr & vbCrLf & "This file should be located in the folder where this software is located.")
            ElseIf My.Settings.Language = "Spanish" Then
                MsgBox("El archivo client_secret.json no fue encontrado. Por favor, siga el Paso 1 de esta guía: https://developers.google.com/drive/v3/web/quickstart/dotnet" & vbCr & vbCrLf & "Este archivo debe estar localizado en la carpeta donde se encuentra este programa.")
            Else
                'Chinese Translation goes here
            End If
        End Try
        ' Create Drive API service.
        Dim Initializer As New BaseClientService.Initializer()
        Initializer.HttpClientInitializer = credential
        Initializer.ApplicationName = ApplicationName
        service = New DriveService(Initializer)
        service.HttpClient.Timeout = TimeSpan.FromSeconds(120)
        ' List files.
        RefreshFileList("root")
        GetFolderIDName(False)
    End Sub

    Private starttime As DateTime
    Private timespent As TimeSpan
    Private secondsremaining As Integer = 0
    Private GetFile As String = ""
    Private UploadFailed As Boolean = False
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Button2.Enabled = False
        If String.IsNullOrEmpty(TextBox2.Text) = False Then
            If GetFolderIDName(False) = True Then
                My.Settings.LastFolder = TextBox2.Text
                My.Settings.Save()
                ResumeFromError = False
                UploadFiles()
            Else
                Dim Message As String = ""
                If RadioButton1.Checked = True Then
                    Message = "The specified folder Is invalid. Do you want To change the folder? If you Select No, your files will be uploaded To the root Of Google Drive"
                Else
                    Message = "La carpeta especificada es invalida. Desea cambiar la carpeta? Si presiona No, sus archivos serán subidos a la raíz de Google Drive"
                End If
                If MsgBox(Message, MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    My.Settings.LastFolder = "root"
                    My.Settings.Save()
                    ResumeFromError = False
                    UploadFiles()
                End If
            End If
        End If
    End Sub
    Private ResumeFromError As Boolean = False
    Private Async Sub UploadFiles()
        Dim DirectoryList As New StringCollection
        Dim DirectoryListID As New StringCollection
        Dim FolderCreated As Boolean = False
        'Checks if folders where created in the last run. If there are, it will load them to the DirectoryList Variable
        If My.Settings.FolderCreated = True Then
            FolderCreated = True
            DirectoryList = My.Settings.FoldersCreated
            DirectoryListID = My.Settings.FoldersCreatedID
        End If
        While ListBox2.Items.Count > 0
            Try
                GetFile = ListBox2.Items.Item(0)
                TextBox2.Text = FolderToUploadFileListBox.Items.Item(0)
                GetFolderIDName(False)
                If System.IO.File.Exists(GetFile) Then
                    Label3.Text = String.Format("{0: N2} MB", My.Computer.FileSystem.GetFileInfo(GetFile).Length / 1024 / 1024)
                    ProgressBar1.Maximum = My.Computer.FileSystem.GetFileInfo(GetFile).Length / 1024 / 1024
                    Dim FileMetadata As New Data.File
                    FileMetadata.Name = My.Computer.FileSystem.GetName(GetFile)
                    Dim FileFolder As New List(Of String)
                    If FolderCreated = False Then
                        FileFolder.Add(TextBox2.Text)
                    Else
                        Dim DirectoryName As String = ""
                        DirectoryName = System.IO.Path.GetDirectoryName(GetFile)
                        For Each directory In DirectoryList
                            If DirectoryName = directory Then
                                FileFolder.Add(DirectoryListID.Item(DirectoryList.IndexOf(directory)))
                            End If
                        Next
                        If FileFolder.Count = 0 Then FileFolder.Add(TextBox2.Text)
                    End If
                    FileMetadata.Parents = FileFolder
                    Dim UploadStream As New FileStream(GetFile, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                    If CheckBox1.Checked Then FileMetadata.ModifiedTime = IO.File.GetLastWriteTimeUtc(GetFile)
                    Dim UploadFile As FilesResource.CreateMediaUpload = service.Files.Create(FileMetadata, UploadStream, "")
                    UploadFile.ChunkSize = ResumableUpload.MinimumChunkSize * 4
                    AddHandler UploadFile.ProgressChanged, New Action(Of IUploadProgress)(AddressOf Upload_ProgressChanged)
                    AddHandler UploadFile.ResponseReceived, New Action(Of Data.File)(AddressOf Upload_ResponseReceived)
                    AddHandler UploadFile.UploadSessionData, AddressOf Upload_UploadSessionData
                    UploadCancellationToken = New CancellationToken
                    Dim uploadUri As Uri = Nothing
                    starttime = DateTime.Now
                    If ResumeFromError = False Then
                        uploadUri = GetSessionRestartUri(True)
                    Else
                        uploadUri = GetSessionRestartUri(False)
                    End If
                    If uploadUri = Nothing Then
                        Await UploadFile.UploadAsync(UploadCancellationToken)
                    Else
                        Await UploadFile.ResumeAsync(uploadUri, UploadCancellationToken)
                    End If

                ElseIf IO.Directory.Exists(GetFile) Then
                    Dim FolderMetadata As New Data.File
                    FolderMetadata.Name = My.Computer.FileSystem.GetName(GetFile)
                    Dim ParentFolder As New List(Of String)
                    If FolderCreated = True Then
                        Dim DirectoryName As String = ""
                        DirectoryName = System.IO.Path.GetDirectoryName(GetFile)
                        For Each directory In DirectoryList
                            If DirectoryName = directory Then
                                ParentFolder.Add(DirectoryListID.Item(DirectoryList.IndexOf(directory)))
                                Exit For
                            End If
                        Next
                        If ParentFolder.Count = 0 Then ParentFolder.Add(TextBox2.Text)
                    Else
                        ParentFolder.Add(TextBox2.Text)
                    End If
                    FolderMetadata.Parents = ParentFolder
                    FolderMetadata.MimeType = "application/vnd.google-apps.folder"
                    Dim CreateFolder As FilesResource.CreateRequest = service.Files.Create(FolderMetadata)
                    CreateFolder.Fields = "id"
                    Dim FolderID As Data.File = CreateFolder.Execute
                    DirectoryList.Add(GetFile)
                    DirectoryListID.Add(FolderID.Id)
                    My.Settings.FoldersCreated.Add(GetFile)
                    My.Settings.FoldersCreatedID.Add(FolderID.Id)
                    FolderCreated = True
                    My.Settings.FolderCreated = True
                    My.Settings.Save()
                End If
            Catch ex As Exception
                UploadFailed = True
            End Try
            If UploadFailed = False Then
                ListBox2.Items.RemoveAt(0)
                FolderToUploadFileListBox.Items.RemoveAt(0)
                RefreshFileList(CurrentFolder)
                My.Settings.UploadQueue.RemoveAt(0)
                My.Settings.UploadQueueFolders.RemoveAt(0)
                My.Settings.Save()
                ResumeFromError = False
            End If
        End While
        If RadioButton1.Checked = True Then MsgBox("Uploads finished!") Else MsgBox("Los archivos han terminado de subir.")
        FolderCreated = False
        My.Settings.FolderCreated = False
        DirectoryListID.Clear()
        DirectoryList.Clear()
        My.Settings.UploadQueue.Clear()
        My.Settings.UploadQueueFolders.Clear()
        My.Settings.FoldersCreated.Clear()
        My.Settings.FoldersCreatedID.Clear()
        My.Settings.Save()
        Button2.Enabled = True
    End Sub
    Private ErrorMessage As String = ""
    Private UploadCancellationToken As System.Threading.CancellationToken
    Shared BytesSentText As Long
    Shared UploadStatusText As String
    Private Sub Upload_ProgressChanged(uploadStatusInfo As IUploadProgress)
        Select Case uploadStatusInfo.Status
            Case UploadStatus.Completed
                UploadFailed = False
                ResumeFromError = False
                If RadioButton1.Checked = True Then UploadStatusText = "Completed!!" Else UploadStatusText = "¡Completado!"
                BytesSentText = My.Computer.FileSystem.GetFileInfo(GetFile).Length
                UpdateBytesSent()
            Case UploadStatus.Starting
                If RadioButton1.Checked = True Then UploadStatusText = "Starting..." Else UploadStatusText = "Comenzando..."
                UpdateBytesSent()
            Case UploadStatus.Uploading
                UploadFailed = False
                BytesSentText = uploadStatusInfo.BytesSent
                If RadioButton1.Checked = True Then UploadStatusText = "Uploading..." Else UploadStatusText = "Subiendo..."
                timespent = DateTime.Now - starttime
                Try
                    secondsremaining = (timespent.TotalSeconds / ProgressBar1.Value * (ProgressBar1.Maximum - ProgressBar1.Value))
                Catch
                    secondsremaining = 0
                End Try
                UpdateBytesSent()
            Case UploadStatus.Failed
                UploadFailed = True
                If RadioButton1.Checked = True Then UploadStatusText = "Retrying..." Else UploadStatusText = "Intentando..."
                UpdateBytesSent()
                ResumeFromError = True
                Thread.Sleep(1000)
        End Select
    End Sub
    Private Sub Upload_ResponseReceived(file As Data.File)
        If RadioButton1.Checked = True Then UploadStatusText = "Completed!!" Else UploadStatusText = "¡Completado!"
        BytesSentText = My.Computer.FileSystem.GetFileInfo(GetFile).Length
        UpdateBytesSent()

    End Sub
    Private Sub Upload_UploadSessionData(ByVal uploadSessionData As IUploadSessionData)
        ' Save UploadUri.AbsoluteUri and FullPath Filename values for use if program faults and we want to restart the program
        My.Settings.ResumeUri = uploadSessionData.UploadUri.AbsoluteUri
        My.Settings.ResumeFilename = GetFile
        ' Saved to a user.config file within a subdirectory of C:\Users\<yourusername>\AppData\Local
        My.Settings.Save()

    End Sub
    Private Function GetSessionRestartUri(Ask As Boolean) As Uri
        If My.Settings.ResumeUri.Length > 0 AndAlso My.Settings.ResumeFilename = GetFile Then
            ' An UploadUri from a previous execution is present, ask if a resume should be attempted
            Dim ResumeText1 As String = ""
            Dim ResumeText2 As String = ""

            If RadioButton1.Checked = True Then
                ResumeText1 = "Resume previous upload?{0}{0}{1}"
                ResumeText2 = "Resume Upload"
            Else
                ResumeText1 = "¿Resumir carga anterior?{0}{0}{1}"
                ResumeText2 = "Resumir"
            End If
            If Ask = True Then
                If MsgBox(String.Format(ResumeText1, vbNewLine, GetFile), MsgBoxStyle.Question Or MsgBoxStyle.YesNo, ResumeText2) = MsgBoxResult.Yes Then
                    Return New Uri(My.Settings.ResumeUri)
                Else
                    Return Nothing
                End If
            Else
                Return New Uri(My.Settings.ResumeUri)
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
        Label4.Text = String.Format("{0:N2} MB", BytesSentText / 1024 / 1024)
        Label8.Text = UploadStatusText
        Try
            ProgressBar1.Value = BytesSentText / 1024 / 1024
        Catch

        End Try
        Label10.Text = String.Format("{0:N2}%", ((ProgressBar1.Value / ProgressBar1.Maximum) * 100))
        Dim timeFormatted As TimeSpan = TimeSpan.FromSeconds(secondsremaining)
        Label14.Text = String.Format("{0}:{1:mm}:{1:ss}", CInt(Math.Truncate(timeFormatted.TotalHours)), timeFormatted)
    End Sub
    Private Shared FileToSave As FileStream
    Private Shared MaxFileSize
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        FileIdsListBox.SelectedIndex = ListBox1.SelectedIndex
        FileSizeListBox.SelectedIndex = ListBox1.SelectedIndex
        If RadioButton1.Checked = True Then
            SaveFileDialog1.Title = "Browse for a location to save the file:"
        Else
            SaveFileDialog1.Title = "Busque un lugar para descargar el archivo:"
        End If
        SaveFileDialog1.FileName = ListBox1.SelectedItem
        Dim SFDResult As MsgBoxResult = SaveFileDialog1.ShowDialog()
        If SFDResult = MsgBoxResult.Ok Then
            starttime = DateTime.Now
            Label3.Text = String.Format("{0:N2} MB", FileSizeListBox.SelectedItem / 1024 / 1024)
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
                If RadioButton1.Checked = True Then UploadStatusText = "Completed!!" Else UploadStatusText = "¡Completado!"
                FileToSave.Close()
                BytesSentText = MaxFileSize
                UpdateBytesSent()

            Case DownloadStatus.Downloading
                BytesSentText = progress.BytesDownloaded
                If RadioButton1.Checked = True Then UploadStatusText = "Downloading..." Else UploadStatusText = "Descargando..."
                timespent = DateTime.Now - starttime
                Try
                    secondsremaining = (timespent.TotalSeconds / ProgressBar1.Value * (ProgressBar1.Maximum - ProgressBar1.Value))
                Catch
                    secondsremaining = 0
                End Try
                UpdateBytesSent()
            Case UploadStatus.Failed
                If RadioButton1.Checked = True Then UploadStatusText = "Failed..." Else UploadStatusText = "Error..."
                UpdateBytesSent()
        End Select
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        RefreshFileList(CurrentFolder)
    End Sub
    Private Delegate Sub RefreshFileListInvoker(FolderID As String)
    Private Sub RefreshFileList(FolderID As String)
        If ListBox1.InvokeRequired Then
            ListBox1.Invoke(New RefreshFileListInvoker(AddressOf RefreshFileList), FolderID)
        End If
        If ListBox3.InvokeRequired Then
            ListBox3.Invoke(New RefreshFileListInvoker(AddressOf RefreshFileList), FolderID)
            'Dim method As MethodInvoker = New MethodInvoker(AddressOf RefreshFileList)
            'Invoke(method)
        End If
        ListBox1.Items.Clear()
        FileIdsListBox.Items.Clear()
        FileSizeListBox.Items.Clear()
        FileModifiedTimeListBox.Items.Clear()
        FileCreatedTimeListBox.Items.Clear()
        FileMD5ListBox.Items.Clear()
        FileMIMEListBox.Items.Clear()
        Dim PageToken1 As String = String.Empty
        Do
            Dim listRequest1 As FilesResource.ListRequest = service.Files.List()
            listRequest1.Fields = "nextPageToken, files(id, name, size, createdTime, modifiedTime, md5Checksum, mimeType)"
            listRequest1.Q = "mimeType!='application/vnd.google-apps.folder' and '" & FolderID & "' in parents and trashed = false"
            listRequest1.OrderBy = "name"
            listRequest1.PageToken = PageToken1
            Try
                Dim files = listRequest1.Execute()
                If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
                    For Each file In files.Files
                        ListBox1.Items.Add(file.Name)
                        FileIdsListBox.Items.Add(file.Id)
                        Try
                            FileSizeListBox.Items.Add(file.Size)
                            FileModifiedTimeListBox.Items.Add(file.ModifiedTime)
                            FileCreatedTimeListBox.Items.Add(file.CreatedTime)
                            FileMD5ListBox.Items.Add(file.Md5Checksum)
                            FileMIMEListBox.Items.Add(file.MimeType)
                        Catch
                            FileSizeListBox.Items.Add("0")
                            FileMIMEListBox.Items.Add("Unknown")
                            FileModifiedTimeListBox.Items.Add(file.ModifiedTime)
                            FileCreatedTimeListBox.Items.Add(file.CreatedTime)
                            FileMD5ListBox.Items.Add("")
                        End Try
                    Next
                End If
                PageToken1 = files.NextPageToken
            Catch ex As Exception
            End Try
        Loop While PageToken1 = String.Empty = False
        ListBox3.Items.Clear()
        FolderIdsListBox.Items.Clear()
        Dim PageToken2 As String = String.Empty
        Do
            Dim listRequest As FilesResource.ListRequest = service.Files.List()
            listRequest.Q = "mimeType='application/vnd.google-apps.folder' and '" & FolderID & "' in parents and trashed = false"
            listRequest.Fields = "nextPageToken, files(id, name)"
            listRequest.OrderBy = "name"
            listRequest.PageToken = PageToken2
            Try
                Dim files = listRequest.Execute()
                If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
                    For Each file In files.Files
                        ListBox3.Items.Add(file.Name)
                        FolderIdsListBox.Items.Add(file.Id)
                    Next
                End If
                PageToken2 = files.NextPageToken
            Catch ex As Exception
            End Try
        Loop While PageToken2 = String.Empty = False
        If CurrentFolder = "root" Then
            Button10.Enabled = False
        Else
            Button10.Enabled = True
        End If
    End Sub
    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        Dim filepath() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each path In filepath
            If System.IO.Directory.Exists(path) Then
                ListBox2.Items.Add(path)
                FolderToUploadFileListBox.Items.Add(TextBox2.Text)
                My.Settings.UploadQueue.Add(path)
                My.Settings.UploadQueueFolders.Add(TextBox2.Text)
                GetDirectoriesAndFiles(New IO.DirectoryInfo(path))
            Else
                ListBox2.Items.Add(path)
                FolderToUploadFileListBox.Items.Add(TextBox2.Text)
                My.Settings.UploadQueue.Add(path)
                My.Settings.UploadQueueFolders.Add(TextBox2.Text)
            End If
        Next
        My.Settings.Save()
    End Sub
    Private Sub GetDirectoriesAndFiles(ByVal BaseFolder As IO.DirectoryInfo)
        ListBox2.Items.AddRange((From FI As IO.FileInfo In BaseFolder.GetFiles Select FI.FullName).ToArray)
        My.Settings.UploadQueue.AddRange((From FI As IO.FileInfo In BaseFolder.GetFiles Select FI.FullName).ToArray)
        For Each subF As IO.DirectoryInfo In BaseFolder.GetDirectories()
            Application.DoEvents()
            ListBox2.Items.Add(subF.FullName)
            FolderToUploadFileListBox.Items.Add(TextBox2.Text)
            My.Settings.UploadQueue.Add(subF.FullName)
            My.Settings.UploadQueueFolders.Add(TextBox2.Text)
            GetDirectoriesAndFiles(subF)
        Next
        My.Settings.Save()
    End Sub
    Private Sub Form1_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        EnglishLanguage()
        My.Settings.Language = "English"
        My.Settings.Save()
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        SpanishLanguage()
        My.Settings.Language = "Spanish"
        My.Settings.Save()
    End Sub
    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        TChineseLanguage()
        My.Settings.Language = "TChinese"
        My.Settings.Save()
    End Sub
    Private Sub EnglishLanguage()
        Label1.Text = "File Size:"
        Label2.Text = "Processed:"
        Label5.Text = "Drag and Drop Files to add them to the list"
        Label6.Text = "By Moisés Cardona" & vbNewLine & "v1.7"
        Label7.Text = "Status:"
        Label9.Text = "Percent: "
        Label11.Text = "Files:"
        Label12.Text = "Upload to this folder ID (""root"" to upload to root folder):"
        Label13.Text = "Time Left: "
        Label15.Text = "Like this software?"
        Label16.Text = "Folder Name:"
        Label17.Text = "Folders:"
        Label18.Text = "File Name:"
        Label19.Text = "File ID:"
        Label20.Text = "Date Created:"
        Label21.Text = "Date Modified:"
        Label22.Text = "MD5 Checksum:"
        Label23.Text = "MIME Type:"
        Label24.Text = "File Size:"
        Button1.Text = "Save Checksum File"
        Button2.Text = "Upload"
        Button3.Text = "Clear List"
        Button4.Text = "Refresh List"
        Button5.Text = "Download File"
        Button6.Text = "Remove selected file(s) from list"
        Button7.Text = "Save Checksums for Selected Files"
        Button8.Text = "Donations"
        Button9.Text = "Get Folder Name"
        Button10.Text = "Back"
        GroupBox2.Text = "File Information:"
        If Button11.Text = "Ver Basura" Then
            Button11.Text = "View Trash"
        ElseIf Button11.Text = "Ver Drive" Then
            Button11.Text = "View Drive"
        End If
        Button12.Text = "Create New Folder"
        CheckBox1.Text = "Preserve File Modified Date"
        btnLogout.Text = "Logout"
    End Sub
    Private Sub TChineseLanguage()
        Label1.Text = "文件大小:"
        Label2.Text = "Processed:"
        Label5.Text = "請將文件拖到下方"
        Label6.Text = "By Moisés Cardona" & vbNewLine & "v1.7" & vbNewLine & "Translated by mic4126"
        Label7.Text = "狀態:"
        Label9.Text = "百份比: "
        Label11.Text = "文件:"
        Label12.Text = "上傳到此文件夾ID (""root"" 指上傳到根目錄):"
        Label13.Text = "餘下時間: "
        Label15.Text = "喜歡此軟件?"
        Label16.Text = "文件夾名稱:"
        Label18.Text = "文件名稱:"
        Label19.Text = "文件ID:"
        Label20.Text = "新建日期:"
        Label21.Text = "修改日期:"
        Label22.Text = "MD5 校驗碼:"
        Label23.Text = "MIME Type:"
        Label24.Text = "文件大小:"
        Button1.Text = "儲存校驗碼"
        Button2.Text = "上傳"
        Button3.Text = "清除列表"
        Button4.Text = "更新列表"
        Button5.Text = "下載文件"
        Button6.Text = "由列表中移除已選文件"
        Button7.Text = "儲存已選文件校驗碼"
        Button8.Text = "捐款"
        Button9.Text = "獲取文件夾名稱"
        Button10.Text = "返回"
        Button12.Text = "新增文件夾"
        CheckBox1.Text = "保留文件修改日期"
        btnLogout.Text = "登岀"
    End Sub
    Private Sub SpanishLanguage()
        Label1.Text = "Tamaño:"
        Label2.Text = "Procesado:"
        Label5.Text = "Arrastre archivos aquí para añadirlos a la lista"
        Label6.Text = "Por Moisés Cardona" & vbNewLine & "v1.7"
        Label7.Text = "Estado:"
        Label9.Text = "Porcentaje: "
        Label11.Text = "Archivos:"
        Label12.Text = "Subir a este ID de directorio (""root"" para subir a la raíz):"
        Label13.Text = "Tiempo Est."
        Label15.Text = "¿Te gusta esta programa?"
        Label16.Text = "Nombre de la Carpeta:"
        Label17.Text = "Carpetas:"
        Label18.Text = "Nombre:"
        Label19.Text = "ID:"
        Label20.Text = "Fecha Creada:"
        Label21.Text = "Fecha Modificada:"
        Label22.Text = "Checksum MD5:"
        Label23.Text = "Tipo MIME:"
        Label24.Text = "Tamaño:"
        Button1.Text = "Guardar Archivo MD5"
        Button2.Text = "Subir"
        Button3.Text = "Borrar Lista"
        Button4.Text = "Refrescar Lista"
        Button5.Text = "Descargar Archivo"
        Button6.Text = "Remover archivo(s) de la lista"
        Button7.Text = "Guardar Checksums de los archivos"
        Button8.Text = "Donar"
        Button9.Text = "Obtener Nombre de la Carpeta"
        Button10.Text = "Atrás"
        GroupBox2.Text = "Información del archivo:"
        If Button11.Text = "View Trash" Then
            Button11.Text = "Ver Basura"
        ElseIf Button11.Text = "View Drive" Then
            Button11.Text = "Ver Drive"
        End If
        Button12.Text = "Crear Nueva Carpeta"
        CheckBox1.Text = "Preservar Fecha de Modificación del Archivo"
        btnLogout.Text = "Cerrar Sesión"
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Do While (ListBox2.SelectedItems.Count > 0)
            Dim CurrentIndex = ListBox2.SelectedIndex
            ListBox2.Items.RemoveAt(CurrentIndex)
            My.Settings.UploadQueue.RemoveAt(CurrentIndex)
            FolderToUploadFileListBox.Items.RemoveAt(CurrentIndex)
            My.Settings.UploadQueueFolders.RemoveAt(CurrentIndex)
            My.Settings.Save()
        Loop
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            My.Settings.PreserveModifiedDate = True
            My.Settings.Save()
        Else
            My.Settings.PreserveModifiedDate = False
            My.Settings.Save()
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        My.Settings.LastFolder = TextBox2.Text
        My.Settings.Save()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListBox2.Items.Clear()
        My.Settings.UploadQueue.Clear()
        FolderToUploadFileListBox.Items.Clear()
        My.Settings.UploadQueueFolders.Clear()
        My.Settings.Save()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Donations.ShowDialog()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        GetFolderIDName(True)
    End Sub

    Private Function GetFolderIDName(ShowMessage As Boolean)
        If String.IsNullOrEmpty(TextBox2.Text) = False Then
            Try
                Dim GetFolderName As FilesResource.GetRequest = service.Files.Get(TextBox2.Text.ToString)
                Dim FolderNameMetadata As Data.File = GetFolderName.Execute
                TextBox1.Text = FolderNameMetadata.Name
                Return True
            Catch ex As Exception
                If ShowMessage = True Then MsgBox("Folder ID is incorrect.")
                Return False
            End Try
        Else
            Return False
        End If
    End Function

    Private Sub ListBox3_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListBox3.MouseDoubleClick
        EnterFolder()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If Button11.Text = "View Trash" Or Button11.Text = "Ver Basura" Then
            If CurrentFolder = "root" = False Then
                Dim PreviousFolderIdBeforeRemoving = PreviousFolderId.Items.Item(PreviousFolderId.Items.Count - 1)
                PreviousFolderId.Items.RemoveAt(PreviousFolderId.Items.Count - 1)
                CurrentFolder = PreviousFolderIdBeforeRemoving
                RefreshFileList(PreviousFolderIdBeforeRemoving)
                TextBox2.Text = CurrentFolder
                GetFolderIDName(False)
                If CurrentFolder = "root" Then
                    Button10.Enabled = False
                Else
                    Button10.Enabled = True
                End If
            End If
        Else
            ViewTrashedFiles()
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If String.IsNullOrEmpty(ListBox1.SelectedItem) = False Then
            TextBox3.Text = ListBox1.SelectedItem
            TextBox4.Text = FileIdsListBox.Items.Item(ListBox1.SelectedIndex)
            TextBox5.Text = FileCreatedTimeListBox.Items.Item(ListBox1.SelectedIndex)
            TextBox6.Text = FileModifiedTimeListBox.Items.Item(ListBox1.SelectedIndex)
            TextBox7.Text = FileMD5ListBox.Items.Item(ListBox1.SelectedIndex)
            TextBox8.Text = FileMIMEListBox.Items.Item(ListBox1.SelectedIndex)
            TextBox9.Text = String.Format("{0:N2} MB", FileSizeListBox.Items.Item(ListBox1.SelectedIndex) / 1024 / 1024)
        Else
            TextBox3.Text = ""
            TextBox4.Text = ""
            TextBox8.Text = ""
            TextBox5.Text = ""
            TextBox6.Text = ""
            TextBox7.Text = ""
            TextBox9.Text = ""
        End If
        If ListBox1.SelectedItems.Count > 1 Then
            Button7.Visible = True
        Else
            Button7.Visible = False
        End If
    End Sub


    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Dim FolderNameToCreate As Object
        Dim Message, Title As String
        If RadioButton1.Checked = True Then
            Message = "Enter a name for the new folder:"
            Title = "Create new Folder"
        Else
            Message = "Escriba un nombre para la nueva carpeta:"
            Title = "Crear nueva carpeta"
        End If
        FolderNameToCreate = InputBox(Message, Title)
        If String.IsNullOrEmpty(FolderNameToCreate) = False Then
            Dim FolderMetadata As New Data.File
            FolderMetadata.Name = FolderNameToCreate
            Dim ParentFolder As New List(Of String)
            ParentFolder.Add(CurrentFolder)
            FolderMetadata.Parents = ParentFolder
            FolderMetadata.MimeType = "application/vnd.google-apps.folder"
            Dim CreateFolder As FilesResource.CreateRequest = service.Files.Create(FolderMetadata)
            CreateFolder.Fields = "id"
            Dim FolderID As Data.File = CreateFolder.Execute
            If TextBox1.Text = "root" Then
                PreviousFolderId.Items.Add("root")
            Else
                PreviousFolderId.Items.Add(CurrentFolder)
            End If
            TextBox1.Text = FolderNameToCreate
            TextBox2.Text = FolderID.Id
            CurrentFolder = FolderID.Id
            RefreshFileList(FolderID.Id)
        End If
    End Sub

    Private Sub ListBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox3.SelectedIndexChanged
        If String.IsNullOrEmpty(ListBox3.SelectedItem) = False Then
            TextBox2.Text = FolderIdsListBox.Items.Item(ListBox3.SelectedIndex)
            TextBox1.Text = ListBox3.SelectedItem
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If String.IsNullOrEmpty(TextBox7.Text) = False Then
            If RadioButton1.Checked = True Then
                SaveFileDialog2.Title = "Browse for a location to save the checksum file:"
            Else
                SaveFileDialog2.Title = "Busque un lugar para guardar el archivo del checksum:"
            End If
            SaveFileDialog2.FileName = ListBox1.SelectedItem & ".md5"
            SaveFileDialog2.Filter = "MD5 Checksum|*.md5"
            Dim SFDResult As MsgBoxResult = SaveFileDialog2.ShowDialog()
            If SFDResult = MsgBoxResult.Ok Then
                My.Computer.FileSystem.WriteAllText(SaveFileDialog2.FileName, TextBox7.Text & " *" & ListBox1.SelectedItem, False)
            End If
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If RadioButton1.Checked = True Then
            SaveFileDialog2.Title = "Browse for a location to save the checksum file:"
        Else
            SaveFileDialog2.Title = "Busque un lugar para guardar el archivo del checksum:"
        End If
        SaveFileDialog2.FileName = "checksums.md5"
        SaveFileDialog2.Filter = "MD5 Checksum|*.md5"
        Dim SFDResult As MsgBoxResult = SaveFileDialog2.ShowDialog()
        If SFDResult = MsgBoxResult.Ok Then
            Dim Checksumfile As StreamWriter = New StreamWriter(SaveFileDialog2.FileName)
            For Each item In ListBox1.SelectedItems
                Checksumfile.WriteLine(FileMD5ListBox.Items.Item(ListBox1.Items.IndexOf(item)) & " *" & item)
            Next
            Checksumfile.Close()
            MsgBox("Checksums saved")
        End If


    End Sub

    Private Sub ListBox3_KeyDown(sender As Object, e As KeyEventArgs) Handles ListBox3.KeyDown
        If e.KeyCode = Keys.Delete Then
            If ListBox3.SelectedItems.Count > 1 Then
                Dim Message As String = String.Empty
                If RadioButton1.Checked Then
                    Message = "Do you really want to move the selected folders to the Trash?"
                Else
                    Message = "¿Está seguro de querer mover las carpetas seleccionadas a la Basura?"
                End If
                If MsgBox(Message, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    Dim FileMetadata As New Data.File
                    FileMetadata.Trashed = True
                    For Each item In ListBox3.SelectedItems
                        Dim RemoveFile As FilesResource.UpdateRequest = service.Files.Update(FileMetadata, FolderIdsListBox.Items.Item(ListBox3.Items.IndexOf(item)))
                        RemoveFile.ExecuteAsync()
                    Next
                    Thread.Sleep(1000)
                    RefreshFileList(CurrentFolder)
                    If RadioButton1.Checked Then
                        MsgBox("Folder moved to trash")
                    Else
                        MsgBox("La carpeta se movió a la basura")
                    End If
                End If
            Else
                Dim Message As String = String.Empty
                If RadioButton1.Checked Then
                    Message = "Do you really want to move the folder """ & ListBox3.SelectedItem & """ to the Trash?"
                Else
                    Message = "¿Está seguro de querer mover la carpeta """ & ListBox3.SelectedItem & """ a la Basura?"
                End If
                If MsgBox(Message, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    Dim FileMetadata As New Data.File
                    FileMetadata.Trashed = True
                    Dim RemoveFile As FilesResource.UpdateRequest = service.Files.Update(FileMetadata, FolderIdsListBox.Items.Item(ListBox3.SelectedIndex))
                    RemoveFile.ExecuteAsync()
                    Thread.Sleep(1000)
                    RefreshFileList(CurrentFolder)
                    If RadioButton1.Checked Then
                        MsgBox("Folder moved to trash")
                    Else
                        MsgBox("La carpeta se movió a la basura")
                    End If
                End If
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            EnterFolder()
        ElseIf e.KeyCode = Keys.F5 Then
            RefreshFileList(CurrentFolder)
            e.Handled = True
        ElseIf e.Modifiers = Keys.Alt And e.KeyCode = Keys.R Then
            If Button11.Text = "View Drive" Or Button11.Text = "Ver Drive" Then
                If ListBox3.SelectedItems.Count > 1 Then
                    Dim Message As String = String.Empty
                    If RadioButton1.Checked = True Then
                        Message = "Do you want to restore the selected folders?"
                    Else
                        Message = "¿Está seguro de querer restaurar las carpetas seleccionados?"
                    End If
                    If MsgBox(Message, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        Dim FileMetadata As New Data.File
                        FileMetadata.Trashed = False
                        For Each item In ListBox3.SelectedItems
                            Dim RemoveFile As FilesResource.UpdateRequest = service.Files.Update(FileMetadata, FolderIdsListBox.Items.Item(ListBox3.Items.IndexOf(item)))
                            RemoveFile.ExecuteAsync()
                        Next
                        Thread.Sleep(1000)
                        ViewTrashedFiles()
                        If RadioButton1.Checked = True Then
                            MsgBox("Folders restored")
                        Else
                            MsgBox("Las carpetas han sido restaurados")
                        End If
                    End If
                Else
                    Dim Message As String = String.Empty
                    If RadioButton1.Checked = True Then
                        Message = "Do you want to restore the folder """ & ListBox3.SelectedItem & """ ?"
                    Else
                        Message = "¿Está seguro de querer restaurar la carpeta """ & ListBox3.SelectedItem & """?"
                    End If
                    If MsgBox(Message, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        Dim FileMetadata As New Data.File
                        FileMetadata.Trashed = False
                        Dim RemoveFile As FilesResource.UpdateRequest = service.Files.Update(FileMetadata, FolderIdsListBox.Items.Item(ListBox3.SelectedIndex))
                        RemoveFile.ExecuteAsync()
                        Thread.Sleep(1000)
                        ViewTrashedFiles()
                        If RadioButton1.Checked = True Then
                            MsgBox("Folder restored")
                        Else
                            MsgBox("La carpeta ha sido restaurado")
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub EnterFolder()
        If String.IsNullOrEmpty(ListBox3.SelectedItem) = False Then
            Dim GoToFolderID As String = FolderIdsListBox.Items.Item(ListBox3.SelectedIndex)
            PreviousFolderId.Items.Add(CurrentFolder)
            CurrentFolder = FolderIdsListBox.Items.Item(ListBox3.SelectedIndex)
            Button7.Visible = False
            Button10.Enabled = True
            RefreshFileList(GoToFolderID)
        End If
    End Sub

    Private Sub ListBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles ListBox1.KeyDown
        If e.KeyCode = Keys.Delete Then
            If ListBox1.SelectedItems.Count > 1 Then
                Dim Message As String = String.Empty
                If RadioButton1.Checked = True Then
                    Message = "Do you really want to move the selected files to the Trash?"
                Else
                    Message = "¿Está seguro de querer mover los archivos seleccionados a la Basura?"
                End If
                If MsgBox(Message, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    Dim FileMetadata As New Data.File
                    FileMetadata.Trashed = True
                    For Each item In ListBox1.SelectedItems
                        Dim RemoveFile As FilesResource.UpdateRequest = service.Files.Update(FileMetadata, FileIdsListBox.Items.Item(ListBox1.Items.IndexOf(item)))
                        RemoveFile.ExecuteAsync()
                    Next
                    Thread.Sleep(1000)
                    RefreshFileList(CurrentFolder)
                    If RadioButton1.Checked = True Then
                        MsgBox("Files moved to trash")
                    Else
                        MsgBox("Los archivos se movieron a la basura.")
                    End If
                End If
            Else
                Dim Message As String = String.Empty
                If RadioButton1.Checked = True Then
                    Message = "Do you really want to move the file """ & ListBox1.SelectedItem & """ to the Trash?"
                Else
                    Message = "¿Está seguro de querer mover el archivo """ & ListBox1.SelectedItem & """ a la Basura?"
                End If
                If MsgBox(Message, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    Dim FileMetadata As New Data.File
                    FileMetadata.Trashed = True
                    Dim RemoveFile As FilesResource.UpdateRequest = service.Files.Update(FileMetadata, FileIdsListBox.Items.Item(ListBox1.SelectedIndex))
                    RemoveFile.ExecuteAsync()
                    Thread.Sleep(1000)
                    RefreshFileList(CurrentFolder)
                    If RadioButton1.Checked = True Then
                        MsgBox("File moved to trash")
                    Else
                        MsgBox("El archivo se movió a la basura.")
                    End If
                End If
            End If
        ElseIf e.KeyCode = Keys.F5 Then
            RefreshFileList(CurrentFolder)
            e.Handled = True
        ElseIf e.Modifiers = Keys.Alt And e.KeyCode = Keys.R Then
            If Button11.Text = "View Drive" Or Button11.Text = "Ver Drive" Then
                If ListBox1.SelectedItems.Count > 1 Then
                    Dim Message As String = String.Empty
                    If RadioButton1.Checked = True Then
                        Message = "Do you want to restore the selected files?"
                    Else
                        Message = "¿Está seguro de querer restaurar los archivos seleccionados?"
                    End If
                    If MsgBox(Message, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        Dim FileMetadata As New Data.File
                        FileMetadata.Trashed = False
                        For Each item In ListBox1.SelectedItems
                            Dim RemoveFile As FilesResource.UpdateRequest = service.Files.Update(FileMetadata, FileIdsListBox.Items.Item(ListBox1.Items.IndexOf(item)))
                            RemoveFile.ExecuteAsync()
                        Next
                        Thread.Sleep(1000)
                        ViewTrashedFiles()
                        If RadioButton1.Checked = True Then
                            MsgBox("Files restored")
                        Else
                            MsgBox("Los archivos han sido restaurados")
                        End If
                    End If
                Else
                    Dim Message As String = String.Empty
                    If RadioButton1.Checked = True Then
                        Message = "Do you want to restore the file """ & ListBox1.SelectedItem & """ ?"
                    Else
                        Message = "¿Está seguro de querer restaurar el archivo """ & ListBox1.SelectedItem & """?"
                    End If
                    If MsgBox(Message, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        Dim FileMetadata As New Data.File
                        FileMetadata.Trashed = False
                        Dim RemoveFile As FilesResource.UpdateRequest = service.Files.Update(FileMetadata, FileIdsListBox.Items.Item(ListBox1.SelectedIndex))
                        RemoveFile.ExecuteAsync()
                        Thread.Sleep(1000)
                        ViewTrashedFiles()
                        If RadioButton1.Checked = True Then
                            MsgBox("File restored")
                        Else
                            MsgBox("El archivo ha sido restaurado")
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim credPath As String = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
        credPath = Path.Combine(credPath, ".credentials\GoogleDriveUploaderTool.json")
        Dim credfiles As String() = Directory.GetFiles(credPath, "*.TokenResponse-user")
        For Each credfile In credfiles
            Debug.WriteLine(credfile)
            File.Delete(credfile)
        Next
        Application.Exit()
    End Sub
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If Button11.Text = "View Trash" Or Button11.Text = "Ver Basura" Then
            ViewTrashedFiles()
            If RadioButton1.Checked Then
                Button11.Text = "View Drive"
            Else
                Button11.Text = "Ver Drive"
            End If
        Else
            If RadioButton1.Checked Then
                Button11.Text = "View Trash"
            Else
                Button11.Text = "Ver Basura"
            End If
            PreviousFolderId.Items.Clear()
            FolderIdsListBox.Items.Clear()
            RefreshFileList("root")
        End If

    End Sub
    Private Sub ViewTrashedFiles()
        CurrentFolder = "root"
        If ListBox1.InvokeRequired Then
            ListBox1.Invoke(New RefreshFileListInvoker(AddressOf ViewTrashedFiles))
        End If
        If ListBox3.InvokeRequired Then
            ListBox3.Invoke(New RefreshFileListInvoker(AddressOf ViewTrashedFiles))
            'Dim method As MethodInvoker = New MethodInvoker(AddressOf RefreshFileList)
            'Invoke(method)
        End If
        ListBox1.Items.Clear()
        FileIdsListBox.Items.Clear()
        FileSizeListBox.Items.Clear()
        FileModifiedTimeListBox.Items.Clear()
        FileCreatedTimeListBox.Items.Clear()
        FileMD5ListBox.Items.Clear()
        FileMIMEListBox.Items.Clear()
        PreviousFolderId.Items.Clear()
        Dim PageToken1 As String = String.Empty
        Do
            Dim listRequest1 As FilesResource.ListRequest = service.Files.List()
            listRequest1.Fields = "nextPageToken, files(id, name, size, createdTime, modifiedTime, md5Checksum, mimeType)"
            listRequest1.Q = "mimeType!='application/vnd.google-apps.folder' and trashed = true"
            listRequest1.OrderBy = "name"
            listRequest1.PageToken = PageToken1
            Try
                Dim files = listRequest1.Execute()
                If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
                    For Each file In files.Files
                        ListBox1.Items.Add(file.Name)
                        FileIdsListBox.Items.Add(file.Id)
                        Try
                            FileSizeListBox.Items.Add(file.Size)
                            FileModifiedTimeListBox.Items.Add(file.ModifiedTime)
                            FileCreatedTimeListBox.Items.Add(file.CreatedTime)
                            FileMD5ListBox.Items.Add(file.Md5Checksum)
                            FileMIMEListBox.Items.Add(file.MimeType)
                        Catch
                            FileSizeListBox.Items.Add("0")
                            FileMIMEListBox.Items.Add("Unknown")
                            FileModifiedTimeListBox.Items.Add(file.ModifiedTime)
                            FileCreatedTimeListBox.Items.Add(file.CreatedTime)
                            FileMD5ListBox.Items.Add("")
                        End Try
                    Next
                End If
                PageToken1 = files.NextPageToken
            Catch ex As Exception
            End Try
        Loop While PageToken1 = String.Empty = False
        ListBox3.Items.Clear()
        FolderIdsListBox.Items.Clear()
        Dim PageToken2 As String = String.Empty
        Do
            Dim listRequest As FilesResource.ListRequest = service.Files.List()
            listRequest.Q = "mimeType='application/vnd.google-apps.folder'and trashed = true"
            listRequest.Fields = "nextPageToken, files(id, name)"
            listRequest.OrderBy = "name"
            listRequest.PageToken = PageToken2
            Try
                Dim files = listRequest.Execute()
                If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
                    For Each file In files.Files
                        ListBox3.Items.Add(file.Name)
                        FolderIdsListBox.Items.Add(file.Id)
                    Next
                End If
                PageToken2 = files.NextPageToken
            Catch ex As Exception
            End Try
        Loop While PageToken2 = String.Empty = False
        Button10.Enabled = False
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        If ListBox2.SelectedIndex <> -1 Then
            TextBox2.Text = ListBox2.Items.Item(ListBox2.SelectedIndex)
            GetFolderIDName(False)
        End If
    End Sub
End Class
