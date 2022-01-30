﻿Imports Google.Apis.Drive.v3
Imports System.IO
Imports System.Threading
Imports Google.Apis.Upload
Imports Google.Apis.Download
Imports System.Collections.Specialized
Imports System.Net
Imports System.Runtime.InteropServices

Public Class Form1
    Private FolderToUploadOrDownloadIdFileList As New List(Of String)
    Private ItemInQueueAction As New List(Of Integer)
    Public viewing_trash As Boolean = False
    Shared SoftwareName As String = "Google Drive Uploader Tool"
    Public pageToken As String = ""
    Public Shared drive As GoogleDriveClass
    Private UploadCancellationToken As CancellationTokenSource
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load         'Initialize Upload Queue Collection
        BackButton.Enabled = False
        If My.Settings.UploadQueue Is Nothing Then
            My.Settings.UploadQueue = New StringCollection
        End If
        If My.Settings.UploadQueueFolders Is Nothing Then
            My.Settings.UploadQueueFolders = New StringCollection
        End If
        If My.Settings.QueueFileAction Is Nothing Then
            My.Settings.QueueFileAction = New StringCollection
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
        'Google Drive initialization
        drive = New GoogleDriveClass(SoftwareName)
        If Not drive.connected Then
            MsgBox(Translations.MsgAndDialogLang("client_secrets_not_found"))
            Process.Start("https://developers.google.com/drive/v3/web/quickstart/dotnet")
            Me.Close()
        Else
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
                    FolderToUploadOrDownloadIdFileList.Add(item)
                Next
            End If
            If My.Settings.QueueFileAction.Count > 0 Then
                For Each item In My.Settings.QueueFileAction
                    ItemInQueueAction.Add(Convert.ToInt32(item))
                Next
            End If
            'Loads the last used Folder ID and lists files
            If My.Settings.PreviousFolderIDs.Count > 0 Then
                drive.currentFolder = My.Settings.LastFolder
            Else
                drive.currentFolder = "root"
            End If
            For Each item In My.Settings.PreviousFolderIDs
                drive.previousFolder.Add(item)
            Next
            SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Checked = My.Settings.SaveAsChecksumsMD5
            StartUploadsAutomaticallyToolStripMenuItem.Checked = My.Settings.AutomaticUploads
            PreserveFileModifiedDateToolStripMenuItem.Checked = My.Settings.PreserveModifiedDate
            UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Checked = My.Settings.UpdateViews
            ChecksumsEncodeFormatComboBox.SelectedIndex = My.Settings.EncodeChecksumsFormat
            OrderByComboBox.SelectedIndex = My.Settings.SortByIndex
            DescendingOrderToolStripMenuItem.Checked = My.Settings.OrderDesc
            CopyFileToRAMBeforeUploadingToolStripMenuItem.Checked = My.Settings.CopyToRAM
            Try
                EnterFolder(drive.currentFolder, True)
            Catch
                GoToRoot()
            End Try
            CurrentFolderLabel.Text = drive.currentFolderName
            If UploadsListBox.Items.Count > 0 Then
                UploadsListBox.SelectedIndex = 0
            Else
                FolderIDTextBox.Text = drive.currentFolder
            End If
        End If
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
    Private DownloadStopped As Boolean = False
    Private Uploading As Boolean = False
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles UploadButton.Click
        CheckBeforeStartingUpload()
    End Sub
    Private ResumeFromError As Boolean = False
    Private Sub CheckBeforeStartingUpload()
        If Uploading Then
            UploadCancellationToken.Cancel()
            Uploading = False
            UploadButton.Text = Translations.MsgAndDialogLang("uploadbtn_start")
        Else
            If UploadsListBox.Items.Count > 0 Then
                FolderIDTextBox.Text = FolderToUploadOrDownloadIdFileList.Item(0)
                If drive.GetFolderName(FolderToUploadOrDownloadIdFileList.Item(0)) <> Translations.MsgAndDialogLang("folder_id_incorrect") Then
                    My.Settings.LastFolder = drive.currentFolder
                    My.Settings.Save()
                    ResumeFromError = False
                    Uploading = True
                    UploadButton.Text = Translations.MsgAndDialogLang("uploadbtn_stop")
                    UploadFiles()
                Else
                    If MsgBox(Translations.MsgAndDialogLang("folder_invaild"), MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                        My.Settings.LastFolder = "root"
                        My.Settings.Save()
                        ResumeFromError = False
                        Uploading = True
                        UploadButton.Text = Translations.MsgAndDialogLang("uploadbtn_stop")
                        UploadFiles()
                    End If
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
            UploadCancellationToken = New CancellationTokenSource
            GetFile = UploadsListBox.Items(0).ToString
            Try
                If ItemInQueueAction.Item(0) = 1 Then
                    If Not My.Computer.FileSystem.DirectoryExists(Path.GetDirectoryName(GetFile)) Then Directory.CreateDirectory(Path.GetDirectoryName(GetFile))
                    Await DownloadFile(GetFile, FolderToUploadOrDownloadIdFileList.Item(0), Nothing, Nothing, UploadCancellationToken)
                    If DownloadStopped = True Then
                        Exit Sub
                    End If
                Else
                    FolderIDTextBox.Text = FolderToUploadOrDownloadIdFileList.Item(0)
                    drive.GetFolderName(FolderToUploadOrDownloadIdFileList.Item(0))
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
                                UploadFile = drive.service.Files.Create(FileMetadata, FileInRAM, "")
                            Else
                                UploadFile = drive.service.Files.Create(FileMetadata, UploadStream, "")
                            End If
                        Else
                            UploadFile = drive.service.Files.Create(FileMetadata, UploadStream, "")
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

                        Dim uploadUri As Uri = Nothing
                        starttime = Date.Now
                        If ResumeFromError = False Then
                            uploadUri = GetSessionRestartUri(True)
                        Else
                            uploadUri = GetSessionRestartUri(False)
                        End If
                        If uploadUri = Nothing Then
                            Await UploadFile.UploadAsync(UploadCancellationToken.Token)
                        Else
                            Await UploadFile.ResumeAsync(uploadUri, UploadCancellationToken.Token)
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
                        Dim CreateFolder As FilesResource.CreateRequest = drive.service.Files.Create(New Data.File With {
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
                End If
            Catch ex As Exception
                If UploadCancellationToken.IsCancellationRequested Then
                    Exit Sub
                End If
                UploadFailed = True
            End Try
            If UploadFailed = False Then
                If UploadsListBox.Items.Count > 0 Then
                    If GetFile = UploadsListBox.Items(0).ToString Then
                        UploadsListBox.Items.RemoveAt(0)
                        FolderToUploadOrDownloadIdFileList.RemoveAt(0)
                        ItemInQueueAction.RemoveAt(0)
                        My.Settings.UploadQueue.RemoveAt(0)
                        My.Settings.UploadQueueFolders.RemoveAt(0)
                        My.Settings.QueueFileAction.RemoveAt(0)
                        My.Settings.Save()
                    End If
                End If
                ResumeFromError = False
                If UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Checked Then EnterFolder(drive.currentFolder, True)
                UpdateQuota()
            End If
        End While
        Uploading = False
        UploadButton.Text = Translations.MsgAndDialogLang("uploadbtn_start")
        FolderCreated = False
        My.Settings.FolderCreated = False
        DirectoryListID.Clear()
        DirectoryList.Clear()
        My.Settings.UploadQueue.Clear()
        My.Settings.UploadQueueFolders.Clear()
        My.Settings.FoldersCreated.Clear()
        My.Settings.FoldersCreatedID.Clear()
        My.Settings.Save()
        MsgBox(Translations.MsgAndDialogLang("queue_finish"))
    End Sub
    Private ErrorMessage As String = ""

    Private Sub Upload_ProgressChanged(uploadStatusInfo As IUploadProgress)
        If UploadCancellationToken.IsCancellationRequested Then
            UpdateBytesSent(uploadStatusInfo.BytesSent, Translations.MsgAndDialogLang("uploadstatus_stopped"), starttime)
            UploadFailed = True
        Else
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
        End If
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
        Dim UserDataRequest As New AboutResource.GetRequest(drive.service) With {.Fields = "user,storageQuota"}
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
            Await DownloadFile(SaveFileDialog1.FileName, drive.FileListID.Item(FilesListBox.SelectedIndex), drive.FileSizeList(FilesListBox.SelectedIndex), drive.FileModifiedTimeList(FilesListBox.SelectedIndex), New CancellationTokenSource)
        End If
    End Sub
    Private Async Function DownloadFile(Location As String, FileName As String, FileSize As Long?, ModifiedTime As Date?, token As CancellationTokenSource) As Task
        If UploadCancellationToken Is Nothing Then
            UploadCancellationToken = New CancellationTokenSource
        End If
        Dim FileToSave As FileStream = New FileStream(Location, FileMode.Create, FileAccess.Write)
        Try
            starttime = Date.Now
            If FileSize Is Nothing Then
                Dim FileMetadata As Data.File = drive.GetFileMetadata(FileName)
                FileSize = FileMetadata.Size
                ModifiedTime = FileMetadata.ModifiedTime
            End If
            FileSizeFromCurrentUploadLabel.Text = String.Format("{0:N2} MB", FileSize / 1024 / 1024)
            ProgressBar1.Maximum = CInt(FileSize / 1024 / 1024)
            MaxFileSize = Convert.ToDouble(FileSize)
            Dim DownloadRequest As FilesResource.GetRequest = drive.service.Files.Get(FileName)
            AddHandler DownloadRequest.MediaDownloader.ProgressChanged, New Action(Of IDownloadProgress)(AddressOf Download_ProgressChanged)
            Await DownloadRequest.DownloadAsync(FileToSave, token.Token)
            FileToSave.Close()
            File.SetLastWriteTime(Location, ModifiedTime.Value)
        Catch ex As Exception
            FileToSave.Close()
        End Try
    End Function
    Private Sub Download_ProgressChanged(progress As IDownloadProgress)
        If UploadCancellationToken.IsCancellationRequested Then
            UpdateBytesSent(0, Translations.MsgAndDialogLang("uploadstatus_stopped"), starttime)
            DownloadStopped = True
        Else
            Select Case progress.Status
                Case DownloadStatus.Completed
                    UpdateBytesSent(Convert.ToInt64(MaxFileSize), Translations.MsgAndDialogLang("uploadstatus_complete"), starttime)
                    DownloadStopped = False
                Case DownloadStatus.Downloading
                    UpdateBytesSent(progress.BytesDownloaded, Translations.MsgAndDialogLang("uploadstatus_downloading"), starttime)
                    DownloadStopped = False
                Case DownloadStatus.Failed
                    UpdateBytesSent(progress.BytesDownloaded, Translations.MsgAndDialogLang("uploadstatus_failed"), starttime)
                    DownloadStopped = False
            End Select
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles RefreshListButton.Click
        If viewing_trash = False Then EnterFolder(drive.currentFolder, True) Else EnterFolder("trash", True)
    End Sub
    Private Delegate Sub RefreshFileListInvoker(FolderID As String)
    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim filepath() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())
        For Each path In filepath
            If Directory.Exists(path) Then
                UploadsListBox.Items.Add(path)
                ItemInQueueAction.Add(0)
                FolderToUploadOrDownloadIdFileList.Add(drive.currentFolder)
                My.Settings.UploadQueue.Add(path)
                My.Settings.UploadQueueFolders.Add(drive.currentFolder)
                My.Settings.QueueFileAction.Add("0")
                GetDirectoriesAndFiles(New DirectoryInfo(path))
            Else
                UploadsListBox.Items.Add(path)
                ItemInQueueAction.Add(0)
                FolderToUploadOrDownloadIdFileList.Add(drive.currentFolder)
                My.Settings.UploadQueue.Add(path)
                My.Settings.UploadQueueFolders.Add(drive.currentFolder)
                My.Settings.QueueFileAction.Add("0")
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
            FolderToUploadOrDownloadIdFileList.Add(drive.currentFolder)
            ItemInQueueAction.Add(0)
            My.Settings.UploadQueueFolders.Add(drive.currentFolder)
            My.Settings.QueueFileAction.Add("0")
        Next
        For Each subF As DirectoryInfo In BaseFolder.GetDirectories()
            Application.DoEvents()
            UploadsListBox.Items.Add(subF.FullName)
            FolderToUploadOrDownloadIdFileList.Add(drive.currentFolder)
            ItemInQueueAction.Add(0)
            My.Settings.UploadQueue.Add(subF.FullName)
            My.Settings.UploadQueueFolders.Add(drive.currentFolder)
            My.Settings.QueueFileAction.Add("0")
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
            UpdateTranslations()
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles SpanishRButton.CheckedChanged
        If SpanishRButton.Checked Then
            Translations.SpanishLanguage()
            My.Settings.Language = "Spanish"
            My.Settings.Save()
            UpdateTranslations()
        End If
    End Sub
    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles TChineseRButton.CheckedChanged
        If TChineseRButton.Checked Then
            Translations.TChineseLanguage()
            My.Settings.Language = "TChinese"
            My.Settings.Save()
            UpdateTranslations()
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles RemoveSelectedFilesFromList.Click
        Do While (UploadsListBox.SelectedItems.Count > 0)
            Dim CurrentIndex = UploadsListBox.SelectedIndex
            UploadsListBox.Items.RemoveAt(CurrentIndex)
            FolderToUploadOrDownloadIdFileList.RemoveAt(CurrentIndex)
            ItemInQueueAction.RemoveAt(CurrentIndex)
            My.Settings.UploadQueue.RemoveAt(CurrentIndex)
            My.Settings.UploadQueueFolders.RemoveAt(CurrentIndex)
            My.Settings.QueueFileAction.RemoveAt(CurrentIndex)
            My.Settings.Save()
        Loop
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles ClearUploadQueueButton.Click
        UploadsListBox.Items.Clear()
        FolderToUploadOrDownloadIdFileList.Clear()
        ItemInQueueAction.Clear()
        My.Settings.UploadQueue.Clear()
        My.Settings.UploadQueueFolders.Clear()
        My.Settings.QueueFileAction.Clear()
        My.Settings.Save()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles GetFolderIdNameButton.Click
        FolderNameTextbox.Text = drive.GetFolderName(FolderIDTextBox.Text)
    End Sub
    Private Function GetIDName(ID As String) As String
        Try
            Dim GetName As FilesResource.GetRequest = drive.service.Files.Get(ID)
            Dim Meta As Data.File = GetName.Execute
            Return Meta.Name
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Sub FolderListBox_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles FolderListBox.MouseDoubleClick
        If My.Computer.Keyboard.ShiftKeyDown Then
            OpenInBrowser(True)
        Else
            If viewing_trash = False Then
                If FolderListBox.SelectedItem IsNot Nothing Then EnterFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
            End If
        End If
    End Sub
    Private Sub GoBack()
        If viewing_trash = False Then
            If drive.currentFolder = "root" = False Then
                EnterFolder("back")
                My.Settings.PreviousFolderIDs.Clear()
                My.Settings.PreviousFolderIDs.AddRange(drive.previousFolder.ToArray())
                My.Settings.LastFolder = drive.currentFolder
                My.Settings.Save()
            End If
        Else
            EnterFolder("trash", True)
        End If
    End Sub

    Private Sub BackButton_Click(sender As Object, e As EventArgs) Handles BackButton.Click
        GoBack()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FilesListBox.SelectedIndexChanged
        If FilesListBox.SelectedItem IsNot Nothing Then
            FileNameTextBox.Text = FilesListBox.SelectedItem.ToString()
            FileIDTextbox.Text = drive.FileListID.Item(FilesListBox.SelectedIndex)
            DateCreatedTextbox.Text = drive.FileCreatedTimeList.Item(FilesListBox.SelectedIndex).ToString
            DateModifiedTextbox.Text = drive.FileModifiedTimeList.Item(FilesListBox.SelectedIndex).ToString
            MD5ChecksumTextbox.Text = drive.FileMD5List.Item(FilesListBox.SelectedIndex)
            MIMETypeTextbox.Text = drive.FileMIMEList.Item(FilesListBox.SelectedIndex)
            FileSizeTextbox.Text = String.Format("{0:N2} MB", drive.FileSizeList.Item(FilesListBox.SelectedIndex) / 1024 / 1024)
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
                drive.currentFolder
            }
            FolderMetadata.Parents = ParentFolder
            FolderMetadata.MimeType = "application/vnd.google-apps.folder"
            Dim CreateFolder As FilesResource.CreateRequest = drive.service.Files.Create(FolderMetadata)
            CreateFolder.Fields = "id"
            Dim FolderID As Data.File = CreateFolder.Execute
            FolderNameTextbox.Text = FolderNameToCreate
            FolderIDTextBox.Text = FolderID.Id
            EnterFolder(FolderID.Id)
            CurrentFolderLabel.Text = drive.currentFolderName
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
            drive.service.Files.Update(FileMetadata, FileOrFolderToRename).ExecuteAsync()
            Thread.Sleep(500)
            EnterFolder(drive.currentFolder, True)
        End If
    End Sub

    Private Sub SaveChecksumFileButton_Click(sender As Object, e As EventArgs) Handles SaveChecksumFileButton.Click
        If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksum.md5") Else SaveChecksumsFile(FilesListBox.SelectedItem.ToString & ".md5")
    End Sub

    Private Sub SaveSelectedFilesChecksumButton_Click(sender As Object, e As EventArgs) Handles SaveSelectedFilesChecksumButton.Click
        If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksums.md5") Else SaveChecksumsFile(drive.currentFolderName & ".md5")
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
            ChecksumString = ChecksumString + drive.FileMD5List.Item(FilesListBox.Items.IndexOf(item)) + " *" + item + GetChecksumsReturnChar()
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
                    drive.service.Files.Update(FileMetadata, drive.FileListID.Item(FilesListBox.Items.IndexOf(item))).ExecuteAsync()
                Else
                    drive.service.Files.Update(FileMetadata, drive.FolderListID.Item(FolderListBox.Items.IndexOf(item))).ExecuteAsync()
                End If
            Next
            Thread.Sleep(1000)
            EnterFolder(drive.currentFolder, True)
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
            If FolderListBox.SelectedItem IsNot Nothing Then EnterFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
            e.SuppressKeyPress = True
        ElseIf e.KeyCode = Keys.F5 Then
            If viewing_trash = False Then EnterFolder(drive.currentFolder, True) Else EnterFolder("trash", True)
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.R Then
            If FolderListBox.SelectedItem IsNot Nothing Then
                If viewing_trash Then
                    WorkWithTrash(FolderListBox.SelectedItems, False, False)
                Else
                    RenameFileOrFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)).ToString)
                End If
            End If
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.A Then
            For i = 0 To FolderListBox.Items.Count - 1
                FolderListBox.SetSelected(i, True)
            Next
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.C Then
            If FolderListBox.SelectedItem IsNot Nothing Then EnterFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
            If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksums.md5", True) Else SaveChecksumsFile(drive.currentFolderName & ".md5", True)
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.D Then
            CheckForFolderDownload()
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.M Then
            MoveFileOrFolder(True)
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.Q Then
            CheckForFolderAddToQueue()
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.U Then
            Initiate_FolderCheckFilesToGetRAWUrl()
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
            FolderList.Add(drive.currentFolder)
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
            Await DownloadFile(Folder & "\" & item, drive.FileListID.Item(FilesListBox.Items.IndexOf(item)), drive.FileSizeList.Item(FilesListBox.Items.IndexOf(item)), drive.FileModifiedTimeList.Item(FilesListBox.Items.IndexOf(item)), New CancellationTokenSource)
        Next
    End Function
    Private Sub SaveChecksumsFile(Filename As String, Optional IsFolder As Boolean = False)
        Dim FolderList As New List(Of String)
        If IsFolder Then
            FolderList.Add(drive.currentFolder)
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
                    FullPath = FullPath + drive.GetFolderName(item).Trim + GetSlashChar()
                Catch ex As Exception

                End Try
            Next
        End If
        'Once Full Path has been created, we check for files inside the folder. If there's files, we will store their MD5 checksum.
        For Each item As String In FilesListBox.Items
            ChecksumString = ChecksumString + drive.FileMD5List.Item(FilesListBox.Items.IndexOf(item)) + " *" + FullPath + item + GetChecksumsReturnChar()
        Next
        'Finally, this loop checks if there are folders inside the folder we are. We start a recursion loop by calling this same function for each folder inside the folder.
        If FolderListBox.Items.Count > 0 Then
            Dim FolderList As New List(Of String)
            For Each FolderInList As String In FolderListBox.Items
                FolderList.Add(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderInList)))
            Next
            For Each Folder2 As String In FolderList
                Path.Add(drive.FolderListID.Item(drive.FolderListID.IndexOf(Folder2)))
                FolderListBox.ClearSelected()
                FolderListBox.SelectedItem = FolderListBox.Items.Item(drive.FolderListID.IndexOf(Folder2))
                EnterFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
                ChecksumString = GetFileFolderChecksum(Path, ChecksumString)
                GoBack()
                Path.Remove(Folder2)
            Next
        End If
        Return ChecksumString
    End Function
    Private Sub AddFolderToDownloadQueue(Path As List(Of String))
        'This creates the full path of the file by getting the ID Name.
        Dim FullPath As String = ""
        Dim count As Integer = 0
        If Path.Count > 0 Then
            For Each item In Path
                Try
                    FullPath = FullPath + drive.GetFolderName(item).Trim + "\"
                Catch ex As Exception

                End Try
                count = count + 1
            Next
            count = 0
        End If
        'Once Full Path has been created, we check for files inside the folder. If there's files, we will add them to the queue
        For Each item As String In FilesListBox.Items
            Dim FileName As String = My.Settings.DownloadLocation + "\" + FullPath & item
            Dim FileId As String = drive.FileListID.Item(FilesListBox.Items.IndexOf(item))
            UploadsListBox.Items.Add(FileName)
            FolderToUploadOrDownloadIdFileList.Add(FileId)
            ItemInQueueAction.Add(1)
            My.Settings.UploadQueue.Add(FileName)
            My.Settings.UploadQueueFolders.Add(FileId)
            My.Settings.QueueFileAction.Add("1")
        Next
        'Finally, this loop checks if there are folders inside the folder we are. We start a recursion loop by calling this same function for each folder inside the folder.
        If FolderListBox.Items.Count > 0 Then
            Dim FolderList As New List(Of String)
            For Each FolderInList As String In FolderListBox.Items
                FolderList.Add(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderInList)))
            Next
            For Each Folder2 As String In FolderList
                Path.Add(drive.FolderListID.Item(drive.FolderListID.IndexOf(Folder2)))
                FolderListBox.ClearSelected()
                FolderListBox.SelectedItem = FolderListBox.Items.Item(drive.FolderListID.IndexOf(Folder2))
                EnterFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
                AddFolderToDownloadQueue(Path)
                Path.Remove(Folder2)
                GoBack()
            Next
        End If
    End Sub
    Private Sub AddFilesToDownloadQueue()
        For Each item As String In FilesListBox.SelectedItems
            Dim FileName As String = My.Settings.DownloadLocation + "\" + drive.currentFolderName.Trim + "\" & item
            Dim FileId As String = drive.FileListID.Item(FilesListBox.Items.IndexOf(item))
            UploadsListBox.Items.Add(FileName)
            FolderToUploadOrDownloadIdFileList.Add(FileId)
            ItemInQueueAction.Add(1)
            My.Settings.UploadQueue.Add(FileName)
            My.Settings.UploadQueueFolders.Add(FileId)
            My.Settings.QueueFileAction.Add("1")
        Next
    End Sub
    Private Async Function DownloadFolder(Path As List(Of String), Location As String) As Task
        'This creates the full path of the file by getting the ID Name.
        Dim FullPath As String = ""
        Dim FolderToCreatePath As String = ""
        Dim count As Integer = 0
        If Path.Count > 0 Then
            For Each item In Path
                Try
                    Dim FolderName As String = drive.GetFolderName(item).Trim
                    If Path.Count = count + 1 Then
                        FolderToCreatePath = FolderToCreatePath + FolderName
                        My.Computer.FileSystem.CreateDirectory(Location + "\" + FolderToCreatePath)
                    Else
                        FolderToCreatePath = FolderToCreatePath + FolderName + "\"
                    End If
                    FullPath = FullPath + FolderName + "\"
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
            Await DownloadFile(Location & "\" & FullPath & item, drive.FileListID.Item(FilesListBox.Items.IndexOf(item)), drive.FileSizeList.Item(FilesListBox.Items.IndexOf(item)), drive.FileModifiedTimeList.Item(FilesListBox.Items.IndexOf(item)), New CancellationTokenSource)
        Next
        'Finally, this loop checks if there are folders inside the folder we are. We start a recursion loop by calling this same function for each folder inside the folder.
        If FolderListBox.Items.Count > 0 Then
            Dim FolderList As New List(Of String)
            For Each FolderInList As String In FolderListBox.Items
                FolderList.Add(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderInList)))
            Next
            For Each Folder2 As String In FolderList
                Path.Add(drive.FolderListID.Item(drive.FolderListID.IndexOf(Folder2)))
                FolderListBox.ClearSelected()
                FolderListBox.SelectedItem = FolderListBox.Items.Item(drive.FolderListID.IndexOf(Folder2))
                EnterFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
                Await DownloadFolder(Path, Location)
                Path.Remove(Folder2)
                GoBack()
            Next
        End If
    End Function

    Public Sub UpdateTranslations()
        Dim FileCountNumber As Integer = drive.FileList.Count
        If FileCountNumber > 1 Then
            FileCount.Text = FileCountNumber.ToString + Translations.MsgAndDialogLang("files_txt")
        ElseIf FileCountNumber = 1 Then
            FileCount.Text = FileCountNumber.ToString + Translations.MsgAndDialogLang("file_txt")
        Else
            FileCount.Text = "0" + Translations.MsgAndDialogLang("files_txt")
        End If
        If Uploading Then
            UploadButton.Text = Translations.MsgAndDialogLang("uploadbtn_stop")
        End If
    End Sub
    Public Function EnterFolder(ByVal Optional location As String = "root", ByVal Optional refreshing As Boolean = False) As Boolean
        Dim OrderBy As String = My.Settings.SortBy
        If My.Settings.OrderDesc Then OrderBy = OrderBy + " desc"
        Dim succeeded As Boolean = False
        If location <> "back" Then
            If Not refreshing Then
                succeeded = drive.GetData(location, OrderBy)
            Else
                succeeded = drive.GetData(location, OrderBy, True)
            End If
        Else
            succeeded = drive.GoBack(OrderBy)
        End If
        If succeeded = False Then
            Return False
        End If
        FolderListBox.DataSource = Nothing
        FolderListBox.DataSource = drive.FolderList
        FilesListBox.DataSource = Nothing
        FilesListBox.DataSource = drive.FileList
        UpdateTranslations()
        My.Settings.LastFolder = drive.currentFolder
        My.Settings.PreviousFolderIDs.Clear()
        My.Settings.PreviousFolderIDs.AddRange(drive.previousFolder.ToArray())
        My.Settings.Save()
        SaveSelectedFilesChecksumButton.Visible = False
        CurrentFolderLabel.Text = drive.currentFolderName
        If location = "root" Or location = "trash" Then
            BackButton.Enabled = False
        Else
            BackButton.Enabled = True
        End If
        UpdateQuota()
        Return True
    End Function
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
            If viewing_trash = False Then EnterFolder(drive.currentFolder, True) Else EnterFolder("trash", True)
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.R Then
            If FilesListBox.SelectedItem IsNot Nothing Then
                If viewing_trash Then
                    WorkWithTrash(FilesListBox.SelectedItems, True, False)
                Else
                    RenameFileOrFolder(drive.FileListID.Item(FilesListBox.Items.IndexOf(FilesListBox.SelectedItem)))
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
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.Q Then
            AddFilesToDownloadQueue()
            e.SuppressKeyPress = True
        ElseIf e.Modifiers = Keys.Control And e.KeyCode = Keys.U Then
            Initiate_CheckFilesToGetRAWUrl("", FilesListBox.SelectedIndices)
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
                If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksums.md5") Else SaveChecksumsFile(drive.currentFolderName & ".md5")
            Else
                If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksum.md5") Else SaveChecksumsFile(FilesListBox.SelectedItem.ToString & ".md5")
            End If
        End If
    End Sub

    Private Sub BtnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim credPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
        credPath = Path.Combine(credPath, ".credentials\" + SoftwareName)
        Dim credfiles As String() = Directory.GetFiles(credPath, "*.TokenResponse-user")
        For Each credfile In credfiles
            Debug.WriteLine(credfile)
            File.Delete(credfile)
        Next
        GoToRoot(True)
        MsgBox(Translations.MsgAndDialogLang("logged_out"))
        Application.Exit()
    End Sub

    Private Sub ViewTrashButton_Click(sender As Object, e As EventArgs) Handles ViewTrashButton.Click
        If viewing_trash = False Then
            viewing_trash = True
            GoToRootLink.Visible = False
            CreateNewFolderButton.Enabled = False
            RestoreToolStripMenuItem.Enabled = True
            MoveToTrashToolStripMenuItem.Enabled = False
            Lang_Select()
            EnterFolder("trash")
        Else
            viewing_trash = False
            GoToRootLink.Visible = True
            CreateNewFolderButton.Enabled = True
            RestoreToolStripMenuItem.Enabled = False
            MoveToTrashToolStripMenuItem.Enabled = True
            CurrentFolderLabel.Text = drive.currentFolderName
            CurrentFolderLabel.Visible = True
            Lang_Select()
            EnterFolder(drive.currentFolder, True)
        End If
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles UploadsListBox.SelectedIndexChanged
        If UploadsListBox.SelectedIndex <> -1 Then
            If ItemInQueueAction.Item(UploadsListBox.SelectedIndex) = 0 Then
                FolderIDTextBox.Text = FolderToUploadOrDownloadIdFileList.Item(UploadsListBox.SelectedIndex)
                drive.GetFolderName(FolderIDTextBox.Text)
                UploadToSelectedFolderButton.Visible = True
                DeselectItemFromUploadQueueButton.Enabled = True
            Else
                Dim FileMetadata As Data.File = drive.GetFileMetadata(FolderToUploadOrDownloadIdFileList.Item(UploadsListBox.SelectedIndex))
                FileNameTextBox.Text = FileMetadata.Name
                FileIDTextbox.Text = FileMetadata.Id
                FileSizeTextbox.Text = String.Format("{0:N2} MB", FileMetadata.Size / 1024 / 1024)
                MIMETypeTextbox.Text = FileMetadata.MimeType
                DateCreatedTextbox.Text = FileMetadata.CreatedTime.ToString
                DateModifiedTextbox.Text = FileMetadata.ModifiedTime.ToString
                MD5ChecksumTextbox.Text = FileMetadata.Md5Checksum
            End If
        Else
            UploadToSelectedFolderButton.Visible = False
            DeselectItemFromUploadQueueButton.Enabled = False
        End If

    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles UploadToSelectedFolderButton.Click
        FolderToUploadOrDownloadIdFileList.Item(UploadsListBox.SelectedIndex) = drive.currentFolder
        FolderIDTextBox.Text = drive.currentFolder
        drive.GetFolderName(FolderIDTextBox.Text)
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
        GoToRoot()
    End Sub

    Private Sub GoToRoot(Optional logout As Boolean = False)
        My.Settings.PreviousFolderIDs.Clear()
        My.Settings.LastFolder = "root"
        My.Settings.Save()
        drive.currentFolder = "root"
        drive.previousFolder.Clear()
        CurrentFolderLabel.Text = drive.currentFolderName
        If Not logout Then
            EnterFolder(drive.currentFolder)
        End If
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
            Process.Start("https://moisescardona.me/google-drive-uploader-explorer-tool-help/")
        ElseIf SpanishRButton.Checked Then
            Process.Start("https://moisescardona.me/ayuda-para-google-drive-uploader-explorer-tool/")
        Else
            Process.Start("https://moisescardona.me/google-drive-uploader-explorer-tool-help/")
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
        EnterFolder(drive.currentFolder, True)
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
        EnterFolder(drive.currentFolder, True)
    End Sub

    Private Sub FileToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles FileToolStripMenuItem1.Click
        OpenFileDialog1.Title = "Select files to upload"
        OpenFileDialog1.Filter = "All Files (*)|*.*"
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            If OpenFileDialog1.FileNames IsNot Nothing Then
                UploadsListBox.Items.AddRange(OpenFileDialog1.FileNames)
                FolderToUploadOrDownloadIdFileList.Add(drive.currentFolder)
                ItemInQueueAction.Add(0)
                My.Settings.UploadQueue.AddRange(OpenFileDialog1.FileNames)
                My.Settings.UploadQueueFolders.Add(drive.currentFolder)
                My.Settings.QueueFileAction.Add("0")
            End If
        End If
        My.Settings.Save()
    End Sub

    Private Sub FolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FolderToolStripMenuItem.Click
        FolderBrowserDialog1.ShowNewFolderButton = False
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            UploadsListBox.Items.Add(FolderBrowserDialog1.SelectedPath)
            FolderToUploadOrDownloadIdFileList.Add(drive.currentFolder)
            ItemInQueueAction.Add(0)
            My.Settings.UploadQueue.Add(FolderBrowserDialog1.SelectedPath)
            My.Settings.UploadQueueFolders.Add(drive.currentFolder)
            My.Settings.QueueFileAction.Add("0")
            GetDirectoriesAndFiles(New DirectoryInfo(FolderBrowserDialog1.SelectedPath))
        End If
        My.Settings.Save()
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
            EnterFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
            DownloadFilesAndFolders(True)
        End If
    End Sub
    Private Sub CheckForFolderAddToQueue()
        If FolderListBox.SelectedItem IsNot Nothing Then
            EnterFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
            AddFolderToDownloadQueue(New List(Of String) From {drive.currentFolder})
            GoBack()
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
            RenameFileOrFolder(drive.FileListID.Item(FilesListBox.Items.IndexOf(FilesListBox.SelectedItem)))
        End If
    End Sub

    Private Sub SelectedFolderToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SelectedFolderToolStripMenuItem1.Click
        If FolderListBox.SelectedItem IsNot Nothing Then
            RenameFileOrFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
        End If
    End Sub

    Private Sub RefreshListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RefreshListToolStripMenuItem.Click
        If viewing_trash Then
            EnterFolder("trash", True)
        Else
            EnterFolder(drive.currentFolder, True)
        End If
    End Sub

    Private Sub SelectedFilesToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles SelectedFilesToolStripMenuItem2.Click
        If FilesListBox.Items.Count > 0 Then
            If FilesListBox.SelectedItems.Count > 0 Then
                If My.Settings.SaveAsChecksumsMD5 Then
                    SaveChecksumsFile("checksums.md5")
                Else
                    If FilesListBox.SelectedItems.Count > 1 Then
                        SaveChecksumsFile(drive.currentFolderName & ".md5")
                    Else
                        SaveChecksumsFile(FilesListBox.SelectedItem.ToString & ".md5")
                    End If
                End If
            Else
                MsgBox(Translations.MsgAndDialogLang("no_files_selected"))
            End If
        Else
            MsgBox(Translations.MsgAndDialogLang("no_files_available"))
        End If
    End Sub

    Private Sub SelectedFolderToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles SelectedFolderToolStripMenuItem2.Click
        If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksums.md5") Else SaveChecksumsFile(drive.currentFolderName & ".md5", True)
    End Sub

    Private Sub FolderListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FolderListBox.SelectedIndexChanged
        If FolderListBox.SelectedItem IsNot Nothing Then
            FolderIDTextBox.Text = drive.FolderListID.Item(FolderListBox.SelectedIndex)
            FolderNameTextbox.Text = drive.GetFolderName(FolderIDTextBox.Text)
        End If
    End Sub

    Private Sub FilesListBox_MouseDoubleClick(sender As Object, e As EventArgs) Handles FilesListBox.MouseDoubleClick
        OpenInBrowser()
    End Sub

    Private Sub ChecksumsEncodeFormatComboBox_DropDownClosed(sender As Object, e As EventArgs) Handles ChecksumsEncodeFormatComboBox.DropDownClosed
        My.Settings.EncodeChecksumsFormat = ChecksumsEncodeFormatComboBox.SelectedIndex
        My.Settings.Save()
    End Sub

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
                Process.Start("https://drive.google.com/file/d/" + drive.FileListID.Item(FilesListBox.SelectedIndex) + "/view")
            End If
        Else
            If FolderListBox.SelectedItem IsNot Nothing Then
                Process.Start("https://drive.google.com/drive/folders/" + drive.FolderListID.Item(FolderListBox.SelectedIndex))
            End If
        End If
    End Sub
    Private Sub MoveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MoveToolStripMenuItem.Click
        MoveFileOrFolder()
    End Sub

    Private Sub MoveFileOrFolder(Optional IsFolder As Boolean = False)
        MoveDialog.FolderIDs = New List(Of String)
        MoveDialog.CurrentFolder = drive.currentFolder
        MoveDialog.PreviousFolderId = drive.previousFolder
        MoveDialog.FolderIDs = drive.FolderListID
        If Not IsFolder Then
            If FilesListBox.SelectedItems IsNot Nothing Then
                For Each item As String In FilesListBox.SelectedItems
                    MoveDialog.ItemsToMove.Add(drive.FileListID.Item(FilesListBox.Items.IndexOf(item)))
                Next
                MoveDialog.Show()
            Else
                Translations.MsgAndDialogLang("no_files_selected")
            End If
        Else
            If FolderListBox.SelectedItems IsNot Nothing Then
                For Each item As String In FolderListBox.SelectedItems
                    MoveDialog.ItemsToMove.Add(drive.FolderListID.Item(FolderListBox.Items.IndexOf(item)))
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
    Private Sub Initiate_FolderCheckFilesToGetRAWUrl()
        If FolderListBox.SelectedItem IsNot Nothing Then
            Dim Path As String = drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)).ToString
            EnterFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
            Initiate_CheckFilesToGetRAWUrl(Path)
        End If
    End Sub
    Private Sub Initiate_CheckFilesToGetRAWUrl(Optional Initial_path As String = "", Optional SelectedIndices As ListBox.SelectedIndexCollection = Nothing)
        Dim DownloadUrls As New Download_URLs
        Dim Path As New List(Of String)
        If Not String.IsNullOrWhiteSpace(Initial_path) Then
            Path.Add(Initial_path)
        End If
        CheckFilesToGetRAWUrl(Path, DownloadUrls, SelectedIndices)
        If DownloadUrls.URLs.Count > 0 Then
            For Each item In DownloadUrls.URLs
                DownloadUrls.RichTextBox1.Text += item + vbCrLf
            Next
            DownloadUrls.ShowDialog()
        Else
            MsgBox(Translations.MsgAndDialogLang("no_files_selected"))
        End If
    End Sub

    Private Function CheckFilesToGetRAWUrl(Path As List(Of String), DownloadUrls As Download_URLs, Optional SelectedIndices As ListBox.SelectedIndexCollection = Nothing) As Download_URLs
        'This creates the full path of the file by getting the ID Name.
        Dim FullPath As String = ""
        If Path.Count > 0 Then
            For Each item In Path
                Try
                    Dim GetFolderName As FilesResource.GetRequest = drive.service.Files.Get(item)
                    Dim FolderNameMetadata As Data.File = GetFolderName.Execute
                    FullPath = FullPath + FolderNameMetadata.Name + "/"
                Catch ex As Exception

                End Try
            Next
        End If
        'Once Full Path has been created, we check for files inside the folder. If there's files, we will store their URL, Path, and MD5 checksum.
        If SelectedIndices Is Nothing Then
            For Each item As String In FilesListBox.Items
                DownloadUrls.URLs.Add("https://www.googleapis.com/drive/v3/files/" + drive.FileListID.Item(FilesListBox.Items.IndexOf(item)) + "?alt=media")
                DownloadUrls.Path.Add(FullPath + item)
                DownloadUrls.Checksums.Add(drive.FileMD5List.Item(FilesListBox.Items.IndexOf(item)))
            Next
        Else
            For Each index As Integer In SelectedIndices
                DownloadUrls.URLs.Add("https://www.googleapis.com/drive/v3/files/" + drive.FileListID.Item(index) + "?alt=media")
                DownloadUrls.Path.Add(FullPath + FilesListBox.Items(index).ToString)
                DownloadUrls.Checksums.Add(drive.FileMD5List.Item(index))
            Next
        End If
        'Finally, this loop checks if there are folders inside the folder we are. We start a recursion loop by calling this same function for each folder inside the folder.
        If FolderListBox.Items.Count > 0 Then
            Dim FolderList As New List(Of String)
            For Each FolderInList As String In FolderListBox.Items
                FolderList.Add(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderInList)))
            Next
            For Each Folder2 As String In FolderList
                Path.Add(drive.FolderListID.Item(drive.FolderListID.IndexOf(Folder2)))
                FolderListBox.ClearSelected()
                FolderListBox.SelectedItem = FolderListBox.Items.Item(drive.FolderListID.IndexOf(Folder2))
                EnterFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
                DownloadUrls = CheckFilesToGetRAWUrl(Path, DownloadUrls)
                GoBack()
                Path.Remove(Folder2)
            Next
        End If
        Return DownloadUrls
    End Function
    Private Sub GetRawDownloadURLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GetRawDownloadURLToolStripMenuItem.Click
        Initiate_CheckFilesToGetRAWUrl("", FilesListBox.SelectedIndices)
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
        If FolderListBox.SelectedItem IsNot Nothing Then EnterFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
        If My.Settings.SaveAsChecksumsMD5 Then SaveChecksumsFile("checksums.md5", True) Else SaveChecksumsFile(drive.currentFolderName & ".md5", True)
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
            RenameFileOrFolder(drive.FileListID.Item(FilesListBox.Items.IndexOf(FilesListBox.SelectedItem)))
        End If
    End Sub

    Private Sub RenameToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles RenameToolStripMenuItem2.Click
        If FolderListBox.SelectedItem IsNot Nothing Then
            RenameFileOrFolder(drive.FolderListID.Item(FolderListBox.Items.IndexOf(FolderListBox.SelectedItem)))
        End If
    End Sub
    Private Sub verifyHash1()
        Dim Index As Integer = FilesListBox.SelectedIndex
        Dim FileBrowser As New OpenFileDialog With {
            .Title = Translations.MsgAndDialogLang("browse_file_verify_checksum"),
            .Filter = FilesListBox.Items.Item(Index).ToString() + "|*" + FilesListBox.Items.Item(Index).ToString(),
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
        StatusLabel.BeginInvoke(Sub() StatusLabel.Text = Translations.MsgAndDialogLang("verifying"))
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
        StatusLabel.BeginInvoke(Sub() StatusLabel.Text = Translations.MsgAndDialogLang("done"))
        Dim ExpectedHash As String = drive.FileMD5List.Item(Index).ToString()
        If Hash = ExpectedHash Then
            MsgBox(Translations.MsgAndDialogLang("checksum_hash_match_1") + " " + filename + "." + Environment.NewLine + Environment.NewLine + Translations.MsgAndDialogLang("computed_hash") + " " + Hash + Environment.NewLine + Translations.MsgAndDialogLang("expected_hash") + " " + ExpectedHash, MsgBoxStyle.Information)
        Else
            MsgBox(Translations.MsgAndDialogLang("checksum_hash_not_match_1") + " " + filename + "." + Environment.NewLine + Environment.NewLine + Translations.MsgAndDialogLang("computed_hash") + " " + Hash + Environment.NewLine + Translations.MsgAndDialogLang("expected_hash") + " " + ExpectedHash, MsgBoxStyle.Critical)
        End If
    End Sub

    Private Sub VerifyChecksumToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VerifyChecksumToolStripMenuItem.Click
        If FilesListBox.Items.Count > 0 Then
            If FilesListBox.SelectedItem IsNot Nothing Then
                verifyHash1()
            Else
                MsgBox(Translations.MsgAndDialogLang("no_file_selected"))
            End If
        Else
            MsgBox(Translations.MsgAndDialogLang("no_files_available"))
        End If
    End Sub

    Private Sub VerifyChecksumToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles VerifyChecksumToolStripMenuItem1.Click
        If FilesListBox.Items.Count > 0 Then
            If FilesListBox.SelectedItem IsNot Nothing Then
                verifyHash1()
            Else
                MsgBox(Translations.MsgAndDialogLang("no_file_selected"))
            End If
        Else
            MsgBox(Translations.MsgAndDialogLang("no_files_available"))
        End If
    End Sub

    Private Sub GetRawDownloadURLsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GetRawDownloadURLsToolStripMenuItem.Click
        Initiate_FolderCheckFilesToGetRAWUrl()
    End Sub

    Private Sub OpenFolderId_Click(sender As Object, e As EventArgs) Handles OpenFolderId.Click
        Try
            EnterFolder(FolderIDTextBox.Text)
        Catch
            MessageBox.Show(Translations.MsgAndDialogLang("folder_id_incorrect"))
        End Try
    End Sub

    Private Sub CurrentFolderLabel_Click(sender As Object, e As EventArgs) Handles CurrentFolderLabel.Click
        Clipboard.SetText(drive.currentFolderName)
    End Sub

    Private Sub SetDownloadLocationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetDownloadLocationToolStripMenuItem.Click
        SetDownloadLocationDialog.ShowDialog()
    End Sub

    Private Sub AddToQueueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddToQueueToolStripMenuItem.Click
        AddFilesToDownloadQueue()
    End Sub

    Private Sub AddToQueueToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddToQueueToolStripMenuItem1.Click
        CheckForFolderAddToQueue()
    End Sub

    Private Sub SendToTrashToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SendToTrashToolStripMenuItem.Click
        If viewing_trash = False Then
            If FilesListBox.SelectedItem IsNot Nothing Then
                WorkWithTrash(FilesListBox.SelectedItems, True, True)
            End If
        End If
    End Sub

    Private Sub FilesContextMenu_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles FilesContextMenu.Opening
        If viewing_trash Then
            SendToTrashToolStripMenuItem.Enabled = False
        Else
            SendToTrashToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub SendToTrashToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SendToTrashToolStripMenuItem1.Click
        If viewing_trash = False Then
            If FolderListBox.SelectedItem IsNot Nothing Then
                WorkWithTrash(FolderListBox.SelectedItems, False, True)
            End If
        End If
    End Sub

    Private Sub FoldersContextMenu_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles FoldersContextMenu.Opening
        If viewing_trash Then
            SendToTrashToolStripMenuItem1.Enabled = False
        Else
            SendToTrashToolStripMenuItem1.Enabled = True
        End If
    End Sub
End Class
