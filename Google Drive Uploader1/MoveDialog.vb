Imports Google.Apis.Drive.v3

Public Class MoveDialog
    Public FolderIDs As New List(Of String)
    Public PreviousFolderId As New List(Of String)
    Public ItemsToMove As New List(Of String)
    Public CurrentFolder As String = "root"
    Private service As DriveService = Form1.service
    Private Sub Move_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GoToFolder(CurrentFolder, True)
    End Sub

    Private Sub GoToFolder(FolderID As String, Optional GoingBack As Boolean = False)
        If Not GoingBack Then PreviousFolderId.Add(CurrentFolder)
        CurrentFolder = FolderID
        Me.Text = "Move - " + GetCurrentFolderIDName()
        Dim listRequestString As String = "mimeType='application/vnd.google-apps.folder' and '" & FolderID & "' in parents and trashed = false"
        FolderListBox.Items.Clear()
        FolderIDs.Clear()
        Dim PageToken2 As String = String.Empty
        Do
            Dim listRequest As FilesResource.ListRequest = service.Files.List()
            listRequest.Q = listRequestString
            listRequest.Fields = "nextPageToken, files(id, name)"
            listRequest.OrderBy = My.Settings.SortBy
            listRequest.PageToken = PageToken2
            Try
                Dim files = listRequest.Execute()
                If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
                    For Each folder In files.Files
                        FolderListBox.Items.Add(folder.Name)
                        FolderIDs.Add(folder.Id)
                    Next
                End If
                PageToken2 = files.NextPageToken
            Catch ex As Exception
            End Try
        Loop While PageToken2 = String.Empty = False
        If CurrentFolder = "root" Then
            BackButton.Enabled = False
        Else
            BackButton.Enabled = True
        End If
    End Sub

    Private Sub FolderListBox_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles FolderListBox.MouseDoubleClick
        If FolderListBox.SelectedItem IsNot Nothing Then GoToFolder(FolderIDs.Item(FolderListBox.SelectedIndex).ToString)
    End Sub
    Private Function GetCurrentFolderIDName() As String
        Try
            Dim GetFolderName As FilesResource.GetRequest = service.Files.Get(CurrentFolder)
            Dim FolderNameMetadata As Data.File = GetFolderName.Execute
            Return FolderNameMetadata.Name
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
    Private Sub GoBack()
        If CurrentFolder = "root" = False Then
            If PreviousFolderId.Count > 0 Then
                Dim PreviousFolderIdBeforeRemoving As String = PreviousFolderId.Item(PreviousFolderId.Count - 1).ToString
                PreviousFolderId.RemoveAt(PreviousFolderId.Count - 1)
                CurrentFolder = PreviousFolderIdBeforeRemoving
                GoToFolder(CurrentFolder, True)
            End If
        End If
    End Sub

    Private Sub BackButton_Click(sender As Object, e As EventArgs) Handles BackButton.Click
        GoBack()
    End Sub

    Private Sub MoveButton_Click(sender As Object, e As EventArgs) Handles MoveButton.Click
        Dim Parents As New List(Of String) From {
            CurrentFolder
        }
        Dim FileMetadata As New Data.File With {.Parents = Parents}
        For Each item In ItemsToMove
            service.Files.Update(FileMetadata, item).ExecuteAsync()
        Next
        Me.Close()
    End Sub
End Class