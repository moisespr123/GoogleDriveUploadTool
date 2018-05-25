Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Drive.v3
Imports Google.Apis.Services
Imports Google.Apis.Util.Store
Imports System.IO
Imports System.Threading
Imports Google.Apis.Upload
Imports Google.Apis.Download
Imports System.Collections.Specialized

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
    Shared SoftwareName As String = "Google Drive Uploader Tool"
    Public service As DriveService
    Dim viewing_trash As Boolean = False
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load         'Initialize Upload Queue Collection
        Button10.Enabled = False
        If My.Settings.UploadQueue Is Nothing Then
            My.Settings.UploadQueue = New StringCollection
        End If
        If My.Settings.UploadQueueFolders Is Nothing Then
            My.Settings.UploadQueueFolders = New StringCollection
        End If
        If My.Settings.FoldersCreated Is Nothing Then
            My.Settings.FoldersCreated = New StringCollection
        End If
        If My.Settings.FoldersCreatedID Is Nothing Then
            My.Settings.FoldersCreatedID = New StringCollection
        End If
        If My.Settings.PreviousFolderIDs Is Nothing Then
            My.Settings.PreviousFolderIDs = New StringCollection
        End If

        'Checks whether the language was set. If not, apply English by default
        Lang_Select()

        'Checks if there are items to upload and if there are, we add them to the list box
        If My.Settings.UploadQueue.Count > 0 Then
            For Each item In My.Settings.UploadQueue
                UploadsListBox.Items.Add(item)
            Next
        End If
        If My.Settings.UploadQueueFolders.Count > 0 Then
            For Each item In My.Settings.UploadQueueFolders
                FolderToUploadFileListBox.Items.Add(item)
            Next
        End If
        'Google Drive initialization
        Dim credential As UserCredential = Nothing
        Try
            Using stream = New FileStream("client_secret.json", FileMode.Open, FileAccess.Read)
                Dim credPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                Debug.WriteLine(Environment.SpecialFolder.Personal)
                credPath = Path.Combine(credPath, ".credentials/GoogleDriveUploaderTool.json")
                Debug.WriteLine(credPath)
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, New FileDataStore(credPath, True)).Result
                Debug.WriteLine(credential)
            End Using
            ' Create Drive API service.
            Dim Initializer As New BaseClientService.Initializer With {
                .HttpClientInitializer = credential,
                .ApplicationName = SoftwareName
            }
            service = New DriveService(Initializer)
            service.HttpClient.Timeout = TimeSpan.FromSeconds(120)
            'Loads the last used Folder ID and lists files
            If My.Settings.PreviousFolderIDs.Count > 0 Then
                CurrentFolder = My.Settings.LastFolder 'My.Settings.PreviousFolderIDs.Item(My.Settings.PreviousFolderIDs.Count - 1)
            Else
                CurrentFolder = "root"
            End If
            For Each item In My.Settings.PreviousFolderIDs
                PreviousFolderId.Items.Add(item)
            Next
            SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Checked = My.Settings.SaveAsChecksumsMD5
            StartUploadsAutomaticallyToolStripMenuItem.Checked = My.Settings.AutomaticUploads
            PreserveFileModifiedDateToolStripMenuItem.Checked = My.Settings.PreserveModifiedDate
            UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Checked = My.Settings.UpdateViews
            OrderByComboBox.SelectedIndex = My.Settings.SortByIndex
            DescendingOrderToolStripMenuItem.Checked = My.Settings.OrderDesc
            CopyFileToRAMBeforeUploadingToolStripMenuItem.Checked = My.Settings.CopyToRAM
            RefreshFileList(CurrentFolder)

            CurrentFolderLabel.Text = GetCurrentFolderIDName()
            UpdateQuota()
            If UploadsListBox.Items.Count > 0 Then
                UploadsListBox.SelectedIndex = 0
            Else
                FolderIDTextBox.Text = CurrentFolder
                GetFolderIDName(False)
            End If
        Catch
            If My.Settings.Language = "English" Then
                MsgBox("client_secret.json file not found. Please follow Step 1 in this guide: https://developers.google.com/drive/v3/web/quickstart/dotnet" & vbCr & vbCrLf & "This file should be located in the folder where this software is located.")
            ElseIf My.Settings.Language = "Spanish" Then
                MsgBox("El archivo client_secret.json no fue encontrado. Por favor, siga el Paso 1 de esta guía: https://developers.google.com/drive/v3/web/quickstart/dotnet" & vbCr & vbCrLf & "Este archivo debe estar localizado en la carpeta donde se encuentra este programa.")
            Else
                'Chinese Translation goes here
                MsgBox("client_secret.json 檔案找不到.請做: https://developers.google.com/drive/v3/web/quickstart/dotnet" & "的第一歩" & vbCr & vbCrLf & "請將client_secret.json放到軟體的根目錄.")
            End If
            Process.Start("https://developers.google.com/drive/v3/web/quickstart/dotnet")
            Me.Close()
        End Try

    End Sub

    Public Sub Lang_Select()
        If String.IsNullOrEmpty(My.Settings.Language) Then
            My.Settings.Language = "English"
            My.Settings.Save()
            RadioButton1.Checked = True
            EnglishLanguage()
        Else
            Select Case My.Settings.Language
                Case "English"
                    EnglishLanguage()
                    RadioButton1.Checked = True
                Case "Spanish"
                    SpanishLanguage()
                    RadioButton2.Checked = True
                Case "TChinese"
                    TChineseLanguage()
                    RadioButton3.Checked = True
                Case Else
                    EnglishLanguage()
                    RadioButton1.Checked = True
            End Select
        End If
    End Sub

    Private starttime As DateTime
    Private timespent As TimeSpan
    Private GetFile As String = ""
    Private UploadFailed As Boolean = False
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles UploadButton.Click
        CheckBeforeStartingUpload()
    End Sub
    Private ResumeFromError As Boolean = False
    Private Sub CheckBeforeStartingUpload()
        If UploadsListBox.Items.Count > 0 Then
            FolderIDTextBox.Text = FolderToUploadFileListBox.Items.Item(0).ToString
            If GetFolderIDName(False) Then
                My.Settings.LastFolder = CurrentFolder
                My.Settings.Save()
                ResumeFromError = False
                UploadButton.Enabled = False
                UploadFiles()
            Else
                Dim Message As String = MsgAndDialogLang("folder_invaild")
                If MsgBox(Message, MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    My.Settings.LastFolder = "root"
                    My.Settings.Save()
                    ResumeFromError = False
                    UploadFiles()
                End If
            End If
        End If
    End Sub
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
        While UploadsListBox.Items.Count > 0
            UploadsListBox.SelectedIndex() = 0
            Try
                GetFile = UploadsListBox.Items.Item(0).ToString
                FolderIDTextBox.Text = FolderToUploadFileListBox.Items.Item(0).ToString
                GetFolderIDName(False)
                If System.IO.File.Exists(GetFile) Then
                    FileSizeLabel.Text = String.Format("{0:N2} MB", My.Computer.FileSystem.GetFileInfo(GetFile).Length / 1024 / 1024)
                    ProgressBar1.Maximum = CInt(My.Computer.FileSystem.GetFileInfo(GetFile).Length / 1024 / 1024)
                    Dim FileMetadata As New Data.File With {
                        .Name = My.Computer.FileSystem.GetName(GetFile)
                    }
                    If My.Settings.PreserveModifiedDate Then FileMetadata.ModifiedTime = IO.File.GetLastWriteTimeUtc(GetFile)
                    Dim FileFolder As New List(Of String)
                    If FolderCreated = False Then
                        FileFolder.Add(FolderIDTextBox.Text)
                    Else
                        Dim DirectoryName As String = ""
                        DirectoryName = System.IO.Path.GetDirectoryName(GetFile)
                        For Each directory In DirectoryList
                            If DirectoryName = directory Then
                                FileFolder.Add(DirectoryListID.Item(DirectoryList.IndexOf(directory)))
                            End If
                        Next
                        If FileFolder.Count = 0 Then FileFolder.Add(FolderIDTextBox.Text)
                    End If
                    FileMetadata.Parents = FileFolder
                    Dim UploadStream As New FileStream(GetFile, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                    Dim FileInRAM As New MemoryTributary.MemoryTributary()
                    Dim UploadFile As FilesResource.CreateMediaUpload = Nothing
                    Dim UsingRAM As Boolean = False
                    If CopyFileToRAMBeforeUploadingToolStripMenuItem.Checked Then
                        If My.Computer.Info.AvailablePhysicalMemory > My.Computer.FileSystem.GetFileInfo(GetFile).Length Then
                            Dim megabyteMultiplication = 1024 * 1024
                            Dim readChunkSize = megabyteMultiplication
                            starttime = DateTime.Now()
                            UploadStream.Seek(0, SeekOrigin.Begin)
                            FileSizeLabel.Text = String.Format("{0:N2} MB", My.Computer.FileSystem.GetFileInfo(GetFile).Length / 1024 / 1024)
                            ProgressBar1.Maximum = CInt(My.Computer.FileSystem.GetFileInfo(GetFile).Length / 1024 / 1024)
                            Me.Update()
                            While UploadStream.Position < UploadStream.Length
                                Dim RemainingBytes = UploadStream.Length - UploadStream.Position
                                If RemainingBytes <= megabyteMultiplication Then
                                    Dim buffer(RemainingBytes) As Byte
                                    UploadStream.Read(buffer, 0, RemainingBytes)
                                    FileInRAM.Write(buffer, 0, RemainingBytes)
                                Else
                                    Dim buffer(readChunkSize) As Byte
                                    UploadStream.Read(buffer, 0, readChunkSize)
                                    FileInRAM.Write(buffer, 0, readChunkSize)
                                End If
                                UpdateBytesSent(UploadStream.Position, MsgAndDialogLang("uploadstatus_copytoram"), starttime)
                                Me.Update()
                            End While
                            UsingRAM = True
                            UploadStream.Dispose()
                            UploadStream.Close()
                            UploadFile = service.Files.Create(FileMetadata, FileInRAM, "")
                        Else
                            UploadFile = service.Files.Create(FileMetadata, UploadStream, "")
                        End If
                    Else
                        UploadFile = service.Files.Create(FileMetadata, UploadStream, "")
                    End If
                    Dim ChunkMultiplier As Integer = 4
                    If My.Computer.FileSystem.FileExists("chunkmultiplier.txt") Then
                        If String.IsNullOrEmpty(My.Computer.FileSystem.ReadAllText("chunkmultiplier.txt")) = False Then
                            ChunkMultiplier = CInt(My.Computer.FileSystem.ReadAllText("chunkmultiplier.txt"))
                            If ChunkMultiplier = 0 Then ChunkMultiplier = 4
                        Else
                            ChunkMultiplier = 4
                        End If
                    End If
                    UploadFile.ChunkSize = ResumableUpload.MinimumChunkSize * ChunkMultiplier
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
                    If UsingRAM Then
                        FileInRAM.Dispose()
                        FileInRAM.Close()
                    Else
                        UploadStream.Dispose()
                        UploadStream.Close()
                    End If
                ElseIf IO.Directory.Exists(GetFile) Then
                    Dim FolderMetadata As New Data.File With {
                        .Name = My.Computer.FileSystem.GetName(GetFile)
                    }
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
                        If ParentFolder.Count = 0 Then ParentFolder.Add(FolderIDTextBox.Text)
                    Else
                        ParentFolder.Add(FolderIDTextBox.Text)
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
                UploadsListBox.Items.RemoveAt(0)
                FolderToUploadFileListBox.Items.RemoveAt(0)
                My.Settings.UploadQueue.RemoveAt(0)
                My.Settings.UploadQueueFolders.RemoveAt(0)
                My.Settings.Save()
                ResumeFromError = False
                If UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Checked Then RefreshFileList(CurrentFolder)
                UpdateQuota()
            End If
        End While
        If RadioButton1.Checked = True Then MsgBox(MsgAndDialogLang("upload_finish"))
        FolderCreated = False
        My.Settings.FolderCreated = False
        DirectoryListID.Clear()
        DirectoryList.Clear()
        My.Settings.UploadQueue.Clear()
        My.Settings.UploadQueueFolders.Clear()
        My.Settings.FoldersCreated.Clear()
        My.Settings.FoldersCreatedID.Clear()
        My.Settings.Save()
        UploadButton.Enabled = True
    End Sub
    Private ErrorMessage As String = ""
    Private UploadCancellationToken As System.Threading.CancellationToken
    Private Sub Upload_ProgressChanged(uploadStatusInfo As IUploadProgress)
        Select Case uploadStatusInfo.Status
            Case UploadStatus.Completed
                UploadFailed = False
                ResumeFromError = False
                UpdateBytesSent(My.Computer.FileSystem.GetFileInfo(GetFile).Length, MsgAndDialogLang("uploadstatus_complete"), starttime)
            Case UploadStatus.Starting
                UpdateBytesSent(0, MsgAndDialogLang("uploadstatus_starting"), starttime)
            Case UploadStatus.Uploading
                UploadFailed = False
                UpdateBytesSent(uploadStatusInfo.BytesSent, MsgAndDialogLang("uploadstatus_uploading"), starttime)
            Case UploadStatus.Failed
                UploadFailed = True
                UpdateBytesSent(uploadStatusInfo.BytesSent, MsgAndDialogLang("uploadstatus_retry"), starttime)
                ResumeFromError = True
                Thread.Sleep(1000)
        End Select
    End Sub
    Private Sub Upload_ResponseReceived(file As Data.File)
        UpdateBytesSent(My.Computer.FileSystem.GetFileInfo(GetFile).Length, MsgAndDialogLang("uploadstatus_complete"), starttime)
    End Sub
    Private Sub Upload_UploadSessionData(ByVal uploadSessionData As IUploadSessionData)
        ' Save UploadUri.AbsoluteUri and FullPath Filename values for use if program faults and we want to restart the program
        My.Settings.ResumeUri = uploadSessionData.UploadUri.AbsoluteUri
        My.Settings.ResumeFilename = GetFile
        ' Saved to a user.config file within a subdirectory of C:\Users\<yourusername>\AppData\Local
        My.Settings.Save()
    End Sub
    Private Delegate Sub GetSessionRestartUriInvoker(Ask As Boolean)
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
                If MsgBox(String.Format(MsgAndDialogLang("resume_upload_question"), vbNewLine, GetFile), MsgBoxStyle.Question Or MsgBoxStyle.YesNo, MsgAndDialogLang("resume_upload")) = MsgBoxResult.Yes Then
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

    Private Delegate Sub UpdateBytesSentInvoker(BytesSent As Long, StatusText As String, startTime As DateTime)
    Private Sub UpdateBytesSent(BytesSent As Long, StatusText As String, startTime As DateTime)
        If StatusLabel.InvokeRequired Then
            StatusLabel.Invoke(New UpdateBytesSentInvoker(AddressOf UpdateBytesSent), BytesSent, StatusText, startTime)
        Else
            StatusLabel.Text = StatusText
        End If
        If ProcessedFileSizeLabel.InvokeRequired Then
            ProcessedFileSizeLabel.Invoke(New UpdateBytesSentInvoker(AddressOf UpdateBytesSent), BytesSent, StatusText, startTime)
        Else
            ProcessedFileSizeLabel.Text = String.Format("{0:N2} MB", BytesSent / 1024 / 1024)
        End If
        If BytesSent > 0 Then
            If ProgressBar1.InvokeRequired Then
                ProgressBar1.Invoke(New UpdateBytesSentInvoker(AddressOf UpdateBytesSent), BytesSent, StatusText, startTime)
            Else
                If ProgressBar1.Maximum >= CInt(BytesSent / 1024 / 1024) Then
                    ProgressBar1.Value = CInt(BytesSent / 1024 / 1024)
                    PercentLabel.Text = String.Format("{0:N2}%", ((ProgressBar1.Value / ProgressBar1.Maximum) * 100))
                    timespent = DateTime.Now - startTime
                    Dim timeFormatted As TimeSpan = Nothing
                    If timespent.TotalSeconds > 0 And ProgressBar1.Value > 0 Then
                        timeFormatted = TimeSpan.FromSeconds(CInt((timespent.TotalSeconds / ProgressBar1.Value * (ProgressBar1.Maximum - ProgressBar1.Value))))
                    Else
                        timeFormatted = TimeSpan.FromSeconds(0)
                    End If
                    If TimeRemainingLabel.InvokeRequired Then
                        TimeRemainingLabel.Invoke(New UpdateBytesSentInvoker(AddressOf UpdateBytesSent), BytesSent, StatusText, startTime)
                    Else
                        TimeRemainingLabel.Text = String.Format("{0}:{1:mm}:{1:ss}", CInt(Math.Truncate(timeFormatted.TotalHours)), timeFormatted)
                    End If
                End If
            End If
        End If
    End Sub
    Private Shared MaxFileSize As Double
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        BrowseToDownloadFile()
    End Sub

    Private Sub UpdateQuota()
        Dim UserDataRequest As New AboutResource.GetRequest(service) With {.Fields = "user,storageQuota"}
        Dim about As Data.About = UserDataRequest.Execute
        LoggedUsername.Text = about.User.DisplayName
        UsedSpace.Text = String.Format("{0:N2} MB", about.StorageQuota.Usage / 1024 / 1024)
        If about.StorageQuota.Limit IsNot Nothing Then
            TotalSpace.Text = String.Format("{0:N2} MB", about.StorageQuota.Limit / 1024 / 1024)
            FreeSpace.Text = String.Format("{0:N2} MB", (about.StorageQuota.Limit - about.StorageQuota.Usage) / 1024 / 1024)
        Else
            TotalSpace.Text = "Unlimited"
            FreeSpace.Text = "Unlimited"
        End If
    End Sub
    Private Async Sub BrowseToDownloadFile()
        FileIdsListBox.SelectedIndex = FilesListBox.SelectedIndex
        FileSizeListBox.SelectedIndex = FilesListBox.SelectedIndex
        FileModifiedTimeListBox.SelectedIndex = FilesListBox.SelectedIndex
        SaveFileDialog1.Title = MsgAndDialogLang("location_browse")
        SaveFileDialog1.FileName = FilesListBox.SelectedItem.ToString
        Dim SFDResult As DialogResult = SaveFileDialog1.ShowDialog()
        If SFDResult = DialogResult.OK Then
            Await DownloadFile(SaveFileDialog1.FileName, FileIdsListBox.SelectedItem.ToString, FileSizeListBox.SelectedItem.ToString, FileModifiedTimeListBox.SelectedItem.ToString)
        End If
    End Sub
    Private Async Function DownloadFile(Location As String, FileName As String, FileSize As String, ModifiedTime As Date) As Task
        starttime = DateTime.Now
        FileSizeLabel.Text = String.Format("{0:N2} MB", Convert.ToDouble(FileSize) / 1024 / 1024)
        ProgressBar1.Maximum = CInt(Convert.ToDouble(FileSize) / 1024 / 1024)
        MaxFileSize = Convert.ToDouble(FileSize)
        Dim FileToSave As FileStream = New FileStream(Location, FileMode.Create, FileAccess.Write)
        Dim DownloadRequest As FilesResource.GetRequest = service.Files.Get(FileName)
        AddHandler DownloadRequest.MediaDownloader.ProgressChanged, New Action(Of IDownloadProgress)(AddressOf Download_ProgressChanged)
        Await DownloadRequest.DownloadAsync(FileToSave)
        FileToSave.Close()
        IO.File.SetLastWriteTime(Location, ModifiedTime)
    End Function
    Private Sub Download_ProgressChanged(progress As IDownloadProgress)
        Select Case progress.Status
            Case DownloadStatus.Completed
                UpdateBytesSent(CInt(MaxFileSize), MsgAndDialogLang("uploadstatus_complete"), starttime)
            Case DownloadStatus.Downloading
                UpdateBytesSent(progress.BytesDownloaded, MsgAndDialogLang("uploadstatus_downloading"), starttime)
            Case DownloadStatus.Failed
                UpdateBytesSent(progress.BytesDownloaded, MsgAndDialogLang("uploadstatus_failed"), starttime)
        End Select
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If viewing_trash = False Then RefreshFileList(CurrentFolder) Else RefreshFileList("trash")
    End Sub
    Private Delegate Sub RefreshFileListInvoker(FolderID As String)
    Private Sub RefreshFileList(FolderID As String)
        If FilesListBox.InvokeRequired Then FilesListBox.Invoke(New RefreshFileListInvoker(AddressOf RefreshFileList), FolderID)
        If FolderListBox.InvokeRequired Then FolderListBox.Invoke(New RefreshFileListInvoker(AddressOf RefreshFileList), FolderID)
        Dim listRequestQString As String = Nothing
        Dim listRequestQFolderString As String = Nothing
        If FolderID = "trash" Then
            CurrentFolderLabel.Text = "Trash"
            listRequestQString = "mimeType!='application/vnd.google-apps.folder' and trashed = true"
            listRequestQFolderString = "mimeType='application/vnd.google-apps.folder' and trashed = true"
        Else
            listRequestQString = "mimeType!='application/vnd.google-apps.folder' and '" & FolderID & "' in parents and trashed = false"
            listRequestQFolderString = "mimeType='application/vnd.google-apps.folder' and '" & FolderID & "' in parents and trashed = false"
        End If
        FilesListBox.Items.Clear()
        FileIdsListBox.Items.Clear()
        FileSizeListBox.Items.Clear()
        FileModifiedTimeListBox.Items.Clear()
        FileCreatedTimeListBox.Items.Clear()
        FileMD5ListBox.Items.Clear()
        FileMIMEListBox.Items.Clear()
        Dim PageToken1 As String = String.Empty
        Dim OrderBy As String = My.Settings.SortBy
        If My.Settings.OrderDesc Then OrderBy = OrderBy + " desc,name"
        Do
            Dim listRequest1 As FilesResource.ListRequest = service.Files.List()
            listRequest1.Fields = "nextPageToken, files(id, name, size, createdTime, modifiedTime, md5Checksum, mimeType)"
            listRequest1.Q = listRequestQString
            listRequest1.OrderBy = OrderBy
            listRequest1.PageToken = PageToken1
            Try
                Dim files = listRequest1.Execute()
                If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
                    For Each file In files.Files
                        FilesListBox.Items.Add(file.Name)
                        FileIdsListBox.Items.Add(file.Id)
                        If file.Size IsNot Nothing Then FileSizeListBox.Items.Add(file.Size) Else FileSizeListBox.Items.Add("0")
                        FileModifiedTimeListBox.Items.Add(file.ModifiedTime)
                        FileCreatedTimeListBox.Items.Add(file.CreatedTime)
                        If file.Md5Checksum IsNot Nothing Then FileMD5ListBox.Items.Add(file.Md5Checksum) Else FileMD5ListBox.Items.Add("")
                        FileMIMEListBox.Items.Add(file.MimeType)
                    Next
                End If
                PageToken1 = files.NextPageToken
            Catch ex As Exception
            End Try
        Loop While PageToken1 = String.Empty = False
        FolderListBox.Items.Clear()
        FolderIdsListBox.Items.Clear()
        Dim PageToken2 As String = String.Empty
        Do
            Dim listRequest As FilesResource.ListRequest = service.Files.List()
            listRequest.Q = listRequestQFolderString
            listRequest.Fields = "nextPageToken, files(id, name)"
            listRequest.OrderBy = OrderBy
            listRequest.PageToken = PageToken2
            Try
                Dim files = listRequest.Execute()
                If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
                    For Each file In files.Files
                        FolderListBox.Items.Add(file.Name)
                        FolderIdsListBox.Items.Add(file.Id)
                    Next
                End If
                PageToken2 = files.NextPageToken
            Catch ex As Exception
            End Try
        Loop While PageToken2 = String.Empty = False
        If CurrentFolder = "root" Or CurrentFolder = "trash" Then
            Button10.Enabled = False
        Else
            Button10.Enabled = True
        End If
    End Sub
    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim filepath() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())
        For Each path In filepath
            If System.IO.Directory.Exists(path) Then
                UploadsListBox.Items.Add(path)
                FolderToUploadFileListBox.Items.Add(CurrentFolder)
                My.Settings.UploadQueue.Add(path)
                My.Settings.UploadQueueFolders.Add(CurrentFolder)
                GetDirectoriesAndFiles(New IO.DirectoryInfo(path))
            Else
                UploadsListBox.Items.Add(path)
                FolderToUploadFileListBox.Items.Add(CurrentFolder)
                My.Settings.UploadQueue.Add(path)
                My.Settings.UploadQueueFolders.Add(CurrentFolder)
            End If
        Next
        My.Settings.Save()
        If My.Settings.AutomaticUploads And UploadButton.Enabled = True Then
            CheckBeforeStartingUpload()
        End If
    End Sub
    Private Sub GetDirectoriesAndFiles(ByVal BaseFolder As IO.DirectoryInfo)
        UploadsListBox.Items.AddRange((From FI As IO.FileInfo In BaseFolder.GetFiles Select FI.FullName).ToArray)
        My.Settings.UploadQueue.AddRange((From FI As IO.FileInfo In BaseFolder.GetFiles Select FI.FullName).ToArray)
        For Each FI As IO.FileInfo In BaseFolder.GetFiles()
            FolderToUploadFileListBox.Items.Add(CurrentFolder)
            My.Settings.UploadQueueFolders.Add(CurrentFolder)
        Next
        For Each subF As IO.DirectoryInfo In BaseFolder.GetDirectories()
            Application.DoEvents()
            UploadsListBox.Items.Add(subF.FullName)
            FolderToUploadFileListBox.Items.Add(CurrentFolder)
            My.Settings.UploadQueue.Add(subF.FullName)
            My.Settings.UploadQueueFolders.Add(CurrentFolder)
            GetDirectoriesAndFiles(subF)
        Next
    End Sub
    Private Sub Form1_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragEnter
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

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Do While (UploadsListBox.SelectedItems.Count > 0)
            Dim CurrentIndex = UploadsListBox.SelectedIndex
            UploadsListBox.Items.RemoveAt(CurrentIndex)
            My.Settings.UploadQueue.RemoveAt(CurrentIndex)
            FolderToUploadFileListBox.Items.RemoveAt(CurrentIndex)
            My.Settings.UploadQueueFolders.RemoveAt(CurrentIndex)
            My.Settings.Save()
        Loop
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        UploadsListBox.Items.Clear()
        My.Settings.UploadQueue.Clear()
        FolderToUploadFileListBox.Items.Clear()
        My.Settings.UploadQueueFolders.Clear()
        My.Settings.Save()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        GetFolderIDName(True)
    End Sub

    Private Function GetFolderIDName(ShowMessage As Boolean) As Boolean
        If String.IsNullOrEmpty(FolderIDTextBox.Text) = False Then
            Try
                Dim GetFolderName As FilesResource.GetRequest = service.Files.Get(FolderIDTextBox.Text.ToString)
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
    Private Function GetIDName(ID As String) As String
        Try
            Dim GetName As FilesResource.GetRequest = service.Files.Get(ID)
            Dim Meta As Data.File = GetName.Execute
            Return Meta.Name
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
    Private Function GetCurrentFolderIDName() As String
        Try
            Dim GetFolderName As FilesResource.GetRequest = service.Files.Get(CurrentFolder)
            Dim FolderNameMetadata As Data.File = GetFolderName.Execute
            Return FolderNameMetadata.Name
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Sub ListBox3_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles FolderListBox.MouseDoubleClick
        If My.Computer.Keyboard.ShiftKeyDown Then
            Process.Start("https://drive.google.com/drive/folders/" + FolderIdsListBox.Items.Item(FolderListBox.SelectedIndex))
        Else
            If viewing_trash = False Then
                EnterFolder()
            End If
        End If
    End Sub
    Private Sub GoBack()
        If viewing_trash = False Then
            If CurrentFolder = "root" = False Then
                Dim PreviousFolderIdBeforeRemoving = PreviousFolderId.Items.Item(PreviousFolderId.Items.Count - 1)
                PreviousFolderId.Items.RemoveAt(PreviousFolderId.Items.Count - 1)
                My.Settings.PreviousFolderIDs.RemoveAt(My.Settings.PreviousFolderIDs.Count - 1)
                CurrentFolder = PreviousFolderIdBeforeRemoving.ToString
                My.Settings.LastFolder = CurrentFolder
                My.Settings.Save()
                CurrentFolderLabel.Text = GetCurrentFolderIDName()
                RefreshFileList(PreviousFolderIdBeforeRemoving.ToString)
                If CurrentFolder = "root" Then
                    Button10.Enabled = False
                Else
                    Button10.Enabled = True
                End If
            End If
        Else
            RefreshFileList("trash")
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        GoBack()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FilesListBox.SelectedIndexChanged
        If FilesListBox.SelectedItem IsNot Nothing Then
            TextBox3.Text = FilesListBox.SelectedItem.ToString()
            TextBox4.Text = FileIdsListBox.Items.Item(FilesListBox.SelectedIndex).ToString
            TextBox5.Text = FileCreatedTimeListBox.Items.Item(FilesListBox.SelectedIndex).ToString
            TextBox6.Text = FileModifiedTimeListBox.Items.Item(FilesListBox.SelectedIndex).ToString
            TextBox7.Text = FileMD5ListBox.Items.Item(FilesListBox.SelectedIndex).ToString
            TextBox8.Text = FileMIMEListBox.Items.Item(FilesListBox.SelectedIndex).ToString
            TextBox9.Text = String.Format("{0:N2} MB", Convert.ToDouble(FileSizeListBox.Items.Item(FilesListBox.SelectedIndex)) / 1024 / 1024)
        Else
            TextBox3.Text = ""
            TextBox4.Text = ""
            TextBox8.Text = ""
            TextBox5.Text = ""
            TextBox6.Text = ""
            TextBox7.Text = ""
            TextBox9.Text = ""
        End If
        If FilesListBox.SelectedItems.Count > 1 Then
            Button7.Visible = True
        Else
            Button7.Visible = False
        End If
    End Sub


    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        CreateFolder()
    End Sub
    Private Sub CreateFolder()
        Dim FolderNameToCreate As String
        Dim Message, Title As String
        Message = MsgAndDialogLang("enter_name_for_folder")
        Title = MsgAndDialogLang("create_new_folder")
        FolderNameToCreate = InputBox(Message, Title)
        If String.IsNullOrEmpty(FolderNameToCreate) = False Then
            Dim FolderMetadata As New Data.File With {
                .Name = FolderNameToCreate
            }
            Dim ParentFolder As New List(Of String) From {
                CurrentFolder
            }
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
            FolderIDTextBox.Text = FolderID.Id
            CurrentFolder = FolderID.Id
            CurrentFolderLabel.Text = GetCurrentFolderIDName()
            RefreshFileList(FolderID.Id)
        End If
    End Sub
    Private Sub RenameFileOrFolder(FileOrFolderToRename As String)
        Dim NewName As String
        Dim Message, Title As String
        Message = MsgAndDialogLang("enter_new_name")
        Title = MsgAndDialogLang("rename_dialog")
        NewName = InputBox(Message, Title, GetIDName(FileOrFolderToRename))
        If String.IsNullOrEmpty(NewName) = False Then
            Dim FileMetadata As New Data.File With {.Name = NewName}
            service.Files.Update(FileMetadata, FileOrFolderToRename).ExecuteAsync()
            Thread.Sleep(500)
            RefreshFileList(CurrentFolder)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksum.md5") Else SaveChecksumsFile(FilesListBox.SelectedItem.ToString & ".md5")
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksums.md5") Else SaveChecksumsFile(GetCurrentFolderIDName() & ".md5")
    End Sub
    Private Function SaveChecksumFileDialog(FileOrFolderName As String) As String
        SaveFileDialog2.Title = MsgAndDialogLang("checksum_location")
        SaveFileDialog2.FileName = FileOrFolderName
        SaveFileDialog2.Filter = "MD5 Checksum|*.md5"
        Dim SFDResult As DialogResult = SaveFileDialog2.ShowDialog()
        Dim ReturnPath As String = String.Empty
        If SFDResult = DialogResult.OK Then
            ReturnPath = SaveFileDialog2.FileName
        Else
            ReturnPath = Nothing
        End If
        Return ReturnPath
    End Function

    Private Sub SaveFileChecksums(ChecksumFile As StreamWriter)
        For Each item In FilesListBox.SelectedItems
            ChecksumFile.WriteLine(FileMD5ListBox.Items.Item(FilesListBox.Items.IndexOf(item)).ToString & " *" & item.ToString)
        Next
    End Sub
    Private Sub WorkWithTrash(Items As ListBox.SelectedObjectCollection, Optional IsFile As Boolean = False, Optional TrashItem As Boolean = False)
        Dim ConfirmMessage As String = String.Empty
        Dim SuccessMessage As String = String.Empty
        If TrashItem Then
            If IsFile Then
                If Items.Count > 1 Then
                    ConfirmMessage = MsgAndDialogLang("move_selected_file2trash")
                    SuccessMessage = MsgAndDialogLang("files_moved2trash")
                Else
                    ConfirmMessage = MsgAndDialogLang("move_file2trash_part1") & FilesListBox.SelectedItem.ToString & MsgAndDialogLang("move_file2trash_part2")
                    SuccessMessage = MsgAndDialogLang("file_moved2trash")
                End If
            Else
                If Items.Count > 1 Then
                    ConfirmMessage = MsgAndDialogLang("confirm_move_selected_folder2trash")
                    SuccessMessage = MsgAndDialogLang("folders_moved2trash")
                Else
                    ConfirmMessage = MsgAndDialogLang("confirm_move_folder2trash_part1") & FolderListBox.SelectedItem.ToString & MsgAndDialogLang("confirm_move_folder2trash_part2")
                    SuccessMessage = MsgAndDialogLang("folder_moved2trash")
                End If
            End If
        Else
            If IsFile Then
                If Items.Count > 1 Then
                    ConfirmMessage = MsgAndDialogLang("confirm_restore_selected_files")
                    SuccessMessage = MsgAndDialogLang("files_restored")
                Else
                    ConfirmMessage = MsgAndDialogLang("restore_file_part1") & FilesListBox.SelectedItem.ToString & MsgAndDialogLang("restore_file_part2")
                    SuccessMessage = MsgAndDialogLang("file_restored")
                End If
            Else
                If Items.Count > 1 Then
                    ConfirmMessage = MsgAndDialogLang("confirm_restore_selected_folders")
                    SuccessMessage = MsgAndDialogLang("folders_restored")
                Else
                    ConfirmMessage = MsgAndDialogLang("restore_folder_part1") & FolderListBox.SelectedItem.ToString & MsgAndDialogLang("restore_folder_part2")
                    SuccessMessage = MsgAndDialogLang("folder_restored")
                End If
            End If
        End If

        If MsgBox(ConfirmMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Dim FileMetadata As New Data.File With {.Trashed = TrashItem}
            For Each item In Items
                If IsFile Then
                    service.Files.Update(FileMetadata, FileIdsListBox.Items.Item(FilesListBox.Items.IndexOf(item)).ToString).ExecuteAsync()
                Else
                    service.Files.Update(FileMetadata, FolderIdsListBox.Items.Item(FolderListBox.Items.IndexOf(item)).ToString).ExecuteAsync()
                End If
            Next
            Thread.Sleep(1000)
            RefreshFileList(CurrentFolder)
            MsgBox(SuccessMessage)
        End If
    End Sub
    Private KeyPressed As Boolean = False
    Private Sub FolderListBox_KeyDown(sender As Object, e As KeyEventArgs) Handles FolderListBox.KeyDown
        KeyPressed = True
        If e.KeyCode = Keys.Delete Then
            If viewing_trash = False Then
                If FolderListBox.SelectedItem IsNot Nothing Then
                    WorkWithTrash(FolderListBox.SelectedItems, False, True)
                End If
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            EnterFolder()
        ElseIf e.KeyCode = Keys.F5 Then
            If viewing_trash = False Then RefreshFileList(CurrentFolder) Else RefreshFileList("trash")
            e.Handled = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.R Then
            If FolderListBox.SelectedItem IsNot Nothing Then
                If viewing_trash Then
                    WorkWithTrash(FolderListBox.SelectedItems, False, False)
                Else
                    RenameFileOrFolder(FolderIdsListBox.Items.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)).ToString)
                End If
            End If
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.A Then
            For i = 0 To FolderListBox.Items.Count - 1
                FolderListBox.SetSelected(i, True)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            EnterFolder()
            If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksums.md5", True) Else SaveChecksumsFile(GetCurrentFolderIDName() & ".md5", True)
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.D Then
            CheckForFolderDownload()
        End If
        KeyPressed = False
    End Sub

    Private Sub FolderListBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles FolderListBox.KeyPress
        If KeyPressed = True Then
            e.Handled = True
        End If
    End Sub
    Private Function GetFolderPath() As String
        FolderBrowserDialog1.ShowNewFolderButton = True
        Dim FolderBrowserDialogResponse As DialogResult = FolderBrowserDialog1.ShowDialog
        If FolderBrowserDialogResponse = DialogResult.OK Then
            Return FolderBrowserDialog1.SelectedPath
        Else
            Return Nothing
        End If
    End Function
    Private Async Sub DownloadFilesAndFolders(Optional IsFolder As Boolean = False)
        Dim FolderList As New List(Of String)
        If IsFolder Then
            FolderList.Add(CurrentFolder)
        End If
        Dim FolderPath As String = GetFolderPath()
        If FolderPath IsNot Nothing Then
            If IsFolder Then
                Await DownloadFolder(FolderList, FolderPath)
            Else
                Await DownloadFiles(FolderPath)
            End If
            MsgBox(MsgAndDialogLang("downloads_finished"))
        End If
    End Sub
    Private Async Function DownloadFiles(Folder As String) As Task
        Dim FileList As New List(Of String)
        For Each item In FilesListBox.SelectedItems
            FileList.Add(item.ToString())
        Next
        For Each item In FileList
            FilesListBox.ClearSelected()
            FilesListBox.SelectedItem = item
            Await DownloadFile(Folder & "\" & item, FileIdsListBox.Items.Item(FilesListBox.Items.IndexOf(item)).ToString, FileSizeListBox.Items.Item(FilesListBox.Items.IndexOf(item)).ToString, FileModifiedTimeListBox.Items.Item(FilesListBox.Items.IndexOf(item)).ToString)
        Next
    End Function
    Private Sub SaveChecksumsFile(Filename As String, Optional IsFolder As Boolean = False)
        Dim FolderList As New List(Of String)
        If IsFolder Then
            FolderList.Add(CurrentFolder)
        End If
        Filename = SaveChecksumFileDialog(Filename)
        If Filename IsNot Nothing Then
            Dim Checksumfile As StreamWriter = New StreamWriter(Filename, False, System.Text.Encoding.UTF8)
            If IsFolder Then
                GetFileFolderChecksum(FolderList, Checksumfile)
                GoBack()
            Else
                SaveFileChecksums(Checksumfile)
            End If
            Checksumfile.Close()
            MsgBox(MsgAndDialogLang("checksums_saved"))
        End If
    End Sub
    Private Sub GetFileFolderChecksum(Path As List(Of String), Stream As StreamWriter)
        'This creates the full path of the file by getting the ID Name.
        Dim FullPath As String = ""
        If Path.Count > 0 Then
            For Each item In Path
                Try
                    Dim GetFolderName As FilesResource.GetRequest = service.Files.Get(item)
                    Dim FolderNameMetadata As Data.File = GetFolderName.Execute
                    FullPath = FullPath + FolderNameMetadata.Name + "\"
                Catch ex As Exception

                End Try
            Next
        End If
        'Once Full Path has been created, we check for files inside the folder. If there's files, we will store their MD5 checksum.
        For Each item In FilesListBox.Items
            Stream.WriteLine(FileMD5ListBox.Items.Item(FilesListBox.Items.IndexOf(item)).ToString & " *" & FullPath & item.ToString)
        Next
        'Finally, this loop checks if there are folders inside the folder we are. We start a recursion loop by calling this same function for each folder inside the folder.
        If FolderListBox.Items.Count > 0 Then
            Dim FolderList As New List(Of String)
            For Each FolderInList In FolderListBox.Items
                FolderList.Add(FolderIdsListBox.Items.Item(FolderListBox.Items.IndexOf(FolderInList)).ToString)
            Next
            For Each Folder2 In FolderList
                Path.Add(FolderIdsListBox.Items.Item(FolderIdsListBox.Items.IndexOf(Folder2)).ToString)
                FolderListBox.SelectedItem = FolderListBox.Items.Item(FolderIdsListBox.Items.IndexOf(Folder2))
                EnterFolder()
                GetFileFolderChecksum(Path, Stream)
                GoBack()
                Path.Remove(Folder2)
            Next
        End If
    End Sub

    Private Async Function DownloadFolder(Path As List(Of String), Location As String) As Task
        'This creates the full path of the file by getting the ID Name.
        Dim FullPath As String = ""
        Dim FolderToCreatePath As String = ""
        Dim count As Integer = 0
        If Path.Count > 0 Then
            For Each item In Path
                Try
                    Dim GetFolderName As FilesResource.GetRequest = service.Files.Get(item)
                    Dim FolderNameMetadata As Data.File = GetFolderName.Execute
                    If Path.Count = count + 1 Then
                        FolderToCreatePath = FolderToCreatePath + FolderNameMetadata.Name
                        My.Computer.FileSystem.CreateDirectory(Location + "\" + FolderToCreatePath)
                    Else
                        FolderToCreatePath = FolderToCreatePath + FolderNameMetadata.Name + "\"
                    End If
                    FullPath = FullPath + FolderNameMetadata.Name + "\"
                Catch ex As Exception

                End Try
                count = count + 1
            Next
            count = 0
        End If
        'Once Full Path has been created, we check for files inside the folder. If there's files, we will download them
        Dim FolderFiles As New List(Of String)
        For Each item In FilesListBox.Items
            FolderFiles.Add(item.ToString)
        Next
        For Each item In FolderFiles
            FilesListBox.ClearSelected()
            FilesListBox.SelectedItem = item
            Await DownloadFile(Location & "\" & FullPath & item, FileIdsListBox.Items.Item(FilesListBox.Items.IndexOf(item)).ToString, FileSizeListBox.Items.Item(FilesListBox.Items.IndexOf(item)).ToString, FileModifiedTimeListBox.Items.Item(FilesListBox.Items.IndexOf(item)).ToString)
        Next
        'Finally, this loop checks if there are folders inside the folder we are. We start a recursion loop by calling this same function for each folder inside the folder.
        If FolderListBox.Items.Count > 0 Then
            Dim FolderList As New List(Of String)
            For Each FolderInList In FolderListBox.Items
                FolderList.Add(FolderIdsListBox.Items.Item(FolderListBox.Items.IndexOf(FolderInList)).ToString)
            Next
            For Each Folder2 In FolderList
                Path.Add(FolderIdsListBox.Items.Item(FolderIdsListBox.Items.IndexOf(Folder2)).ToString)
                FolderListBox.SelectedItem = FolderListBox.Items.Item(FolderIdsListBox.Items.IndexOf(Folder2))
                EnterFolder()
                Await DownloadFolder(Path, Location)
                GoBack()
                Path.Remove(Folder2)
            Next
        End If
    End Function
    Private Sub EnterFolder()
        If FolderListBox.SelectedItem IsNot Nothing Then
            Dim GoToFolderID As String = FolderIdsListBox.Items.Item(FolderListBox.SelectedIndex).ToString
            PreviousFolderId.Items.Add(CurrentFolder)
            My.Settings.PreviousFolderIDs.Add(CurrentFolder)
            CurrentFolder = FolderIdsListBox.Items.Item(FolderListBox.SelectedIndex).ToString
            My.Settings.LastFolder = CurrentFolder
            My.Settings.Save()
            Button7.Visible = False
            Button10.Enabled = True
            CurrentFolderLabel.Text = GetCurrentFolderIDName()
            RefreshFileList(GoToFolderID)
        End If
    End Sub

    Private Sub FilesListBox_KeyDown(sender As Object, e As KeyEventArgs) Handles FilesListBox.KeyDown
        KeyPressed = True
        If e.KeyCode = Keys.Delete Then
            If viewing_trash = False Then
                If FilesListBox.SelectedItem IsNot Nothing Then
                    WorkWithTrash(FilesListBox.SelectedItems, True, True)
                End If
            End If
        ElseIf e.KeyCode = Keys.F5 Then
            If viewing_trash = False Then RefreshFileList(CurrentFolder) Else RefreshFileList("trash")
            e.Handled = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.R Then
            If FilesListBox.SelectedItem IsNot Nothing Then
                If viewing_trash Then
                    WorkWithTrash(FilesListBox.SelectedItems, True, False)
                Else
                    RenameFileOrFolder(FileIdsListBox.Items.Item(FilesListBox.Items.IndexOf(FilesListBox.SelectedItem)).ToString)
                End If
            End If
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.A Then
            For i = 0 To FilesListBox.Items.Count - 1
                FilesListBox.SetSelected(i, True)
            Next
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            If FilesListBox.SelectedIndex <> -1 Then
                If FilesListBox.SelectedItems.Count > 1 Then
                    If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksums.md5") Else SaveChecksumsFile(GetCurrentFolderIDName() & ".md5")
                Else
                    If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksum.md5") Else SaveChecksumsFile(FilesListBox.SelectedItem.ToString & ".md5")
                End If
            End If
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.D Then
            CheckForFilesDownload()
        End If
        KeyPressed = False
    End Sub
    Private Sub FilesListBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles FilesListBox.KeyPress
        If KeyPressed = True Then
            e.Handled = True
        End If
    End Sub

    Private Sub BtnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim credPath As String = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
        credPath = Path.Combine(credPath, ".credentials\GoogleDriveUploaderTool.json")
        Dim credfiles As String() = Directory.GetFiles(credPath, "*.TokenResponse-user")
        For Each credfile In credfiles
            Debug.WriteLine(credfile)
            File.Delete(credfile)
        Next
        MsgBox(MsgAndDialogLang("logged_out"))
        Application.Exit()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If viewing_trash = False Then
            viewing_trash = True
            GoToRootLink.Visible = False
            Button12.Enabled = False
            RestoreToolStripMenuItem.Enabled = True
            MoveToTrashToolStripMenuItem.Enabled = False
            'PreviousFolderId.Items.Clear()
            'FolderIdsListBox.Items.Clear()
            Lang_Select()
            RefreshFileList("trash")
        Else
            viewing_trash = False
            GoToRootLink.Visible = True
            Button12.Enabled = True
            RestoreToolStripMenuItem.Enabled = False
            MoveToTrashToolStripMenuItem.Enabled = True
            Lang_Select()
            CurrentFolderLabel.Text = GetCurrentFolderIDName()
            CurrentFolderLabel.Visible = True
            RefreshFileList(CurrentFolder)
        End If
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles UploadsListBox.SelectedIndexChanged
        If UploadsListBox.SelectedIndex <> -1 Then
            FolderIDTextBox.Text = FolderToUploadFileListBox.Items.Item(UploadsListBox.SelectedIndex).ToString
            GetFolderIDName(False)
            Button13.Visible = True
            Button14.Enabled = True
        Else
            Button13.Visible = False
            Button14.Enabled = False
        End If

    End Sub

    Private Sub EnglishLanguage()
        Label1.Text = "File Size:"
        Label2.Text = "Processed:"
        Label5.Text = "Drag and Drop Files to add them to the list"
        Label6.Text = "By Moisés Cardona" & vbNewLine & "v1.8.3"
        Label7.Text = "Status:"
        Label9.Text = "Percent: "
        Label11.Text = "Files:"
        Label12.Text = "Upload to this folder ID (""root"" to upload to root folder):"
        Label13.Text = "Time Left: "
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
        UploadButton.Text = "Upload"
        Button3.Text = "Clear List"
        Button4.Text = "Refresh List"
        Button5.Text = "Download File"
        Button6.Text = "Remove selected file(s) from list"
        Button7.Text = "Save Checksums for Selected Files"
        Button9.Text = "Get Folder Name"
        Button10.Text = "Back"
        GroupBox2.Text = "File Information:"
        If viewing_trash = False Then
            Button11.Text = "View Trash"
        ElseIf viewing_trash = True Then
            Button11.Text = "View Drive"
        End If
        Button12.Text = "Create New Folder"
        Button13.Text = "Upload selected file(s) to current folder"
        Button14.Text = "Deselect"
        btnLogout.Text = "Logout"
        ActionsToolStripMenuItem.Text = "Actions"
        CreateNewFolderToolStripMenuItem.Text = "Create New Folder"
        RenameToolStripMenuItem.Text = "Rename"
        SelectedFileToolStripMenuItem1.Text = "Selected File"
        SelectedFolderToolStripMenuItem1.Text = "Selected Folder"
        RefreshListToolStripMenuItem.Text = "Refresh List"
        MoveToTrashToolStripMenuItem.Text = "Move to Trash"
        SelectedFilesToolStripMenuItem.Text = "Selected file(s)"
        SelectedFoldersToolStripMenuItem.Text = "Selected folder(s)"
        RestoreToolStripMenuItem.Text = "Restore"
        SelectedFilesToolStripMenuItem1.Text = "Selected file(s)"
        SelectedFoldersToolStripMenuItem1.Text = "Selected folder(s)"
        SaveChecksumsToolStripMenuItem.Text = "Save Checksums"
        SelectedFilesToolStripMenuItem2.Text = "Selected file(s)"
        SelectedFolderToolStripMenuItem2.Text = "Selected folder"
        CopyFileToRAMBeforeUploadingToolStripMenuItem.Text = "Copy File to RAM before uploading if there's enough Free Memory available"
        DonationsToolStripMenuItem.Text = "Donations"
        FileToolStripMenuItem.Text = "File"
        UploadToolStripMenuItem.Text = "Upload"
        FileToolStripMenuItem1.Text = "File(s)"
        FolderToolStripMenuItem.Text = "Folder"
        DownloadToolStripMenuItem.Text = "Download"
        SelectedFileToolStripMenuItem.Text = "Selected File(s)"
        SelectedFolderToolStripMenuItem.Text = "Selected Folder"
        HelpToolStripMenuItem.Text = "Help"
        OptionsToolStripMenuItem.Text = "Options"
        OrderByToolStripMenuItem.Text = "Order By"
        OrderByComboBox.Items.Clear()
        OrderByComboBox.Items.AddRange({"Created Time", "Folder", "Modified By Me Time", "Modified Time", "Name", "Natural Name", "Quota Bytes Used", "Recency", "Shared With Me Time", "Starred", "Viewed By Me Time"})
        OrderByComboBox.SelectedIndex = My.Settings.SortByIndex
        DescendingOrderToolStripMenuItem.Text = "Descending Order"
        PreserveFileModifiedDateToolStripMenuItem.Text = "Preserve File Modification Date"
        SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Text = "Save checksums as checksums.md5"
        StartUploadsAutomaticallyToolStripMenuItem.Text = "Start Uploads Automatically"
        SpecifyChunkSizeToolStripMenuItem.Text = "Specify Chunk Size"
        UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Text = "Update File and Folder views after an upload finishes"
        ReadmeToolStripMenuItem.Text = "Readme / Help"
        LoggedInAs.Text = "Logged In As:"
        UsedSpaceText.Text = "Used Space:"
        TotalSpaceText.Text = "Total Space:"
        FreeSpaceText.Text = "Free Space:"
    End Sub

    Private Sub TChineseLanguage()
        Label1.Text = "文件大小:"
        Label2.Text = "Processed:"
        Label5.Text = "請將文件拖到下方"
        Label6.Text = "By Moisés Cardona" & vbNewLine & "v1.8.3" & vbNewLine & "Translated by mic4126"
        Label7.Text = "狀態:"
        Label9.Text = "百份比: "
        Label11.Text = "文件:"
        Label12.Text = "上傳到此文件夾ID (""root"" 指上傳到根目錄):"
        Label13.Text = "餘下時間: "
        Label16.Text = "文件夾名稱:"
        Label18.Text = "文件名稱:"
        Label19.Text = "文件ID:"
        Label20.Text = "新建日期:"
        Label21.Text = "修改日期:"
        Label22.Text = "MD5 校驗碼:"
        Label23.Text = "MIME Type:"
        Label24.Text = "文件大小:"
        Button1.Text = "儲存校驗碼"
        UploadButton.Text = "上傳"
        Button3.Text = "清除列表"
        Button4.Text = "更新列表"
        Button5.Text = "下載文件"
        Button6.Text = "由列表中移除已選文件"
        Button7.Text = "儲存已選文件校驗碼"
        Button9.Text = "獲取文件夾名稱"
        Button10.Text = "返回"
        GroupBox2.Text = "File Information:"
        If viewing_trash = False Then
            Button11.Text = "查看垃圾桶"
        ElseIf viewing_trash = True Then
            Button11.Text = "回到Google Drive"
        End If
        Button12.Text = "新增文件夾"
        Button13.Text = "Upload selected file(s) to current folder"
        Button14.Text = "Deselect"
        btnLogout.Text = "登岀"
        ActionsToolStripMenuItem.Text = "Actions"
        CreateNewFolderToolStripMenuItem.Text = "Create New Folder"
        RenameToolStripMenuItem.Text = "Rename"
        SelectedFileToolStripMenuItem1.Text = "Selected File"
        SelectedFolderToolStripMenuItem1.Text = "Selected Folder"
        RefreshListToolStripMenuItem.Text = "Refresh List"
        MoveToTrashToolStripMenuItem.Text = "Move to Trash"
        SelectedFilesToolStripMenuItem.Text = "Selected file(s)"
        SelectedFoldersToolStripMenuItem.Text = "Selected folder(s)"
        RestoreToolStripMenuItem.Text = "Restore"
        SelectedFilesToolStripMenuItem1.Text = "Selected file(s)"
        SelectedFoldersToolStripMenuItem1.Text = "Selected folder(s)"
        SaveChecksumsToolStripMenuItem.Text = "Save Checksums"
        SelectedFilesToolStripMenuItem2.Text = "Selected file(s)"
        SelectedFolderToolStripMenuItem2.Text = "Selected folder"
        CopyFileToRAMBeforeUploadingToolStripMenuItem.Text = "Copy File to RAM before uploading if there's enough Free Memory available"
        DonationsToolStripMenuItem.Text = "捐款"
        FileToolStripMenuItem.Text = "File"
        UploadToolStripMenuItem.Text = "Upload"
        FileToolStripMenuItem1.Text = "File(s)"
        FolderToolStripMenuItem.Text = "Folder"
        DownloadToolStripMenuItem.Text = "Download"
        SelectedFileToolStripMenuItem.Text = "Selected File(s)"
        SelectedFolderToolStripMenuItem.Text = "Selected Folder"
        HelpToolStripMenuItem.Text = "Help"
        OptionsToolStripMenuItem.Text = "Options"
        OrderByToolStripMenuItem.Text = "Order By"
        OrderByComboBox.Items.Clear()
        OrderByComboBox.Items.AddRange({"Created Time", "Folder", "Modified By Me Time", "Modified Time", "Name", "Natural Name", "Quota Bytes Used", "Recency", "Shared With Me Time", "Starred", "Viewed By Me Time"})
        OrderByComboBox.SelectedIndex = My.Settings.SortByIndex
        DescendingOrderToolStripMenuItem.Text = "Descending Order"
        PreserveFileModifiedDateToolStripMenuItem.Text = "Preserve File Modification Date"
        SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Text = "Save checksums as checksums.md5"
        StartUploadsAutomaticallyToolStripMenuItem.Text = "Start Uploads Automatically"
        SpecifyChunkSizeToolStripMenuItem.Text = "Specify Chunk Size"
        UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Text = "Update File and Folder views after an upload finishes"
        ReadmeToolStripMenuItem.Text = "Readme / Help"
        LoggedInAs.Text = "Logged In As:"
        UsedSpaceText.Text = "Used Space:"
        TotalSpaceText.Text = "Total Space:"
        FreeSpaceText.Text = "Free Space:"
    End Sub

    Private Sub SpanishLanguage()
        Label1.Text = "Tamaño:"
        Label2.Text = "Procesado:"
        Label5.Text = "Arrastre archivos aquí para añadirlos a la lista"
        Label6.Text = "Por Moisés Cardona" & vbNewLine & "v1.8.3"
        Label7.Text = "Estado:"
        Label9.Text = "Porcentaje: "
        Label11.Text = "Archivos:"
        Label12.Text = "Subir a este ID de directorio (""root"" para subir a la raíz):"
        Label13.Text = "Tiempo Est."
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
        UploadButton.Text = "Subir"
        Button3.Text = "Borrar Lista"
        Button4.Text = "Refrescar Lista"
        Button5.Text = "Descargar Archivo"
        Button6.Text = "Remover archivo(s) de la lista"
        Button7.Text = "Guardar Checksums de los archivos"
        Button9.Text = "Obtener Nombre de la Carpeta"
        Button10.Text = "Atrás"
        If viewing_trash = False Then
            Button11.Text = "Ver Basura"
        ElseIf viewing_trash = True Then
            Button11.Text = "Ver Drive"
        End If
        Button12.Text = "Crear Carpeta"
        Button13.Text = "Subir archivo(s) a esta carpeta"
        Button14.Text = "Deseleccionar"
        btnLogout.Text = "Cerrar Sesión"
        GroupBox2.Text = "Información del archivo:"
        ActionsToolStripMenuItem.Text = "Acciones"
        CreateNewFolderToolStripMenuItem.Text = "Crear nueva carpeta"
        RenameToolStripMenuItem.Text = "Renombrar"
        SelectedFileToolStripMenuItem1.Text = "Archivo seleccionado"
        SelectedFolderToolStripMenuItem1.Text = "Carpeta seleccionada"
        RefreshListToolStripMenuItem.Text = "Refrescar Lista"
        MoveToTrashToolStripMenuItem.Text = "Mover a la Basura"
        SelectedFilesToolStripMenuItem.Text = "Archivo(s) seleccionados"
        SelectedFoldersToolStripMenuItem.Text = "Carpeta(s) seleccionadas"
        RestoreToolStripMenuItem.Text = "Restaurar"
        SelectedFilesToolStripMenuItem1.Text = "Archivo(s) seleccionados"
        SelectedFoldersToolStripMenuItem1.Text = "Carpeta(s) seleccionadas"
        SaveChecksumsToolStripMenuItem.Text = "Guardar checksums"
        SelectedFilesToolStripMenuItem2.Text = "Archivo(s) seleccionados"
        SelectedFolderToolStripMenuItem2.Text = "Carpeta seleccionada"
        CopyFileToRAMBeforeUploadingToolStripMenuItem.Text = "Copiar archivo a memoria antes de subirlo si hay memoria disponible"
        DonationsToolStripMenuItem.Text = "Donar"
        FileToolStripMenuItem.Text = "Archivo"
        UploadToolStripMenuItem.Text = "Subir"
        FileToolStripMenuItem1.Text = "Archivo(s)"
        FolderToolStripMenuItem.Text = "Carpeta"
        DownloadToolStripMenuItem.Text = "Descargar"
        SelectedFileToolStripMenuItem.Text = "Archivo(s) seleccionado(s)"
        SelectedFolderToolStripMenuItem.Text = "Carpeta seleccionada"
        HelpToolStripMenuItem.Text = "Ayuda"
        OptionsToolStripMenuItem.Text = "Opciones"
        OrderByToolStripMenuItem.Text = "Ordenar por"
        OrderByComboBox.Items.Clear()
        OrderByComboBox.Items.AddRange({"Fecha de creación", "Carpeta", "Modificado por mí", "Fecha de Modificación", "Nombre", "Nombre Natural", "Espacio usado", "Recientes", "Compartidos conmigo", "Estrellado", "Fecha de Acceso/Visto"})
        OrderByComboBox.SelectedIndex = My.Settings.SortByIndex
        DescendingOrderToolStripMenuItem.Text = "Ordenar Descendiente"
        PreserveFileModifiedDateToolStripMenuItem.Text = "Preservar fecha de modificación"
        SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Text = "Guardar checksums como checksums.md5"
        StartUploadsAutomaticallyToolStripMenuItem.Text = "Subir archivos automáticamente"
        SpecifyChunkSizeToolStripMenuItem.Text = "Especificar tamaño de pedazos"
        UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Text = "Actualizar vista de archivos y carpetas al terminar de subir un archivo"
        ReadmeToolStripMenuItem.Text = "Léeme / Ayuda"
        LoggedInAs.Text = "Usuario:"
        UsedSpaceText.Text = "Espacio Usado:"
        TotalSpaceText.Text = "Espacio Total:"
        FreeSpaceText.Text = "Espacio Libre:"
    End Sub

    Function MsgAndDialogLang(tag As String) As String
        Select Case tag
            Case "downloads_finished"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Download(s) Finished!"
                    Case "Spanish"
                        Return "Archivo(s) descargado(s)"
                    Case "TChinese"
                        Return "Download(s) Finished!"
                    Case Else
                End Select
            Case "folder_invaild"
                Select Case My.Settings.Language
                    Case "English"
                        Return "The specified folder is invalid. Do you want to change the folder? If you select No, your files will be uploaded to the root of Google Drive"
                    Case "Spanish"
                        Return "La carpeta especificada es invalida. Desea cambiar la carpeta? Si presiona No, sus archivos serán subidos a la raíz de Google Drive"
                    Case "TChinese"
                        Return "The specified folder is invalid. Do you want to change the folder? If you select No, your files will be uploaded to the root of Google Drive"
                    Case Else

                End Select
            Case "upload_finish"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Uploads Finished!"
                    Case "Spanish"
                        Return "Los archivos han terminado de subir."
                    Case "TChinese"
                        Return "完成上傳"
                    Case Else

                End Select
            Case "uploadstatus_copytoram"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Copying to RAM"
                    Case "Spanish"
                        Return "Copiando a RAM"
                    Case "TChinese"
                        Return "Copying to RAM"
                End Select
            Case "uploadstatus_complete"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Completed!!"
                    Case "Spanish"
                        Return "¡Completado!"
                    Case "TChinese"
                        Return "完成!!"
                End Select
            Case "uploadstatus_downloading"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Downloading..."
                    Case "Spanish"
                        Return "Descargando..."
                    Case "TChinese"
                        Return "下載中..."

                End Select
            Case "uploadstatus_starting"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Starting..."
                    Case "Spanish"
                        Return "Comenzando..."
                    Case "TChinese"
                        Return "開始中..."
                End Select
            Case "uploadstatus_uploading"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Uploading..."
                    Case "Spanish"
                        Return "Subiendo..."
                    Case "TChinese"
                        Return "上傳中..."

                End Select
            Case "uploadstatus_retry"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Retrying..."
                    Case "Spanish"
                        Return "Intentando..."
                    Case "TChinese"
                        Return "重試中..."

                End Select
            Case "uploadstatus_failed"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Failed..."
                    Case "Spanish"
                        Return "Error..."
                    Case "TChinese"
                        Return "出錯了..."

                End Select
            Case "resume_upload_question"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Resume previous upload?{0}{0}{1}"
                    Case "Spanish"
                        Return "¿Resumir carga anterior?{0}{0}{1}"
                    Case "TChinese"
                        Return "要不要恢復上次未完成上傳?{0}{0}{1}"
                End Select
            Case "resume_upload"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Resume Upload"
                    Case "Spanish"
                        Return "Resumir"
                    Case "TChinese"
                        Return "恢復上載"
                End Select
            Case "location_browse"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Browse for a location to save the file:"
                    Case "Spanish"
                        Return "Busque un lugar para descargar el archivo:"
                    Case "TChinese"
                        Return "請選擇地方儲存:"
                End Select
            Case "enter_name_for_folder"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Enter a name for the new folder:"
                    Case "Spanish"
                        Return "Escriba un nombre para la nueva carpeta:"
                    Case "TChinese"
                        Return "請為新文件夾改名:"
                End Select
            Case "enter_new_name"
                Select Case My.Settings.Language
                    Case "English"
                        Return "New Name:"
                    Case "Spanish"
                        Return "Nuevo nombre:"
                    Case "TChinese"
                        Return "New Name:"
                End Select
            Case "create_new_folder"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Create new Folder"
                    Case "Spanish"
                        Return "Crear nueva carpeta"
                    Case "TChinese"
                        Return "增加新文件夾"
                End Select
            Case "rename_dialog"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Rename"
                    Case "Spanish"
                        Return "Renombrar"
                    Case "TChinese"
                        Return "Rename"
                End Select
            Case "checksum_location"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Browse for a location to save the checksum file:"
                    Case "Spanish"
                        Return "Busque un lugar para guardar el archivo del checksum:"
                    Case "TChinese"
                        Return "請選擇地方儲存校驗碼:"
                End Select
            Case "confirm_move_selected_folder2trash"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you really want to move the selected folders to the Trash?"
                    Case "Spanish"
                        Return "¿Está seguro de querer mover las carpetas seleccionadas a la Basura?"
                    Case "TChinese"
                        Return "Do you really want to move the selected folders to the Trash?"
                End Select
            Case "folders_moved2trash"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Folders moved to trash"
                    Case "Spanish"
                        Return "Las carpetas se movieron a la basura."
                    Case "TChinese"
                        Return "文件夾已移到垃圾桶"
                End Select
            Case "folder_moved2trash"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Folder moved to trash"
                    Case "Spanish"
                        Return "La carpeta se movió a la basura"
                    Case "TChinese"
                        Return "文件夾已移到垃圾桶"
                End Select
            Case "checksums_saved"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Checksum(s) Saved!"
                    Case "Spanish"
                        Return "Los Checksums han sido guardados"
                    Case "TChinese"
                        Return "???"
                End Select
            Case "confirm_move_folder2trash_part1"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you really want to move the folder "
                    Case "Spanish"
                        Return "¿Está seguro de querer mover la carpeta "
                    Case "TChinese"
                        Return "你真係想將此文件夾 "
                End Select
            Case "confirm_move_folder2trash_part2"
                Select Case My.Settings.Language
                    Case "English"
                        Return " to the Trash?"
                    Case "Spanish"
                        Return " a la Basura?"
                    Case "TChinese"
                        Return " 移到垃圾桶?"
                End Select
            Case "confirm_restore_selected_files"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you want to restore the selected files?"
                    Case "Spanish"
                        Return "¿Está seguro de querer restaurar los archivos seleccionados?"
                    Case "TChinese"
                        Return "你真係想還原所選的文件夾?"
                End Select
            Case "confirm_restore_selected_folders"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you want to restore the selected folders?"
                    Case "Spanish"
                        Return "¿Está seguro de querer restaurar las carpetas seleccionados?"
                    Case "TChinese"
                        Return "你真係想還原所選的文件夾?"
                End Select
            Case "restore_folder_part1"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you want to restore the folder "
                    Case "Spanish"
                        Return "¿Está seguro de querer restaurar la carpeta "
                    Case "TChinese"
                        Return "你真係想還原文件夾"
                End Select
            Case "restore_folder_part2"
                Select Case My.Settings.Language
                    Case "English"
                        Return "?"
                    Case "Spanish"
                        Return "?"
                    Case "TChinese"
                        Return "?"
                End Select
            Case "folder_restored"
                Select Case My.Settings.Language
                    Case "English"
                        Return "The Folder has been restored"
                    Case "Spanish"
                        Return "La carpeta ha sido restaurada"
                    Case "TChinese"
                        Return "文件夾巳還原"
                End Select
            Case "folders_restored"
                Select Case My.Settings.Language
                    Case "English"
                        Return "The Folders have been restored"
                    Case "Spanish"
                        Return "Las carpetas han sido restauradas"
                    Case "TChinese"
                        Return "文件夾巳還原"
                End Select
            Case "move_selected_file2trash"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you really want to move the selected files to the Trash?"
                    Case "Spanish"
                        Return "¿Está seguro de querer mover los archivos seleccionados a la Basura?"
                    Case "TChinese"
                        Return "Do you really want to move the selected files to the Trash?"
                End Select
            Case "file_moved2trash"
                Select Case My.Settings.Language
                    Case "English"
                        Return "File moved to trash"
                    Case "Spanish"
                        Return "El archivo se movió a la basura."
                    Case "TChinese"
                        Return "檔案已移到垃圾桶"
                End Select
            Case "files_moved2trash"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Files moved to trash"
                    Case "Spanish"
                        Return "Los archivos se movieron a la basura."
                    Case "TChinese"
                        Return "檔案已移到垃圾桶"
                End Select
            Case "move_file2trash_part1"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you really want to move the file "
                    Case "Spanish"
                        Return "¿Está seguro de querer mover el archivo "
                    Case "TChinese"
                        Return "你真係想將 "
                End Select
            Case "move_file2trash_part2"
                Select Case My.Settings.Language
                    Case "English"
                        Return " to the Trash?"
                    Case "Spanish"
                        Return " a la Basura?"
                    Case "TChinese"
                        Return " 移到垃圾桶?"
                End Select
            Case "confirm_restrore_selected_file"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you want to restore the selected files?"
                    Case "Spanish"
                        Return "¿Está seguro de querer restaurar los archivos seleccionados?"
                    Case "TChinese"
                        Return "Do you want to restore the selected files?"
                End Select
            Case "file_restored"
                Select Case My.Settings.Language
                    Case "English"
                        Return "The File has been restored"
                    Case "Spanish"
                        Return "El archivo ha sido restaurado"
                    Case "TChinese"
                        Return "文件巳還原"
                End Select
            Case "files_restored"
                Select Case My.Settings.Language
                    Case "English"
                        Return "The Files have been restored"
                    Case "Spanish"
                        Return "Los archivos han sido restaurados"
                    Case "TChinese"
                        Return "文件巳還原"
                End Select
            Case "restore_file_part1"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you want to restore the file "
                    Case "Spanish"
                        Return "¿Desea restaurar el archivo "
                    Case "TChinese"
                        Return "Do you want to restore the file "
                End Select
            Case "restore_file_part2"
                Select Case My.Settings.Language
                    Case "English"
                        Return "?"
                    Case "Spanish"
                        Return "?"
                    Case "TChinese"
                        Return "?"
                End Select
            Case "logged_out"
                Select Case My.Settings.Language
                    Case "English"
                        Return "You have been logged out. The software will now close"
                    Case "Spanish"
                        Return "Tu sesión ha sido cerrada. El programa cerrará ahora"
                    Case "TChinese"
                        Return "You have been logged out. The software will now close"
                End Select
            Case Else
                Return "Error Typo " & tag
        End Select
        Return tag & " not found"
    End Function

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        FolderToUploadFileListBox.Items.Item(UploadsListBox.SelectedIndex) = CurrentFolder
        FolderIDTextBox.Text = CurrentFolder
        GetFolderIDName(False)
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        UploadsListBox.ClearSelected()
    End Sub

    Private Sub ListBox2_KeyDown(sender As Object, e As KeyEventArgs) Handles UploadsListBox.KeyDown
        If e.Modifiers = Keys.Alt And e.KeyCode = Keys.A Then
            For i = 0 To UploadsListBox.Items.Count - 1
                UploadsListBox.SetSelected(i, True)
            Next
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles GoToRootLink.LinkClicked
        My.Settings.PreviousFolderIDs.Clear()
        My.Settings.LastFolder = "root"
        My.Settings.Save()
        CurrentFolder = "root"
        CurrentFolderLabel.Text = GetCurrentFolderIDName()
        RefreshFileList(CurrentFolder)
    End Sub

    Private Sub PreserveFileModifiedDateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PreserveFileModifiedDateToolStripMenuItem.Click
        My.Settings.PreserveModifiedDate = PreserveFileModifiedDateToolStripMenuItem.Checked
        My.Settings.Save()
    End Sub

    Private Sub StartUploadsAutomaticallyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartUploadsAutomaticallyToolStripMenuItem.Click
        My.Settings.AutomaticUploads = StartUploadsAutomaticallyToolStripMenuItem.Checked
        My.Settings.Save()
    End Sub

    Private Sub ReadmeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReadmeToolStripMenuItem.Click
        If RadioButton1.Checked Then
            Process.Start("https://github.com/moisesmcardona/GoogleDriveUploadTool/blob/master/README.md")
        ElseIf RadioButton2.Checked Then
            Process.Start("https://github.com/moisesmcardona/GoogleDriveUploadTool/blob/master/LEEME.md")
        Else
            Process.Start("https://github.com/moisesmcardona/GoogleDriveUploadTool/blob/master/README.md")
        End If
    End Sub

    Private Sub DonationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DonationsToolStripMenuItem.Click
        Donations.ShowDialog()
    End Sub

    Private Sub SaveCheckumsAsChecksumsmd5ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Click
        My.Settings.SaveAsChecksumsMD5 = SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Checked
        My.Settings.Save()
    End Sub

    Private Sub UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Click
        My.Settings.UpdateViews = UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Checked
        My.Settings.Save()
    End Sub

    Private Sub DescendingOrderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DescendingOrderToolStripMenuItem.Click
        My.Settings.OrderDesc = DescendingOrderToolStripMenuItem.Checked
        My.Settings.Save()
        RefreshFileList(CurrentFolder)
    End Sub

    Private Sub OrderByComboBox_DropDownClosed(sender As Object, e As EventArgs) Handles OrderByComboBox.DropDownClosed
        If OrderByComboBox.SelectedIndex = 0 Then
            My.Settings.SortBy = "createdTime"
        ElseIf OrderByComboBox.SelectedIndex = 1 Then
            My.Settings.SortBy = "folder"
        ElseIf OrderByComboBox.SelectedIndex = 2 Then
            My.Settings.SortBy = "modifiedByMeTime"
        ElseIf OrderByComboBox.SelectedIndex = 3 Then
            My.Settings.SortBy = "modifiedTime"
        ElseIf OrderByComboBox.SelectedIndex = 4 Then
            My.Settings.SortBy = "name"
        ElseIf OrderByComboBox.SelectedIndex = 5 Then
            My.Settings.SortBy = "name_natural"
        ElseIf OrderByComboBox.SelectedIndex = 6 Then
            My.Settings.SortBy = "quotaBytesUsed"
        ElseIf OrderByComboBox.SelectedIndex = 7 Then
            My.Settings.SortBy = "recency"
        ElseIf OrderByComboBox.SelectedIndex = 8 Then
            My.Settings.SortBy = "sharedWithMeTime"
        ElseIf OrderByComboBox.SelectedIndex = 9 Then
            My.Settings.SortBy = "starred"
        ElseIf OrderByComboBox.SelectedIndex = 10 Then
            My.Settings.SortBy = "viewedByMeTime"
        End If
        My.Settings.SortByIndex = OrderByComboBox.SelectedIndex
        My.Settings.Save()
        RefreshFileList(CurrentFolder)
    End Sub

    Private Sub FileToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles FileToolStripMenuItem1.Click
        OpenFileDialog1.Title = "Select files to upload"
        OpenFileDialog1.Filter = "All Files (*)|*.*"
        Dim DialogResultVar As DialogResult = OpenFileDialog1.ShowDialog
        If DialogResultVar = DialogResult.OK Then
            If OpenFileDialog1.FileNames IsNot Nothing Then
                UploadsListBox.Items.AddRange(OpenFileDialog1.FileNames)
                FolderToUploadFileListBox.Items.Add(CurrentFolder)
                My.Settings.UploadQueue.AddRange(OpenFileDialog1.FileNames)
                My.Settings.UploadQueueFolders.Add(CurrentFolder)
            End If
        End If
    End Sub

    Private Sub FolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FolderToolStripMenuItem.Click
        FolderBrowserDialog1.ShowNewFolderButton = False
        Dim FolderBrowserDialogResponse As DialogResult = FolderBrowserDialog1.ShowDialog
        If FolderBrowserDialogResponse = DialogResult.OK Then
            UploadsListBox.Items.Add(FolderBrowserDialog1.SelectedPath)
            FolderToUploadFileListBox.Items.Add(CurrentFolder)
            My.Settings.UploadQueue.Add(FolderBrowserDialog1.SelectedPath)
            My.Settings.UploadQueueFolders.Add(CurrentFolder)
            GetDirectoriesAndFiles(New IO.DirectoryInfo(FolderBrowserDialog1.SelectedPath))
        End If
    End Sub

    Private Sub SelectedFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectedFileToolStripMenuItem.Click
        CheckForFilesDownload()
    End Sub

    Private Sub SelectedFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectedFolderToolStripMenuItem.Click
        CheckForFolderDownload()
    End Sub
    Private Sub CheckForFilesDownload()
        If FilesListBox.SelectedItems.Count >= 1 Then
            If FilesListBox.SelectedItems.Count = 1 Then
                BrowseToDownloadFile()
            Else
                DownloadFilesAndFolders()
            End If
        End If
    End Sub
    Private Sub CheckForFolderDownload()
        If FolderListBox.SelectedItem IsNot Nothing Then
            EnterFolder()
            DownloadFilesAndFolders(True)
        End If
    End Sub

    Private Sub SpecifyChunkSizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SpecifyChunkSizeToolStripMenuItem.Click
        UploadChunkSize.ShowDialog()
    End Sub

    Private Sub CopyFileToRAMBeforeUploadingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyFileToRAMBeforeUploadingToolStripMenuItem.Click
        My.Settings.CopyToRAM = CopyFileToRAMBeforeUploadingToolStripMenuItem.Checked
        My.Settings.Save()
    End Sub

    Private Sub CreateNewFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateNewFolderToolStripMenuItem.Click
        CreateFolder()
    End Sub

    Private Sub SelectedFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectedFilesToolStripMenuItem.Click
        If viewing_trash = False Then
            If FilesListBox.SelectedItem IsNot Nothing Then
                WorkWithTrash(FilesListBox.SelectedItems, True, True)
            End If
        End If
    End Sub

    Private Sub SelectedFoldersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectedFoldersToolStripMenuItem.Click
        If viewing_trash = False Then
            If FolderListBox.SelectedItem IsNot Nothing Then
                WorkWithTrash(FolderListBox.SelectedItems, False, True)
            End If
        End If
    End Sub

    Private Sub SelectedFilesToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SelectedFilesToolStripMenuItem1.Click
        If viewing_trash Then
            If FilesListBox.SelectedItem IsNot Nothing Then
                WorkWithTrash(FilesListBox.SelectedItems, True, False)
            End If
        End If
    End Sub

    Private Sub SelectedFolderssToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectedFoldersToolStripMenuItem1.Click
        If viewing_trash Then
            WorkWithTrash(FolderListBox.SelectedItems, False, False)
        End If
    End Sub

    Private Sub SelectedFileToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SelectedFileToolStripMenuItem1.Click
        If FilesListBox.SelectedItem IsNot Nothing Then
            RenameFileOrFolder(FileIdsListBox.Items.Item(FilesListBox.Items.IndexOf(FilesListBox.SelectedItem)).ToString)
        End If
    End Sub

    Private Sub SelectedFolderToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SelectedFolderToolStripMenuItem1.Click
        If FolderListBox.SelectedItem IsNot Nothing Then
            RenameFileOrFolder(FolderIdsListBox.Items.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)).ToString)
        End If
    End Sub

    Private Sub RefreshListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RefreshListToolStripMenuItem.Click
        If viewing_trash Then
            RefreshFileList("trash")
        Else
            RefreshFileList(CurrentFolder)
        End If
    End Sub

    Private Sub SelectedFilesToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles SelectedFilesToolStripMenuItem2.Click
        If My.Settings.SaveAsChecksumsMD5 Then
            SaveChecksumsFile("checksums.md5")
        Else
            If FilesListBox.SelectedItems.Count > 1 Then
                SaveChecksumsFile(GetCurrentFolderIDName() & ".md5")
            Else
                SaveChecksumsFile(FilesListBox.SelectedItem.ToString & ".md5")
            End If
        End If
    End Sub

    Private Sub SelectedFolderToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles SelectedFolderToolStripMenuItem2.Click
        If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksums.md5") Else SaveChecksumsFile(GetCurrentFolderIDName() & ".md5", True)
    End Sub

    Private Sub FolderListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FolderListBox.SelectedIndexChanged
        If FolderListBox.SelectedItem IsNot Nothing Then
            If UploadButton.Enabled = True And UploadsListBox.SelectedItem Is Nothing Then
                FolderIDTextBox.Text = FolderIdsListBox.Items.Item(FolderListBox.SelectedIndex)
                GetFolderIDName(False)
            End If
        End If
    End Sub

    Private Sub FilesListBox_MouseDoubleClick(sender As Object, e As EventArgs) Handles FilesListBox.MouseDoubleClick
        If FilesListBox.SelectedItem IsNot Nothing Then
            Process.Start("https://drive.google.com/file/d/" + FileIdsListBox.Items.Item(FilesListBox.SelectedIndex) + "/view")
        End If
    End Sub
End Class
