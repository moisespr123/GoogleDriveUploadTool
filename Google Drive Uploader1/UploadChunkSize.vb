Public Class UploadChunkSize
    Private Sub UploadChunkSize_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NumericUpDown1.Maximum = Decimal.MaxValue
        If Form1.RadioButton1.Checked Or Form1.RadioButton3.Checked Then
            Me.Text = "Chunk Size"
            Label1.Text = "Enter chunk size"
            Button1.Text = "Save"
        Else
            Me.Text = "Tamaño de pedazo"
            Label1.Text = "Introduzca tamaño de pedazo"
            Button1.Text = "Guardar"
        End If
        If My.Computer.FileSystem.FileExists("chunkmultiplier.txt") Then
            Dim value As Integer = My.Computer.FileSystem.ReadAllText("chunkmultiplier.txt") * 256
            If value < 256 Then
                NumericUpDown1.Value = 256
            Else
                NumericUpDown1.Value = value
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim result As Integer = NumericUpDown1.Value / 256
        My.Computer.FileSystem.WriteAllText("chunkmultiplier.txt", result, False)
        Me.Close()
    End Sub

End Class