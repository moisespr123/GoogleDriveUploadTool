Public Class UploadChunkSize
    Public file As String = "chunkmultiplier.txt"
    Private Sub UploadChunkSize_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NumericUpDown1.Maximum = Decimal.MaxValue
        If Form1.EnglishRButton.Checked Or Form1.TChineseRButton.Checked Then
            Me.Text = "Chunk Size"
            Label1.Text = "Enter chunk size"
            Button1.Text = "Save"
        Else
            Me.Text = "Tamaño de pedazo"
            Label1.Text = "Introduzca tamaño de pedazo"
            Button1.Text = "Guardar"
        End If
        If file = "chunkmultiplier.txt" Then
            NumericUpDown1.Minimum = 256
            unitLabel.Text = "KB"
            If My.Computer.FileSystem.FileExists(file) Then NumericUpDown1.Value = CInt(My.Computer.FileSystem.ReadAllText(file)) * 256
            If NumericUpDown1.Value < NumericUpDown1.Minimum Then NumericUpDown1.Value = NumericUpDown1.Minimum
        Else
            unitLabel.Text = "MB"
            NumericUpDown1.Minimum = 1024
            If My.Computer.FileSystem.FileExists(file) Then NumericUpDown1.Value = CInt(My.Computer.FileSystem.ReadAllText(file))
            If NumericUpDown1.Value < NumericUpDown1.Minimum Then NumericUpDown1.Value = NumericUpDown1.Minimum
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If file = "chunkmultiplier.txt" Then
            Dim result As Integer = CInt(NumericUpDown1.Value / 256)
            My.Computer.FileSystem.WriteAllText(file, result.ToString(), False)
        Else
            My.Computer.FileSystem.WriteAllText(file, NumericUpDown1.Value.ToString(), False)
        End If
        Me.Close()
    End Sub

End Class