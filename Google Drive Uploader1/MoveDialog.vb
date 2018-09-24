Imports Google.Apis.Drive.v3
Imports Google.Apis.Drive.v3.Data

Public Class MoveDialog
    Private ReadOnly _folderNames As New List(Of String)
    Public FolderIDs As New List(Of String)
    Public PreviousFolderId As New List(Of String)
    Public ReadOnly ItemsToMove As New List(Of String)
    Public CurrentFolder As String = "root"
    Private _service As DriveService = Nothing

    Private Sub Move_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _service = Form1.service
        GoToFolder(CurrentFolder, True)
    End Sub

    Private Sub GoToFolder(folderId As String, Optional goingBack As Boolean = False)
        If Not goingBack Then PreviousFolderId.Add(CurrentFolder)
        CurrentFolder = folderId
        Text = Translations.MsgAndDialogLang("move") + " - " + GetCurrentFolderIdName()
        Dim listRequestString As String = "mimeType='application/vnd.google-apps.folder' and '" & folderId &
                                          "' in parents and trashed = false"
        _folderNames.Clear()
        FolderIDs.Clear()
        Dim pageToken2 As String = String.Empty
        Do
            Dim listRequest As FilesResource.ListRequest = _service.Files.List()
            listRequest.Q = listRequestString
            listRequest.Fields = "nextPageToken, files(id, name)"
            listRequest.OrderBy = My.Settings.SortBy
            listRequest.PageToken = pageToken2
            Try
                Dim files = listRequest.Execute()
                If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
                    For Each folder In files.Files
                        _folderNames.Add(folder.Name)
                        FolderIDs.Add(folder.Id)
                    Next
                End If
                pageToken2 = files.NextPageToken
            Catch ex As Exception
            End Try
        Loop While pageToken2 = String.Empty = False
        FolderListBox.DataSource = Nothing
        FolderListBox.DataSource = _folderNames
        If CurrentFolder = "root" Then
            BackButton.Enabled = False
        Else
            BackButton.Enabled = True
        End If
    End Sub

    Private Sub FolderListBox_MouseDoubleClick(sender As Object, e As MouseEventArgs) _
        Handles FolderListBox.MouseDoubleClick
        If FolderListBox.SelectedItem IsNot Nothing Then _
            GoToFolder(FolderIDs.Item(FolderListBox.SelectedIndex).ToString)
    End Sub

    Private Function GetCurrentFolderIdName() As String
        Try
            Dim getFolderName As FilesResource.GetRequest = _service.Files.Get(CurrentFolder)
            Dim folderNameMetadata As File = getFolderName.Execute
            Return folderNameMetadata.Name
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Sub GoBack()
        If CurrentFolder = "root" = False Then
            If PreviousFolderId.Count > 0 Then
                Dim previousFolderIdBeforeRemoving As String = PreviousFolderId.Item(PreviousFolderId.Count - 1)
                PreviousFolderId.RemoveAt(PreviousFolderId.Count - 1)
                CurrentFolder = previousFolderIdBeforeRemoving
                GoToFolder(CurrentFolder, True)
            End If
        End If
    End Sub

    Private Sub BackButton_Click(sender As Object, e As EventArgs) Handles BackButton.Click
        GoBack()
    End Sub

    Private Async Sub MoveButton_Click(sender As Object, e As EventArgs) Handles MoveButton.Click
        For Each item In ItemsToMove
            Dim previousFileParents As File = Await New FilesResource.GetRequest(_service, item) With {.Fields = "parents"}.ExecuteAsync()
            Await New FilesResource.UpdateRequest(_service, New File(), item) With {.RemoveParents = String.Join(",", previousFileParents.Parents), .AddParents = CurrentFolder}.ExecuteAsync()
        Next
        If ItemsToMove.Count > 1 Then
            MsgBox(Translations.MsgAndDialogLang("files_moved"))
        Else
            MsgBox(Translations.MsgAndDialogLang("file_moved"))
        End If
        Form1.RefreshFileList(Form1.CurrentFolder)
        Close()
    End Sub
End Class