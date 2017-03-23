Public Class Donations
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Donations_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Form1.RadioButton1.Checked = True Then Button1.Text = "Close" Else Button1.Text = "Cerrar"
    End Sub
End Class