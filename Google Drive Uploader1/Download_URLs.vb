Public Class Download_URLs
    Public URLs As List(Of String) = New List(Of String)
    Public Filenames As List(Of String) = New List(Of String)
    Public Checksums As List(Of String) = New List(Of String)

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        Dim SaveDialog As New SaveFileDialog With {
            .Filter = "Shell Script|*.sh",
            .Title = Translations.MsgAndDialogLang("browse_save_shell_script")
        }
        Dim Result As DialogResult = SaveDialog.ShowDialog
        If Result = DialogResult.OK Then
            Dim FileStream As New IO.StreamWriter(SaveDialog.FileName, False, New System.Text.UTF8Encoding(False))
            FileStream.Write("echo ""Downloading files""" + vbLf)
            For Each URL In URLs
                If DownloadIfNotExistCheckbox.Checked Then
                    FileStream.Write("[ ! -f " + Filenames.Item(URLs.IndexOf(URL)) + " ] && wget " + URL + " -O " + pathTxt.Text + Filenames.Item(URLs.IndexOf(URL)) + vbLf)
                Else
                    FileStream.Write("wget " + URL + " -O " + Filenames.Item(URLs.IndexOf(URL)) + vbLf)
                End If
            Next
            FileStream.Write("echo ""Finished downloading files""" + vbLf)
            If CheckChecksumsAfterDownloadsCheckbox.Checked Then
                FileStream.Write("echo ""Checking file checksums""" + vbLf)
                For Each checksum In Checksums
                    FileStream.Write("md5sum -c - <<< """ + Checksums.Item(Checksums.IndexOf(checksum)) + " " + pathTxt.Text + Filenames.Item(Checksums.IndexOf(checksum)) + """" + vbLf)
                Next
                FileStream.Write("echo ""Finished checking file checksums""" + vbLf)
            End If
            FileStream.Close()
            MsgBox(Translations.MsgAndDialogLang("shell_script_saved"))
        End If
    End Sub
End Class