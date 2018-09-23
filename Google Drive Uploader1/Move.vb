Public Class Move
    Private FolderIDs As List(Of String) = New List(Of String)
    Private Sub Move_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub GoToFolder(FolderID As String)
        'Dim listRequestString As String = "mimeType='application/vnd.google-apps.folder' and '" & FolderID & "' in parents and trashed = false"
        'ListBox1.Items.Clear()
        'FolderIDs.Clear()
        'Dim PageToken2 As String = String.Empty
        'Do
        '    Dim listRequest As FilesResource.ListRequest = Form1.service.Files.List()
        '    listRequest.Q = listRequestString
        '    listRequest.Fields = "nextPageToken, files(id, name)"
        '    listRequest.OrderBy = OrderBy
        '    listRequest.PageToken = PageToken2
        '    Try
        '        Dim files = listRequest.Execute()
        '        If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
        '            For Each file In files.Files
        '                FolderListBox.Items.Add(file.Name)
        '                FolderIdsListBox.Items.Add(file.Id)
        '            Next
        '        End If
        '        PageToken2 = files.NextPageToken
        '    Catch ex As Exception
        '    End Try
        'Loop While PageToken2 = String.Empty = False
        'If CurrentFolder = "root" Or CurrentFolder = "trash" Then
        '    Button10.Enabled = False
        'Else
        '    Button10.Enabled = True
        'End If
    End Sub
End Class