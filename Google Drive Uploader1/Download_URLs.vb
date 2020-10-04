Public Class Download_URLs
    Public URLs As List(Of String) = New List(Of String)
    Public Checksums As List(Of String) = New List(Of String)
    Public Path As List(Of String) = New List(Of String)
    Private AuthorizationToken As String = ""

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
                If URLs.IndexOf(URL) > 0 Then
                    If Not IO.Path.GetDirectoryName(Path.Item(URLs.IndexOf(URL))) = IO.Path.GetDirectoryName(Path.Item(URLs.IndexOf(URL) - 1)) Then
                        FileStream.Write("mkdir -p """ + pathTxt.Text + "/" + IO.Path.GetDirectoryName(Path.Item(URLs.IndexOf(URL))).Replace("\", "/") + """" + vbLf)
                    End If
                Else
                    FileStream.Write("mkdir -p """ + pathTxt.Text + "/" + IO.Path.GetDirectoryName(Path.Item(URLs.IndexOf(URL))).Replace("\", "/") + """" + vbLf)
                End If
                If DownloadIfNotExistCheckbox.Checked Then
                    If wget.Checked Then
                        FileStream.Write("[ ! -f """ + Path.Item(URLs.IndexOf(URL)) + """ ] && wget --header=""Authorization: Bearer " + AuthorizationToken + """ " + URL + " -O """ + pathTxt.Text + "/" + Path.Item(URLs.IndexOf(URL)) + """" + vbLf)
                    Else
                        FileStream.Write("[ ! -f """ + Path.Item(URLs.IndexOf(URL)) + """ ] && curl --header ""Authorization: Bearer " + AuthorizationToken + """ " + URL + " -o """ + pathTxt.Text + "/" + Path.Item(URLs.IndexOf(URL)) + """" + vbLf)
                    End If
                Else
                    If wget.Checked Then
                        FileStream.Write("wget --header=""Authorization: Bearer " + AuthorizationToken + """ " + URL + " -O """ + pathTxt.Text + "/" + Path.Item(URLs.IndexOf(URL)) + """" + vbLf)
                    Else
                        FileStream.Write("curl --header ""Authorization: Bearer " + AuthorizationToken + """ " + URL + " -o """ + pathTxt.Text + "/" + Path.Item(URLs.IndexOf(URL)) + """" + vbLf)
                    End If
                End If
            Next
            FileStream.Write("echo ""Finished downloading files""" + vbLf)
            If CheckChecksumsAfterDownloadsCheckbox.Checked Then
                FileStream.Write("echo ""Checking file checksums""" + vbLf)
                For Each checksum In Checksums
                    FileStream.Write("md5sum -c - <<< """ + Checksums.Item(Checksums.IndexOf(checksum)) + " " + pathTxt.Text + "/" + Path.Item(Checksums.IndexOf(checksum)) + """" + vbLf)
                Next
                FileStream.Write("echo ""Finished checking file checksums""" + vbLf)
            End If
            FileStream.Close()
            MsgBox(Translations.MsgAndDialogLang("shell_script_saved"))
        End If
    End Sub

    Private Async Sub Download_URLs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Form1.EnglishRButton.Checked Then
            AuthorizationHeaderLabel.Text = "  Authorization Header:"
            Me.Text = "Download URLs"
            DownloadIfNotExistCheckbox.Text = "Download only if file does not exist"
            CheckChecksumsAfterDownloadsCheckbox.Text = "Check File checksums after downloads"
            SaveButton.Text = "Save script"
        ElseIf Form1.SpanishRButton.Checked Then
            AuthorizationHeaderLabel.Text = "Header de Autorización:"
            Me.Text = "Enlaces de descarga"
            DownloadIfNotExistCheckbox.Text = "Descargar si el archivo no existe"
            CheckChecksumsAfterDownloadsCheckbox.Text = "Verificar checksums luego de descargas"
            SaveButton.Text = "Guardar script"
        Else
            AuthorizationHeaderLabel.Text = "  Authorization Header:"
            Me.Text = "Download URLs"
            DownloadIfNotExistCheckbox.Text = "Download only if file does not exist"
            CheckChecksumsAfterDownloadsCheckbox.Text = "Check File checksums after downloads"
            SaveButton.Text = "Save script"
        End If
        AuthorizationToken = Await Form1.drive.credential.GetAccessTokenForRequestAsync()
        AuthorizationHeaderTextBox.Text = "Authorization: Bearer " + AuthorizationToken
    End Sub
End Class