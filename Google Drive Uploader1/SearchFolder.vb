Imports Google.Apis.Drive.v3

Public Class SearchFolder
    Private FolderIdsListBox As New ListBox
    Private PreviousFolderName As New ListBox
    Private PreviousFolderId As New ListBox
    Private Sub SearchFolder_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If Form1.CheckBox1.Checked = True Then
            Label1.Text = "Current Folder:"
        Else
            Label1.Text = "Carpeta Actual"
        End If
        SearchFolders("root")
    End Sub
    Private Sub SearchFolders(Directory As String)
        ListBox1.Items.Clear()
        FolderIdsListBox.Items.Clear()
        Dim listRequest As FilesResource.ListRequest = Form1.service.Files.List()
        listRequest.Q = "mimeType='application/vnd.google-apps.folder' and '" & Directory & "' in parents"
        listRequest.Fields = "nextPageToken, files(id, name)"
        ' List files.
        Try
            Dim files = listRequest.Execute()
            If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
                For Each file In files.Files
                    ListBox1.Items.Add(file.Name)
                    FolderIdsListBox.Items.Add(file.Id)
                Next
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub ListBox1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListBox1.MouseDoubleClick
        If String.IsNullOrEmpty(ListBox1.SelectedItem) = False Then
            Dim GoToFolderID As String = FolderIdsListBox.Items.Item(ListBox1.SelectedIndex)
            If TextBox1.Text = "root" Then
                PreviousFolderId.Items.Add("root")
                PreviousFolderName.Items.Add("root")
            Else
                PreviousFolderId.Items.Add(TextBox2.Text)
                PreviousFolderName.Items.Add(TextBox1.Text)
            End If
            TextBox1.Text = ListBox1.SelectedItem
            TextBox2.Text = FolderIdsListBox.Items.Item(ListBox1.SelectedIndex)
            SearchFolders(GoToFolderID)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text = "root" = False Then
            Dim PreviousFolderIdBeforeRemoving = PreviousFolderId.Items.Item(PreviousFolderId.Items.Count - 1)
            TextBox1.Text = PreviousFolderName.Items.Item(PreviousFolderId.Items.Count - 1)
            TextBox2.Text = PreviousFolderIdBeforeRemoving
            PreviousFolderName.Items.RemoveAt(PreviousFolderId.Items.Count - 1)
            PreviousFolderId.Items.RemoveAt(PreviousFolderId.Items.Count - 1)
            SearchFolders(PreviousFolderIdBeforeRemoving)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.TextBox2.Text = TextBox2.Text
        Me.Close()
    End Sub
End Class