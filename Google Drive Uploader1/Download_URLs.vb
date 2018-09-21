Public Class Download_URLs
    Public URLs As List(Of String) = New List(Of String)
    Public Filenames As List(Of String) = New List(Of String)

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SaveDialog As New SaveFileDialog With {
            .Filter = "Shell Script|*.sh",
            .Title = "Browse to save the shell script"
        }
        Dim Result As DialogResult = SaveDialog.ShowDialog
        If Result = DialogResult.OK Then
            Dim FileStream As New IO.StreamWriter(SaveDialog.FileName, False, New System.Text.UTF8Encoding(False))
            For Each URL In URLs
                If CheckBox1.Checked Then
                    FileStream.Write("[ ! -f " + Filenames.Item(URLs.IndexOf(URL)) + " ] && wget " + URL + " -O " + Filenames.Item(URLs.IndexOf(URL)) + vbLf)
                Else
                    FileStream.Write("wget " + URL + " -O " + Filenames.Item(URLs.IndexOf(URL)) + vbLf)
                End If
            Next
            FileStream.Close()
            MsgBox("Shell script saved.")
        End If
    End Sub
End Class