Imports Google.Apis.Drive.v3

Public Class SearchFolder
    Private FolderIdsListBox As New ListBox
    Private PreviousFolderName As New ListBox
    Private PreviousFolderId As New ListBox
    Private Sub SearchFolder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Form1.RadioButton1.Checked = True Then
            Label1.Text = "Current Folder:"
            Label2.Text = "Current ID:"
            Button1.Text = "Select"
            Button2.Text = "Back"
            Button3.Text = "New"
        Else
            Label1.Text = "Carpeta Actual"
            Label2.Text = "ID Actual:"
            Button1.Text = "Seleccionar"
            Button2.Text = "Atrás"
            Button3.Text = "Nuevo"
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
        If String.IsNullOrEmpty(ListBox1.SelectedItem) Then
            Form1.TextBox1.Text = TextBox1.Text
            Form1.TextBox2.Text = TextBox2.Text
        Else
            Form1.TextBox1.Text = ListBox1.SelectedItem
            Form1.TextBox2.Text = FolderIdsListBox.Items.Item(ListBox1.SelectedIndex)
        End If
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim FolderNameToCreate As Object
        Dim Message, Title As String
        If Form1.RadioButton1.Checked = True Then
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
            ParentFolder.Add(TextBox2.Text)
            FolderMetadata.Parents = ParentFolder
            FolderMetadata.MimeType = "application/vnd.google-apps.folder"
            Dim CreateFolder As FilesResource.CreateRequest = Form1.service.Files.Create(FolderMetadata)
            CreateFolder.Fields = "id"
            Dim FolderID As Data.File = CreateFolder.Execute
            If TextBox1.Text = "root" Then
                PreviousFolderName.Items.Add("root")
                PreviousFolderId.Items.Add("root")
            Else
                PreviousFolderName.Items.Add(TextBox1.Text)
                PreviousFolderId.Items.Add(TextBox2.Text)
            End If
            TextBox1.Text = FolderNameToCreate
            TextBox2.Text = FolderID.Id
            SearchFolders(FolderID.Id)
        End If
    End Sub
End Class