Public Class SetDownloadLocationDialog
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim Dialog As New FolderBrowserDialog With {
            .SelectedPath = TextBox1.Text,
            .ShowNewFolderButton = True
        }
        If Dialog.ShowDialog = DialogResult.OK Then
            TextBox1.Text = Dialog.SelectedPath
            My.Settings.DownloadLocation = Dialog.SelectedPath
            My.Settings.Save()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Not String.IsNullOrWhiteSpace(TextBox1.Text) Then
            My.Settings.DownloadLocation = TextBox1.Text
            My.Settings.Save()
        End If
        Me.Close()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If Not String.IsNullOrWhiteSpace(TextBox1.Text) Then
            My.Settings.DownloadLocation = TextBox1.Text
            My.Settings.Save()
        End If
    End Sub

    Private Sub SetDownloadLocationDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = My.Settings.DownloadLocation
    End Sub
End Class