<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MoveDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.FolderListBox = New System.Windows.Forms.ListBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.BackButton = New System.Windows.Forms.Button()
        Me.SuspendLayout
        '
        'FolderListBox
        '
        Me.FolderListBox.FormattingEnabled = true
        Me.FolderListBox.Location = New System.Drawing.Point(12, 12)
        Me.FolderListBox.Name = "FolderListBox"
        Me.FolderListBox.Size = New System.Drawing.Size(481, 342)
        Me.FolderListBox.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(105, 360)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(388, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Move Here"
        Me.Button1.UseVisualStyleBackColor = true
        '
        'BackButton
        '
        Me.BackButton.Location = New System.Drawing.Point(12, 360)
        Me.BackButton.Name = "BackButton"
        Me.BackButton.Size = New System.Drawing.Size(87, 23)
        Me.BackButton.TabIndex = 2
        Me.BackButton.Text = "Back"
        Me.BackButton.UseVisualStyleBackColor = true
        '
        'MoveDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(506, 393)
        Me.Controls.Add(Me.BackButton)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.FolderListBox)
        Me.Name = "MoveDialog"
        Me.Text = "Move"
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents FolderListBox As ListBox
    Friend WithEvents Button1 As Button
    Friend WithEvents BackButton As Button
End Class
