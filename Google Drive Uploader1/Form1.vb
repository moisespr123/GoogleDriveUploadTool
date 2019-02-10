Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Drive.v3
Imports Google.Apis.Services
Imports Google.Apis.Util.Store
Imports System.IO
Imports System.Threading
Imports Google.Apis.Upload
Imports Google.Apis.Download
Imports System.Collections.Specialized
Imports System.Net

Public Class Form1
    Private FileNameList As New List(Of String)
    Private FileIdsList As New List(Of String)
    Private FileSizeList As New List(Of Long?)
    Private FileMIMEList As New List(Of String)
    Private FolderToUploadFileList As New List(Of String)
    Private FileModifiedTimeList As New List(Of Date?)
    Private FileCreatedTimeList As New List(Of Date?)
    Private FileMD5List As New List(Of String)
    Private FolderNameList As New List(Of String)
    Private FolderIdsList As New List(Of String)
    Private PreviousFolderId As New List(Of String)
    Public viewing_trash As Boolean = False
    Private credential As UserCredential = Nothing
    Public CurrentFolder As String = "root"
    Shared Scopes As String() = {DriveService.Scope.DriveFile, DriveService.Scope.Drive}
    Shared SoftwareName As String = "Google Drive Uploader Tool"
    Public service As DriveService
    Public pageToken As String = ""
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load         'Initialize Upload Queue Collection
        BackButton.Enabled = False
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
                FolderToUploadFileList.Add(item)
            Next
        End If
        'Google Drive initialization
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
                PreviousFolderId.Add(item)
            Next
            SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Checked = My.Settings.SaveAsChecksumsMD5
            StartUploadsAutomaticallyToolStripMenuItem.Checked = My.Settings.AutomaticUploads
            PreserveFileModifiedDateToolStripMenuItem.Checked = My.Settings.PreserveModifiedDate
            UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Checked = My.Settings.UpdateViews
            ChecksumsEncodeFormatComboBox.SelectedIndex = My.Settings.EncodeChecksumsFormat
            OrderByComboBox.SelectedIndex = My.Settings.SortByIndex
            DescendingOrderToolStripMenuItem.Checked = My.Settings.OrderDesc
            CopyFileToRAMBeforeUploadingToolStripMenuItem.Checked = My.Settings.CopyToRAM
            RefreshFileList(CurrentFolder)
            CurrentFolderLabel.Text = GetCurrentFolderIDName()
            If UploadsListBox.Items.Count > 0 Then
                UploadsListBox.SelectedIndex = 0
            Else
                FolderIDTextBox.Text = CurrentFolder
                GetFolderIDName(False)
            End If
        Catch
            MsgBox(Translations.MsgAndDialogLang("client_secrets_not_found"))
            Process.Start("https://developers.google.com/drive/v3/web/quickstart/dotnet")
            Me.Close()
        End Try

    End Sub

    Public Sub Lang_Select()
        If String.IsNullOrEmpty(My.Settings.Language) Then
            My.Settings.Language = "English"
            My.Settings.Save()
            EnglishRButton.Checked = True
            Translations.EnglishLanguage()
        Else
            Select Case My.Settings.Language
                Case "English"
                    Translations.EnglishLanguage()
                    EnglishRButton.Checked = True
                Case "Spanish"
                    Translations.SpanishLanguage()
                    SpanishRButton.Checked = True
                Case "TChinese"
                    Translations.TChineseLanguage()
                    TChineseRButton.Checked = True
                Case Else
                    Translations.EnglishLanguage()
                    EnglishRButton.Checked = True
            End Select
        End If
    End Sub

    Private starttime As Date
    Private timespent As TimeSpan
    Private GetFile As String = ""
    Private UploadFailed As Boolean = False
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles UploadButton.Click
        CheckBeforeStartingUpload()
    End Sub
    Private ResumeFromError As Boolean = False
    Private Sub CheckBeforeStartingUpload()
        If UploadsListBox.Items.Count > 0 Then
            FolderIDTextBox.Text = FolderToUploadFileList.Item(0)
            If GetFolderIDName(False) Then
                My.Settings.LastFolder = CurrentFolder
                My.Settings.Save()
                ResumeFromError = False
                UploadButton.Enabled = False
                UploadFiles()
            Else
                If MsgBox(Translations.MsgAndDialogLang("folder_invaild"), MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.No Then
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
                FolderIDTextBox.Text = FolderToUploadFileList.Item(0)
                GetFolderIDName(False)
                If File.Exists(GetFile) Then
                    FileSizeFromCurrentUploadLabel.Text = String.Format("{0:N2} MB", My.Computer.FileSystem.GetFileInfo(GetFile).Length / 1024 / 1024)
                    ProgressBar1.Maximum = CInt(My.Computer.FileSystem.GetFileInfo(GetFile).Length / 1024 / 1024)
                    Dim FileMetadata As New Data.File With {
                        .Name = My.Computer.FileSystem.GetName(GetFile)
                    }
                    If My.Settings.PreserveModifiedDate Then FileMetadata.ModifiedTime = File.GetLastWriteTimeUtc(GetFile)
                    Dim FileFolder As New List(Of String)
                    If FolderCreated = False Then
                        FileFolder.Add(FolderIDTextBox.Text)
                    Else
                        Dim DirectoryName As String = ""
                        DirectoryName = Path.GetDirectoryName(GetFile)
                        For Each directory In DirectoryList
                            If DirectoryName = directory Then
                                FileFolder.Add(DirectoryListID.Item(DirectoryList.IndexOf(directory)))
                            End If
                        Next
                        If FileFolder.Count = 0 Then FileFolder.Add(FolderIDTextBox.Text)
                    End If
                    FileMetadata.Parents = FileFolder
                    Dim UploadStream As New FileStream(GetFile, FileMode.Open, FileAccess.Read)
                    Dim FileInRAM As New MemoryTributary.MemoryTributary()
                    Dim UploadFile As FilesResource.CreateMediaUpload = Nothing
                    Dim UsingRAM As Boolean = False
                    If CopyFileToRAMBeforeUploadingToolStripMenuItem.Checked Then
                        If My.Computer.Info.AvailablePhysicalMemory > My.Computer.FileSystem.GetFileInfo(GetFile).Length Then
                            Dim RAMMultiplier = 1024 * 1024
                            If My.Computer.FileSystem.FileExists("rammultiplier.txt") Then
                                If String.IsNullOrEmpty(My.Computer.FileSystem.ReadAllText("rammultiplier.txt")) = False Then
                                    RAMMultiplier = CInt(My.Computer.FileSystem.ReadAllText("rammultiplier.txt"))
                                    If RAMMultiplier = 0 Then RAMMultiplier = 4
                                Else
                                    RAMMultiplier = 4
                                End If
                            End If
                            Dim readChunkSize = 1024 * RAMMultiplier
                            starttime = Date.Now()
                            UploadStream.Seek(0, SeekOrigin.Begin)
                            FileSizeFromCurrentUploadLabel.Text = String.Format("{0:N2} MB", My.Computer.FileSystem.GetFileInfo(GetFile).Length / 1024 / 1024)
                            ProgressBar1.Maximum = CInt(My.Computer.FileSystem.GetFileInfo(GetFile).Length / 1024 / 1024)
                            Me.Update()
                            While UploadStream.Position < UploadStream.Length
                                Dim RemainingBytes As Long = UploadStream.Length - UploadStream.Position
                                If RemainingBytes <= 1024 * RAMMultiplier Then
                                    Dim ChunkSize As Integer = Convert.ToInt32(RemainingBytes)
                                    Dim buffer(ChunkSize) As Byte
                                    UploadStream.Read(buffer, 0, ChunkSize)
                                    FileInRAM.Write(buffer, 0, ChunkSize)
                                Else
                                    Dim buffer(readChunkSize) As Byte
                                    UploadStream.Read(buffer, 0, readChunkSize)
                                    FileInRAM.Write(buffer, 0, readChunkSize)
                                End If
                                UpdateBytesSent(UploadStream.Position, Translations.MsgAndDialogLang("uploadstatus_copytoram"), starttime)
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
                    starttime = Date.Now
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
                        FileInRAM = New MemoryTributary.MemoryTributary()
                    Else
                        UploadStream.Dispose()
                        UploadStream.Close()
                    End If
                ElseIf Directory.Exists(GetFile) Then
                    Dim ParentFolder As New List(Of String)
                    If FolderCreated = True Then
                        Dim DirectoryName As String = ""
                        DirectoryName = Path.GetDirectoryName(GetFile)
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
                    Dim CreateFolder As FilesResource.CreateRequest = service.Files.Create(New Data.File With {
                        .Name = My.Computer.FileSystem.GetName(GetFile),
                        .Parents = ParentFolder,
                        .MimeType = "application/vnd.google-apps.folder"
                    })
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
                FolderToUploadFileList.RemoveAt(0)
                My.Settings.UploadQueue.RemoveAt(0)
                My.Settings.UploadQueueFolders.RemoveAt(0)
                My.Settings.Save()
                ResumeFromError = False
                If UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Checked Then RefreshFileList(CurrentFolder)
                UpdateQuota()
            End If
        End While
        If EnglishRButton.Checked = True Then MsgBox(Translations.MsgAndDialogLang("upload_finish"))
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
    Private UploadCancellationToken As CancellationToken
    Private Sub Upload_ProgressChanged(uploadStatusInfo As IUploadProgress)
        Select Case uploadStatusInfo.Status
            Case UploadStatus.Completed
                UploadFailed = False
                ResumeFromError = False
                UpdateBytesSent(My.Computer.FileSystem.GetFileInfo(GetFile).Length, Translations.MsgAndDialogLang("uploadstatus_complete"), starttime)
            Case UploadStatus.Starting
                UpdateBytesSent(0, Translations.MsgAndDialogLang("uploadstatus_starting"), starttime)
            Case UploadStatus.Uploading
                UploadFailed = False
                UpdateBytesSent(uploadStatusInfo.BytesSent, Translations.MsgAndDialogLang("uploadstatus_uploading"), starttime)
            Case UploadStatus.Failed
                UploadFailed = True
                UpdateBytesSent(uploadStatusInfo.BytesSent, Translations.MsgAndDialogLang("uploadstatus_retry"), starttime)
                ResumeFromError = True
                Thread.Sleep(1000)
        End Select
    End Sub
    Private Sub Upload_ResponseReceived(file As Data.File)
        UpdateBytesSent(My.Computer.FileSystem.GetFileInfo(GetFile).Length, Translations.MsgAndDialogLang("uploadstatus_complete"), starttime)
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
            If EnglishRButton.Checked = True Then
                ResumeText1 = "Resume previous upload?{0}{0}{1}"
                ResumeText2 = "Resume Upload"

            Else
                ResumeText1 = "¿Resumir carga anterior?{0}{0}{1}"
                ResumeText2 = "Resumir"
            End If
            If Ask = True Then
                If MsgBox(String.Format(Translations.MsgAndDialogLang("resume_upload_question"), vbNewLine, GetFile), MsgBoxStyle.Question Or MsgBoxStyle.YesNo, Translations.MsgAndDialogLang("resume_upload")) = MsgBoxResult.Yes Then
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

    Private Delegate Sub UpdateBytesSentInvoker(BytesSent As Long, StatusText As String, startTime As Date)
    Private Sub UpdateBytesSent(BytesSent As Long, StatusText As String, startTime As Date)
        If StatusLabel.InvokeRequired Then
            StatusLabel.Invoke(New UpdateBytesSentInvoker(AddressOf UpdateBytesSent), BytesSent, StatusText, startTime)
        Else
            StatusLabel.Text = StatusText
        End If
        If ProcessedFileSizeFromCurrentUploadLabel.InvokeRequired Then
            ProcessedFileSizeFromCurrentUploadLabel.Invoke(New UpdateBytesSentInvoker(AddressOf UpdateBytesSent), BytesSent, StatusText, startTime)
        Else
            ProcessedFileSizeFromCurrentUploadLabel.Text = String.Format("{0:N2} MB", BytesSent / 1024 / 1024)
        End If
        If BytesSent > 0 Then
            If ProgressBar1.InvokeRequired Then
                ProgressBar1.Invoke(New UpdateBytesSentInvoker(AddressOf UpdateBytesSent), BytesSent, StatusText, startTime)
            Else
                If ProgressBar1.Maximum >= CInt(BytesSent / 1024 / 1024) Then
                    ProgressBar1.Value = CInt(BytesSent / 1024 / 1024)
                    PercentLabel.Text = String.Format("{0:N2}%", ((ProgressBar1.Value / ProgressBar1.Maximum) * 100))
                    timespent = Date.Now - startTime
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
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles DownloadFileButton.Click
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
            TotalSpace.Text = Translations.MsgAndDialogLang("unlimited")
            FreeSpace.Text = Translations.MsgAndDialogLang("unlimited")
        End If
    End Sub
    Private Async Sub BrowseToDownloadFile()
        SaveFileDialog1.Title = Translations.MsgAndDialogLang("location_browse")
        SaveFileDialog1.FileName = FilesListBox.SelectedItem.ToString
        Dim SFDResult As DialogResult = SaveFileDialog1.ShowDialog()
        If SFDResult = DialogResult.OK Then
            Await DownloadFile(SaveFileDialog1.FileName, FileIdsList.Item(FilesListBox.SelectedIndex), FileSizeList(FilesListBox.SelectedIndex), FileModifiedTimeList(FilesListBox.SelectedIndex))
        End If
    End Sub
    Private Async Function DownloadFile(Location As String, FileName As String, FileSize As Long?, ModifiedTime As Date?) As Task
        starttime = Date.Now
        FileSizeFromCurrentUploadLabel.Text = String.Format("{0:N2} MB", FileSize / 1024 / 1024)
        ProgressBar1.Maximum = CInt(FileSize / 1024 / 1024)
        MaxFileSize = Convert.ToDouble(FileSize)
        Dim FileToSave As FileStream = New FileStream(Location, FileMode.Create, FileAccess.Write)
        Dim DownloadRequest As FilesResource.GetRequest = service.Files.Get(FileName)
        AddHandler DownloadRequest.MediaDownloader.ProgressChanged, New Action(Of IDownloadProgress)(AddressOf Download_ProgressChanged)
        Await DownloadRequest.DownloadAsync(FileToSave)
        FileToSave.Close()
        File.SetLastWriteTime(Location, ModifiedTime.Value)
    End Function
    Private Sub Download_ProgressChanged(progress As IDownloadProgress)
        Select Case progress.Status
            Case DownloadStatus.Completed
                UpdateBytesSent(Convert.ToInt64(MaxFileSize), Translations.MsgAndDialogLang("uploadstatus_complete"), starttime)
            Case DownloadStatus.Downloading
                UpdateBytesSent(progress.BytesDownloaded, Translations.MsgAndDialogLang("uploadstatus_downloading"), starttime)
            Case DownloadStatus.Failed
                UpdateBytesSent(progress.BytesDownloaded, Translations.MsgAndDialogLang("uploadstatus_failed"), starttime)
        End Select
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles RefreshListButton.Click
        If viewing_trash = False Then RefreshFileList(CurrentFolder) Else RefreshFileList("trash")
    End Sub
    Private Delegate Sub RefreshFileListInvoker(FolderID As String)
    Public Sub RefreshFileList(FolderID As String)
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
        FileNameList.Clear()
        FileIdsList.Clear()
        FileSizeList.Clear()
        FileModifiedTimeList.Clear()
        FileCreatedTimeList.Clear()
        FileMD5List.Clear()
        FileMIMEList.Clear()
        Dim PageToken1 As String = String.Empty
        Dim OrderBy As String = My.Settings.SortBy
        If My.Settings.OrderDesc Then OrderBy = OrderBy + " desc"
        If Not My.Settings.SortBy.Equals("name") Then OrderBy = OrderBy + ",name"
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
                        FileNameList.Add(file.Name)
                        FileIdsList.Add(file.Id)
                        If file.Size IsNot Nothing Then FileSizeList.Add(file.Size) Else FileSizeList.Add(0)
                        FileModifiedTimeList.Add(file.ModifiedTime)
                        FileCreatedTimeList.Add(file.CreatedTime)
                        If file.Md5Checksum IsNot Nothing Then FileMD5List.Add(file.Md5Checksum) Else FileMD5List.Add("")
                        FileMIMEList.Add(file.MimeType)
                    Next
                End If
                PageToken1 = files.NextPageToken
            Catch ex As Exception
            End Try
        Loop While PageToken1 = String.Empty = False
        Dim FileCountNumber As Integer = FileNameList.Count
        If FileCountNumber > 1 Then
            FileCount.Text = FileCountNumber.ToString + Translations.MsgAndDialogLang("files_txt")
        ElseIf FileCountNumber = 1 Then
            FileCount.Text = FileCountNumber.ToString + Translations.MsgAndDialogLang("file_txt")
        Else
            FileCount.Text = "0" + Translations.MsgAndDialogLang("files_txt")
        End If
        FolderNameList.Clear()
        FolderIdsList.Clear()
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
                        FolderNameList.Add(file.Name)
                        FolderIdsList.Add(file.Id)
                    Next
                End If
                PageToken2 = files.NextPageToken
            Catch ex As Exception
            End Try
        Loop While PageToken2 = String.Empty = False
        FolderListBox.DataSource = Nothing
        FolderListBox.DataSource = FolderNameList
        FilesListBox.DataSource = Nothing
        FilesListBox.DataSource = FileNameList
        If CurrentFolder = "root" Or CurrentFolder = "trash" Then
            BackButton.Enabled = False
        Else
            BackButton.Enabled = True
        End If
        UpdateQuota()
    End Sub
    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim filepath() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())
        For Each path In filepath
            If Directory.Exists(path) Then
                UploadsListBox.Items.Add(path)
                FolderToUploadFileList.Add(CurrentFolder)
                My.Settings.UploadQueue.Add(path)
                My.Settings.UploadQueueFolders.Add(CurrentFolder)
                GetDirectoriesAndFiles(New DirectoryInfo(path))
            Else
                UploadsListBox.Items.Add(path)
                FolderToUploadFileList.Add(CurrentFolder)
                My.Settings.UploadQueue.Add(path)
                My.Settings.UploadQueueFolders.Add(CurrentFolder)
            End If
        Next
        My.Settings.Save()
        If My.Settings.AutomaticUploads And UploadButton.Enabled = True Then
            CheckBeforeStartingUpload()
        End If
    End Sub
    Private Sub GetDirectoriesAndFiles(ByVal BaseFolder As DirectoryInfo)
        UploadsListBox.Items.AddRange((From FI As FileInfo In BaseFolder.GetFiles Select FI.FullName).ToArray)
        My.Settings.UploadQueue.AddRange((From FI As FileInfo In BaseFolder.GetFiles Select FI.FullName).ToArray)
        For Each FI As FileInfo In BaseFolder.GetFiles()
            FolderToUploadFileList.Add(CurrentFolder)
            My.Settings.UploadQueueFolders.Add(CurrentFolder)
        Next
        For Each subF As DirectoryInfo In BaseFolder.GetDirectories()
            Application.DoEvents()
            UploadsListBox.Items.Add(subF.FullName)
            FolderToUploadFileList.Add(CurrentFolder)
            My.Settings.UploadQueue.Add(subF.FullName)
            My.Settings.UploadQueueFolders.Add(CurrentFolder)
            GetDirectoriesAndFiles(subF)
        Next
    End Sub
    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles EnglishRButton.CheckedChanged
        If EnglishRButton.Checked Then
            Translations.EnglishLanguage()
            My.Settings.Language = "English"
            My.Settings.Save()
            If service IsNot Nothing Then RefreshFileList(CurrentFolder)
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles SpanishRButton.CheckedChanged
        If SpanishRButton.Checked Then
            Translations.SpanishLanguage()
            My.Settings.Language = "Spanish"
            My.Settings.Save()
            If service IsNot Nothing Then RefreshFileList(CurrentFolder)
        End If
    End Sub
    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles TChineseRButton.CheckedChanged
        If TChineseRButton.Checked Then
            Translations.TChineseLanguage()
            My.Settings.Language = "TChinese"
            My.Settings.Save()
            If service IsNot Nothing Then RefreshFileList(CurrentFolder)
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles RemoveSelectedFilesFromList.Click
        Do While (UploadsListBox.SelectedItems.Count > 0)
            Dim CurrentIndex = UploadsListBox.SelectedIndex
            UploadsListBox.Items.RemoveAt(CurrentIndex)
            My.Settings.UploadQueue.RemoveAt(CurrentIndex)
            FolderToUploadFileList.RemoveAt(CurrentIndex)
            My.Settings.UploadQueueFolders.RemoveAt(CurrentIndex)
            My.Settings.Save()
        Loop
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles ClearUploadQueueButton.Click
        UploadsListBox.Items.Clear()
        My.Settings.UploadQueue.Clear()
        FolderToUploadFileList.Clear()
        My.Settings.UploadQueueFolders.Clear()
        My.Settings.Save()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles GetFolderIdNameButton.Click
        GetFolderIDName(True)
    End Sub

    Private Function GetFolderIDName(ShowMessage As Boolean) As Boolean
        If String.IsNullOrEmpty(FolderIDTextBox.Text) = False Then
            Try
                Dim GetFolderName As FilesResource.GetRequest = service.Files.Get(FolderIDTextBox.Text)
                Dim FolderNameMetadata As Data.File = GetFolderName.Execute
                FolderNameTextbox.Text = FolderNameMetadata.Name
                Return True
            Catch ex As Exception
                If ShowMessage = True Then MsgBox(Translations.MsgAndDialogLang("folder_id_incorrect"))
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
            OpenInBrowser(True)
        Else
            If viewing_trash = False Then
                If FolderListBox.SelectedItem IsNot Nothing Then EnterFolder(FolderIdsList.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
            End If
        End If
    End Sub
    Private Sub GoBack()
        If viewing_trash = False Then
            If CurrentFolder = "root" = False Then
                If PreviousFolderId.Count > 0 Then
                    Dim PreviousFolderIdBeforeRemoving As String = PreviousFolderId.Item(PreviousFolderId.Count - 1)
                    PreviousFolderId.RemoveAt(PreviousFolderId.Count - 1)
                    My.Settings.PreviousFolderIDs.RemoveAt(My.Settings.PreviousFolderIDs.Count - 1)
                    CurrentFolder = PreviousFolderIdBeforeRemoving
                    My.Settings.LastFolder = CurrentFolder
                    My.Settings.Save()
                    CurrentFolderLabel.Text = GetCurrentFolderIDName()
                    EnterFolder(PreviousFolderIdBeforeRemoving, True)
                End If
            End If
        Else
            RefreshFileList("trash")
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles BackButton.Click
        GoBack()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FilesListBox.SelectedIndexChanged
        If FilesListBox.SelectedItem IsNot Nothing Then
            FileNameTextBox.Text = FilesListBox.SelectedItem.ToString()
            FileIDTextbox.Text = FileIdsList.Item(FilesListBox.SelectedIndex)
            DateCreatedTextbox.Text = FileCreatedTimeList.Item(FilesListBox.SelectedIndex).ToString
            DateModifiedTextbox.Text = FileModifiedTimeList.Item(FilesListBox.SelectedIndex).ToString
            MD5ChecksumTextbox.Text = FileMD5List.Item(FilesListBox.SelectedIndex)
            MIMETypeTextbox.Text = FileMIMEList.Item(FilesListBox.SelectedIndex)
            FileSizeTextbox.Text = String.Format("{0:N2} MB", FileSizeList.Item(FilesListBox.SelectedIndex) / 1024 / 1024)
        Else
            FileNameTextBox.Text = ""
            FileIDTextbox.Text = ""
            MIMETypeTextbox.Text = ""
            DateCreatedTextbox.Text = ""
            DateModifiedTextbox.Text = ""
            MD5ChecksumTextbox.Text = ""
            FileSizeTextbox.Text = ""
        End If
        If FilesListBox.SelectedItems.Count > 1 Then
            SaveSelectedFilesChecksumButton.Visible = True
        Else
            SaveSelectedFilesChecksumButton.Visible = False
        End If
    End Sub


    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles CreateNewFolderButton.Click
        CreateFolder()
    End Sub
    Private Sub CreateFolder()
        Dim FolderNameToCreate As String
        Dim Message, Title As String
        Message = Translations.MsgAndDialogLang("enter_name_for_folder")
        Title = Translations.MsgAndDialogLang("create_new_folder")
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
            If FolderNameTextbox.Text = "root" Then
                PreviousFolderId.Add("root")
            Else
                PreviousFolderId.Add(CurrentFolder)
            End If
            FolderNameTextbox.Text = FolderNameToCreate
            FolderIDTextBox.Text = FolderID.Id
            CurrentFolder = FolderID.Id
            CurrentFolderLabel.Text = GetCurrentFolderIDName()
            RefreshFileList(FolderID.Id)
        End If
    End Sub
    Private Sub RenameFileOrFolder(FileOrFolderToRename As String)
        Dim NewName As String
        Dim Message, Title As String
        Message = Translations.MsgAndDialogLang("enter_new_name")
        Title = Translations.MsgAndDialogLang("rename_dialog")
        NewName = InputBox(Message, Title, GetIDName(FileOrFolderToRename))
        If String.IsNullOrEmpty(NewName) = False Then
            Dim FileMetadata As New Data.File With {.Name = NewName}
            service.Files.Update(FileMetadata, FileOrFolderToRename).ExecuteAsync()
            Thread.Sleep(500)
            RefreshFileList(CurrentFolder)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles SaveChecksumFileButton.Click
        If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksum.md5") Else SaveChecksumsFile(FilesListBox.SelectedItem.ToString & ".md5")
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles SaveSelectedFilesChecksumButton.Click
        If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksums.md5") Else SaveChecksumsFile(GetCurrentFolderIDName() & ".md5")
    End Sub
    Private Function SaveChecksumFileDialog(FileOrFolderName As String) As String
        SaveFileDialog2.Title = Translations.MsgAndDialogLang("checksum_location")
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

    Private Function GetChecksumsReturnChar() As String
        If My.Settings.EncodeChecksumsFormat = 0 Then
            Return vbCrLf
        ElseIf My.Settings.EncodeChecksumsFormat = 1 Then
            Return vbCr
        ElseIf My.Settings.EncodeChecksumsFormat = 2 Then
            Return vbLf
        Else
            Return vbCrLf
        End If
    End Function
    Private Function GetSlashChar() As String
        If My.Settings.EncodeChecksumsFormat = 0 Then
            Return "\"
        ElseIf My.Settings.EncodeChecksumsFormat = 1 Or My.Settings.EncodeChecksumsFormat = 2 Then
            Return "/"
        Else
            Return "/"
        End If
    End Function
    Private Function SaveFileChecksums() As String
        Dim ChecksumString As String = String.Empty
        For Each item As String In FilesListBox.SelectedItems
            ChecksumString = ChecksumString + FileMD5List.Item(FilesListBox.Items.IndexOf(item)) + " *" + item + GetChecksumsReturnChar()
        Next
        Return ChecksumString
    End Function
    Private Sub WorkWithTrash(Items As ListBox.SelectedObjectCollection, Optional IsFile As Boolean = False, Optional TrashItem As Boolean = False)
        Dim ConfirmMessage As String = String.Empty
        Dim SuccessMessage As String = String.Empty
        If TrashItem Then
            If IsFile Then
                If Items.Count > 1 Then
                    ConfirmMessage = Translations.MsgAndDialogLang("move_selected_file2trash")
                    SuccessMessage = Translations.MsgAndDialogLang("files_moved2trash")
                Else
                    ConfirmMessage = Translations.MsgAndDialogLang("move_file2trash_part1") & FilesListBox.SelectedItem.ToString & Translations.MsgAndDialogLang("move_file2trash_part2")
                    SuccessMessage = Translations.MsgAndDialogLang("file_moved2trash")
                End If
            Else
                If Items.Count > 1 Then
                    ConfirmMessage = Translations.MsgAndDialogLang("confirm_move_selected_folder2trash")
                    SuccessMessage = Translations.MsgAndDialogLang("folders_moved2trash")
                Else
                    ConfirmMessage = Translations.MsgAndDialogLang("confirm_move_folder2trash_part1") & FolderListBox.SelectedItem.ToString & Translations.MsgAndDialogLang("confirm_move_folder2trash_part2")
                    SuccessMessage = Translations.MsgAndDialogLang("folder_moved2trash")
                End If
            End If
        Else
            If IsFile Then
                If Items.Count > 1 Then
                    ConfirmMessage = Translations.MsgAndDialogLang("confirm_restore_selected_files")
                    SuccessMessage = Translations.MsgAndDialogLang("files_restored")
                Else
                    ConfirmMessage = Translations.MsgAndDialogLang("restore_file_part1") & FilesListBox.SelectedItem.ToString & Translations.MsgAndDialogLang("restore_file_part2")
                    SuccessMessage = Translations.MsgAndDialogLang("file_restored")
                End If
            Else
                If Items.Count > 1 Then
                    ConfirmMessage = Translations.MsgAndDialogLang("confirm_restore_selected_folders")
                    SuccessMessage = Translations.MsgAndDialogLang("folders_restored")
                Else
                    ConfirmMessage = Translations.MsgAndDialogLang("restore_folder_part1") & FolderListBox.SelectedItem.ToString & Translations.MsgAndDialogLang("restore_folder_part2")
                    SuccessMessage = Translations.MsgAndDialogLang("folder_restored")
                End If
            End If
        End If

        If MsgBox(ConfirmMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Dim FileMetadata As New Data.File With {.Trashed = TrashItem}
            For Each item In Items
                If IsFile Then
                    service.Files.Update(FileMetadata, FileIdsList.Item(FilesListBox.Items.IndexOf(item))).ExecuteAsync()
                Else
                    service.Files.Update(FileMetadata, FolderIdsList.Item(FolderListBox.Items.IndexOf(item))).ExecuteAsync()
                End If
            Next
            Thread.Sleep(1000)
            RefreshFileList(CurrentFolder)
            MsgBox(SuccessMessage)
        End If
    End Sub
    Private Sub FolderListBox_KeyDown(sender As Object, e As KeyEventArgs) Handles FolderListBox.KeyDown
        If e.Modifiers = Keys.Control Then
            controlPressed = True
        End If
        If e.KeyCode = Keys.Delete Then
            If viewing_trash = False Then
                If FolderListBox.SelectedItem IsNot Nothing Then
                    WorkWithTrash(FolderListBox.SelectedItems, False, True)
                End If
            End If
            e.SuppressKeyPress = True
        ElseIf e.KeyCode = Keys.Enter Then
            If FolderListBox.SelectedItem IsNot Nothing Then EnterFolder(FolderIdsList.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
            e.SuppressKeyPress = True
        ElseIf e.KeyCode = Keys.F5 Then
            If viewing_trash = False Then RefreshFileList(CurrentFolder) Else RefreshFileList("trash")
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.R Then
            If FolderListBox.SelectedItem IsNot Nothing Then
                If viewing_trash Then
                    WorkWithTrash(FolderListBox.SelectedItems, False, False)
                Else
                    RenameFileOrFolder(FolderIdsList.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)).ToString)
                End If
            End If
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.A Then
            For i = 0 To FolderListBox.Items.Count - 1
                FolderListBox.SetSelected(i, True)
            Next
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            If FolderListBox.SelectedItem IsNot Nothing Then EnterFolder(FolderIdsList.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
            If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksums.md5", True) Else SaveChecksumsFile(GetCurrentFolderIDName() & ".md5", True)
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.D Then
            CheckForFolderDownload()
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.M Then
            MoveFileOrFolder(True)
            e.SuppressKeyPress = True
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
            MsgBox(Translations.MsgAndDialogLang("downloads_finished"))
        End If
    End Sub
    Private Async Function DownloadFiles(Folder As String) As Task
        Dim FileList As New List(Of String)
        For Each item As String In FilesListBox.SelectedItems
            FileList.Add(item)
        Next
        For Each item As String In FileList
            FilesListBox.ClearSelected()
            FilesListBox.SelectedItem = item
            Await DownloadFile(Folder & "\" & item, FileIdsList.Item(FilesListBox.Items.IndexOf(item)), FileSizeList.Item(FilesListBox.Items.IndexOf(item)), FileModifiedTimeList.Item(FilesListBox.Items.IndexOf(item)))
        Next
    End Function
    Private Sub SaveChecksumsFile(Filename As String, Optional IsFolder As Boolean = False)
        Dim FolderList As New List(Of String)
        If IsFolder Then
            FolderList.Add(CurrentFolder)
        End If
        Filename = SaveChecksumFileDialog(Filename)
        If Filename IsNot Nothing Then
            Dim ChecksumString As String = String.Empty
            If IsFolder Then
                ChecksumString = GetFileFolderChecksum(FolderList, ChecksumString)
                GoBack()
            Else
                ChecksumString = SaveFileChecksums()
            End If
            If ChecksumString.EndsWith(GetChecksumsReturnChar) Then ChecksumString = ChecksumString.Remove(ChecksumString.LastIndexOf(GetChecksumsReturnChar))
            My.Computer.FileSystem.WriteAllText(Filename, ChecksumString, False, New Text.UTF8Encoding(False))
            MsgBox(Translations.MsgAndDialogLang("checksums_saved"))
        End If
    End Sub
    Private Function GetFileFolderChecksum(Path As List(Of String), ChecksumString As String) As String
        'This creates the full path of the file by getting the ID Name.
        Dim FullPath As String = ""
        If Path.Count > 0 Then
            For Each item In Path
                Try
                    Dim GetFolderName As FilesResource.GetRequest = service.Files.Get(item)
                    Dim FolderNameMetadata As Data.File = GetFolderName.Execute
                    FullPath = FullPath + FolderNameMetadata.Name + GetSlashChar()
                Catch ex As Exception

                End Try
            Next
        End If
        'Once Full Path has been created, we check for files inside the folder. If there's files, we will store their MD5 checksum.
        For Each item As String In FilesListBox.Items
            ChecksumString = ChecksumString + FileMD5List.Item(FilesListBox.Items.IndexOf(item)) + " *" + FullPath + item + GetChecksumsReturnChar()
        Next
        'Finally, this loop checks if there are folders inside the folder we are. We start a recursion loop by calling this same function for each folder inside the folder.
        If FolderListBox.Items.Count > 0 Then
            Dim FolderList As New List(Of String)
            For Each FolderInList As String In FolderListBox.Items
                FolderList.Add(FolderIdsList.Item(FolderListBox.Items.IndexOf(FolderInList)))
            Next
            For Each Folder2 As String In FolderList
                Path.Add(FolderIdsList.Item(FolderIdsList.IndexOf(Folder2)))
                FolderListBox.ClearSelected()
                FolderListBox.SelectedItem = FolderListBox.Items.Item(FolderIdsList.IndexOf(Folder2))
                EnterFolder(FolderIdsList.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
                ChecksumString = GetFileFolderChecksum(Path, ChecksumString)
                GoBack()
                Path.Remove(Folder2)
            Next
        End If
        Return ChecksumString
    End Function

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
        For Each item As String In FilesListBox.Items
            FolderFiles.Add(item)
        Next
        For Each item In FolderFiles
            FilesListBox.ClearSelected()
            FilesListBox.SelectedItem = item
            Await DownloadFile(Location & "\" & FullPath & item, FileIdsList.Item(FilesListBox.Items.IndexOf(item)), FileSizeList.Item(FilesListBox.Items.IndexOf(item)), FileModifiedTimeList.Item(FilesListBox.Items.IndexOf(item)))
        Next
        'Finally, this loop checks if there are folders inside the folder we are. We start a recursion loop by calling this same function for each folder inside the folder.
        If FolderListBox.Items.Count > 0 Then
            Dim FolderList As New List(Of String)
            For Each FolderInList As String In FolderListBox.Items
                FolderList.Add(FolderIdsList.Item(FolderListBox.Items.IndexOf(FolderInList)))
            Next
            For Each Folder2 As String In FolderList
                Path.Add(FolderIdsList.Item(FolderIdsList.IndexOf(Folder2)))
                FolderListBox.SelectedItem = FolderListBox.Items.Item(FolderIdsList.IndexOf(Folder2))
                EnterFolder(FolderIdsList.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
                Await DownloadFolder(Path, Location)
                GoBack()
                Path.Remove(Folder2)
            Next
        End If
    End Function
    Private Sub EnterFolder(FolderID As String, Optional GoingBack As Boolean = False)
        If Not GoingBack Then
            PreviousFolderId.Add(CurrentFolder)
            My.Settings.PreviousFolderIDs.Add(CurrentFolder)
        End If
        CurrentFolder = FolderID
        My.Settings.LastFolder = CurrentFolder
        My.Settings.Save()
        SaveSelectedFilesChecksumButton.Visible = False
        BackButton.Enabled = True
        CurrentFolderLabel.Text = GetCurrentFolderIDName()
        RefreshFileList(FolderID)
    End Sub
    Private controlPressed As Boolean = False
    Private Sub FilesListBox_KeyDown(sender As Object, e As KeyEventArgs) Handles FilesListBox.KeyDown
        If e.Modifiers = Keys.Control Then
            controlPressed = True
        End If
        If e.KeyCode = Keys.Delete Then
            If viewing_trash = False Then
                If FilesListBox.SelectedItem IsNot Nothing Then
                    WorkWithTrash(FilesListBox.SelectedItems, True, True)
                End If
            End If
            e.SuppressKeyPress = True
        ElseIf e.KeyCode = Keys.F5 Then
            If viewing_trash = False Then RefreshFileList(CurrentFolder) Else RefreshFileList("trash")
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.R Then
            If FilesListBox.SelectedItem IsNot Nothing Then
                If viewing_trash Then
                    WorkWithTrash(FilesListBox.SelectedItems, True, False)
                Else
                    RenameFileOrFolder(FileIdsList.Item(FilesListBox.Items.IndexOf(FilesListBox.SelectedItem)))
                End If
            End If
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.A Then
            For i = 0 To FilesListBox.Items.Count - 1
                FilesListBox.SetSelected(i, True)
            Next
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            CheckFilesToSaveChecksums()
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.D Then
            CheckForFilesDownload()
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.M Then
            MoveFileOrFolder()
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.U Then
            CheckFilesToGetRAWUrl()
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.V Then
            verifyHash1()
            e.SuppressKeyPress = True
        End If
        Return
    End Sub

    Private Sub CheckFilesToSaveChecksums()
        If FilesListBox.SelectedIndex <> -1 Then
            If FilesListBox.SelectedItems.Count > 1 Then
                If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksums.md5") Else SaveChecksumsFile(GetCurrentFolderIDName() & ".md5")
            Else
                If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksum.md5") Else SaveChecksumsFile(FilesListBox.SelectedItem.ToString & ".md5")
            End If
        End If
    End Sub

    Private Sub BtnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim credPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
        credPath = Path.Combine(credPath, ".credentials\GoogleDriveUploaderTool.json")
        Dim credfiles As String() = Directory.GetFiles(credPath, "*.TokenResponse-user")
        For Each credfile In credfiles
            Debug.WriteLine(credfile)
            File.Delete(credfile)
        Next
        MsgBox(Translations.MsgAndDialogLang("logged_out"))
        Application.Exit()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles ViewTrashButton.Click
        If viewing_trash = False Then
            viewing_trash = True
            GoToRootLink.Visible = False
            CreateNewFolderButton.Enabled = False
            RestoreToolStripMenuItem.Enabled = True
            MoveToTrashToolStripMenuItem.Enabled = False
            Lang_Select()
            RefreshFileList("trash")
        Else
            viewing_trash = False
            GoToRootLink.Visible = True
            CreateNewFolderButton.Enabled = True
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
            FolderIDTextBox.Text = FolderToUploadFileList.Item(UploadsListBox.SelectedIndex)
            GetFolderIDName(False)
            UploadToSelectedFolderButton.Visible = True
            DeselectItemFromUploadQueueButton.Enabled = True
        Else
            UploadToSelectedFolderButton.Visible = False
            DeselectItemFromUploadQueueButton.Enabled = False
        End If

    End Sub



    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles UploadToSelectedFolderButton.Click
        FolderToUploadFileList.Item(UploadsListBox.SelectedIndex) = CurrentFolder
        FolderIDTextBox.Text = CurrentFolder
        GetFolderIDName(False)
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles DeselectItemFromUploadQueueButton.Click
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
        If EnglishRButton.Checked Then
            Process.Start("https://github.com/moisesmcardona/GoogleDriveUploadTool/blob/master/README.md")
        ElseIf SpanishRButton.Checked Then
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
                FolderToUploadFileList.Add(CurrentFolder)
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
            FolderToUploadFileList.Add(CurrentFolder)
            My.Settings.UploadQueue.Add(FolderBrowserDialog1.SelectedPath)
            My.Settings.UploadQueueFolders.Add(CurrentFolder)
            GetDirectoriesAndFiles(New DirectoryInfo(FolderBrowserDialog1.SelectedPath))
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
            EnterFolder(FolderIdsList.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
            DownloadFilesAndFolders(True)
        End If
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
            RenameFileOrFolder(FileIdsList.Item(FilesListBox.Items.IndexOf(FilesListBox.SelectedItem)))
        End If
    End Sub

    Private Sub SelectedFolderToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SelectedFolderToolStripMenuItem1.Click
        If FolderListBox.SelectedItem IsNot Nothing Then
            RenameFileOrFolder(FolderIdsList.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
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
                FolderIDTextBox.Text = FolderIdsList.Item(FolderListBox.SelectedIndex)
                GetFolderIDName(False)
            End If
        End If
    End Sub

    Private Sub FilesListBox_MouseDoubleClick(sender As Object, e As EventArgs) Handles FilesListBox.MouseDoubleClick
        OpenInBrowser()
    End Sub

    Private Sub ChecksumsEncodeFormatComboBox_DropDownClosed(sender As Object, e As EventArgs) Handles ChecksumsEncodeFormatComboBox.DropDownClosed
        My.Settings.EncodeChecksumsFormat = ChecksumsEncodeFormatComboBox.SelectedIndex
        My.Settings.Save()
    End Sub

    Private Async Function GetUrl(ID As String) As Task(Of String)
        Dim request As WebRequest = WebRequest.Create("https://www.googleapis.com/drive/v3/files/" + ID + "?alt=media&access_token=" + Await credential.GetAccessTokenForRequestAsync())
        Dim response As WebResponse = request.GetResponse
        Return response.ResponseUri.OriginalString
    End Function

    Private Sub FilesListBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles FilesListBox.KeyPress
        If controlPressed Then
            controlPressed = False
            e.Handled = True
        End If
    End Sub

    Private Sub FolderListBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles FolderListBox.KeyPress
        If controlPressed Then
            controlPressed = False
            e.Handled = True
        End If
    End Sub

    Private Sub OpenInBrowserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenInBrowserToolStripMenuItem.Click
        If FilesListBox.SelectedItem IsNot Nothing Then OpenInBrowser() Else Translations.MsgAndDialogLang("no_file_selected")
    End Sub

    Private Sub OpenInBrowser(Optional Folder As Boolean = False)
        If Not Folder Then
            If FilesListBox.SelectedItem IsNot Nothing Then
                Process.Start("https://drive.google.com/file/d/" + FileIdsList.Item(FilesListBox.SelectedIndex) + "/view")
            End If
        Else
            If FolderListBox.SelectedItem IsNot Nothing Then
                Process.Start("https://drive.google.com/drive/folders/" + FolderIdsList.Item(FolderListBox.SelectedIndex))
            End If
        End If
    End Sub
    Private Sub MoveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MoveToolStripMenuItem.Click
        MoveFileOrFolder()
    End Sub

    Private Sub MoveFileOrFolder(Optional IsFolder As Boolean = False)
        MoveDialog.FolderIDs = New List(Of String)
        MoveDialog.CurrentFolder = CurrentFolder
        MoveDialog.PreviousFolderId = PreviousFolderId
        MoveDialog.FolderIDs = FolderIdsList
        If Not IsFolder Then
            If FilesListBox.SelectedItems IsNot Nothing Then
                For Each item As String In FilesListBox.SelectedItems
                    MoveDialog.ItemsToMove.Add(FileIdsList.Item(FilesListBox.Items.IndexOf(item)))
                Next
                MoveDialog.Show()
            Else
                Translations.MsgAndDialogLang("no_files_selected")
            End If
        Else
            If FolderListBox.SelectedItems IsNot Nothing Then
                For Each item As String In FolderListBox.SelectedItems
                    MoveDialog.ItemsToMove.Add(FolderIdsList.Item(FolderListBox.Items.IndexOf(item)))
                Next
                MoveDialog.Show()
            Else
                Translations.MsgAndDialogLang("no_folders_selected")
            End If
        End If
    End Sub

    Private Sub DownloadToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DownloadToolStripMenuItem1.Click
        CheckForFilesDownload()
    End Sub

    Private Sub SaveChecksumToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveChecksumToolStripMenuItem.Click
        CheckFilesToSaveChecksums()
    End Sub

    Private Async Sub CheckFilesToGetRAWUrl()
        If FilesListBox.SelectedItems.Count > 0 Then
            Dim URLs As List(Of String) = New List(Of String)
            Download_URLs.Filenames = New List(Of String)
            Download_URLs.Checksums = New List(Of String)
            For Each item As String In FilesListBox.SelectedItems
                URLs.Add(Await GetUrl(FileIdsList.Item(FilesListBox.Items.IndexOf(item))))
                Download_URLs.Filenames.Add(FilesListBox.Items.Item(FilesListBox.Items.IndexOf(item)).ToString)
                Download_URLs.Checksums.Add(FileMD5List.Item(FilesListBox.Items.IndexOf(item)))
            Next
            Download_URLs.RichTextBox1.Clear()
            For Each item In URLs
                Download_URLs.RichTextBox1.Text += item
            Next
            Download_URLs.URLs = URLs
            Download_URLs.ShowDialog()
        Else
            MsgBox(Translations.MsgAndDialogLang("no_files_selected"))
        End If
    End Sub
    Private Sub GetRawDownloadURLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GetRawDownloadURLToolStripMenuItem.Click
        CheckFilesToGetRAWUrl()
    End Sub

    Private Sub OpenInBrowserToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles OpenInBrowserToolStripMenuItem1.Click
        OpenInBrowser(True)
    End Sub

    Private Sub DownloadToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles DownloadToolStripMenuItem2.Click
        CheckForFolderDownload()
    End Sub

    Private Sub MoveToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles MoveToolStripMenuItem1.Click
        MoveFileOrFolder(True)
    End Sub

    Private Sub SaveChecksumsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SaveChecksumsToolStripMenuItem1.Click
        If FolderListBox.SelectedItem IsNot Nothing Then EnterFolder(FolderIdsList.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
        If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksums.md5", True) Else SaveChecksumsFile(GetCurrentFolderIDName() & ".md5", True)
    End Sub

    Private Sub UploadChunkToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UploadChunkToolStripMenuItem.Click
        UploadChunkSize.file = "chunkmultiplier.txt"
        UploadChunkSize.ShowDialog()
    End Sub

    Private Sub RAMChunkToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RAMChunkToolStripMenuItem.Click
        UploadChunkSize.file = "rammultiplier.txt"
        UploadChunkSize.ShowDialog()
    End Sub

    Private Sub RenameToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RenameToolStripMenuItem1.Click
        If FilesListBox.SelectedItem IsNot Nothing Then
            RenameFileOrFolder(FileIdsList.Item(FilesListBox.Items.IndexOf(FilesListBox.SelectedItem)))
        End If
    End Sub

    Private Sub RenameToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles RenameToolStripMenuItem2.Click
        If FolderListBox.SelectedItem IsNot Nothing Then
            RenameFileOrFolder(FolderIdsList.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
        End If
    End Sub
    Private Sub verifyHash1()
        Dim Index As Integer = FilesListBox.SelectedIndex
        Dim FileBrowser As New OpenFileDialog With {
            .Title = "Browse for a file to compare the checksum",
            .Filter = IO.Path.GetFileNameWithoutExtension(FilesListBox.Items.Item(Index).ToString()) + "|*" + IO.Path.GetFileNameWithoutExtension(FilesListBox.Items.Item(Index).ToString()),
            .FileName = FilesListBox.Items.Item(Index).ToString()}
        Dim result As DialogResult = FileBrowser.ShowDialog()
        If result = DialogResult.OK Then
            Dim BrowserFileName As String = FileBrowser.FileName
            Dim thread As New Thread(Sub() verifyHash(Index, BrowserFileName))
            thread.Start()
        End If
    End Sub
    Private Sub verifyHash(Index As Integer, filename As String)
        ProgressBar1.BeginInvoke(Sub() ProgressBar1.Style = ProgressBarStyle.Marquee)
        StatusLabel.BeginInvoke(Sub() StatusLabel.Text = "Verifying...")
        Dim stream As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim MD5Hash As System.Security.Cryptography.MD5 = System.Security.Cryptography.MD5.Create
        Dim Hash As String = ""
        MD5Hash.ComputeHash(stream)
        stream.Close()
        For Each b In MD5Hash.Hash
            Hash += b.ToString("x2")
        Next
        ProgressBar1.BeginInvoke(Sub()
                                     ProgressBar1.Style = ProgressBarStyle.Blocks
                                     ProgressBar1.Minimum = 0
                                     ProgressBar1.Maximum = 100
                                     ProgressBar1.Value = 100
                                 End Sub)
        StatusLabel.BeginInvoke(Sub() StatusLabel.Text = "Done")
        Dim ExpectedHash As String = FileMD5List.Item(Index).ToString()
        If Hash = ExpectedHash Then
            MsgBox("Hash match for file " + filename + "." + Environment.NewLine + Environment.NewLine + "Computed hash: " + Hash + Environment.NewLine + " Expected Hash: " + ExpectedHash, MsgBoxStyle.Information)
        Else
            MsgBox("Hash does not match for file " + filename + "." + Environment.NewLine + Environment.NewLine + "Computed hash: " + Hash + Environment.NewLine + " + Expected Hash: " + ExpectedHash, MsgBoxStyle.Critical)
        End If
    End Sub

    Private Sub VerifyChecksumToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VerifyChecksumToolStripMenuItem.Click
        verifyHash1()
    End Sub
End Class
