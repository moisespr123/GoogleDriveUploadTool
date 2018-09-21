Public Class Download_URLs
    Public URLs As List(Of String) = New List(Of String)
    Public Filenames As List(Of String) = New List(Of String)

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SaveDialog As New SaveFileDialog With{
            .Filter = "Shell Script|*.sh",
            .Title = "Browse to save the shell script"
        }
        Dim Result As DialogResult = SaveDialog.ShowDialog
        If Result = DialogResult.OK
            Dim FileStream As new IO.StreamWriter(SaveDialog.Filename, False, System.Text.Encoding.UTF8)
            For Each URL In URLs
                FileStream.Write("wget " + URL + " -O " + Filenames.Item(URLs.IndexOf(URL)) + VbLf)
            Next
            FileStream.Close()
            MsgBox("Shell script saved.")
        End If
    End Sub
End Class