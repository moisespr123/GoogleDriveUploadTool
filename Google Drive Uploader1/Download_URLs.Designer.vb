<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Download_URLs
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
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.DownloadIfNotExistCheckbox = New System.Windows.Forms.CheckBox()
        Me.CheckChecksumsAfterDownloadsCheckbox = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.RichTextBox1.Location = New System.Drawing.Point(12, 12)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(776, 426)
        Me.RichTextBox1.TabIndex = 0
        Me.RichTextBox1.Text = ""
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.SaveButton.Location = New System.Drawing.Point(12, 467)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(775, 23)
        Me.SaveButton.TabIndex = 1
        Me.SaveButton.Text = "Save WGET script"
        Me.SaveButton.UseVisualStyleBackColor = true
        '
        'DownloadIfNotExistCheckbox
        '
        Me.DownloadIfNotExistCheckbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.DownloadIfNotExistCheckbox.AutoSize = true
        Me.DownloadIfNotExistCheckbox.Location = New System.Drawing.Point(12, 444)
        Me.DownloadIfNotExistCheckbox.Name = "DownloadIfNotExistCheckbox"
        Me.DownloadIfNotExistCheckbox.Size = New System.Drawing.Size(188, 17)
        Me.DownloadIfNotExistCheckbox.TabIndex = 2
        Me.DownloadIfNotExistCheckbox.Text = "Download only if file does not exist"
        Me.DownloadIfNotExistCheckbox.UseVisualStyleBackColor = true
        '
        'CheckChecksumsAfterDownloadsCheckbox
        '
        Me.CheckChecksumsAfterDownloadsCheckbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.CheckChecksumsAfterDownloadsCheckbox.AutoSize = true
        Me.CheckChecksumsAfterDownloadsCheckbox.Location = New System.Drawing.Point(206, 444)
        Me.CheckChecksumsAfterDownloadsCheckbox.Name = "CheckChecksumsAfterDownloadsCheckbox"
        Me.CheckChecksumsAfterDownloadsCheckbox.Size = New System.Drawing.Size(211, 17)
        Me.CheckChecksumsAfterDownloadsCheckbox.TabIndex = 3
        Me.CheckChecksumsAfterDownloadsCheckbox.Text = "Check File checksums after downloads"
        Me.CheckChecksumsAfterDownloadsCheckbox.UseVisualStyleBackColor = true
        '
        'Download_URLs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 495)
        Me.Controls.Add(Me.CheckChecksumsAfterDownloadsCheckbox)
        Me.Controls.Add(Me.DownloadIfNotExistCheckbox)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.RichTextBox1)
        Me.Name = "Download_URLs"
        Me.Text = "Download URLs"
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Friend WithEvents RichTextBox1 As RichTextBox
    Friend WithEvents SaveButton As Button
    Friend WithEvents DownloadIfNotExistCheckbox As CheckBox
    Friend WithEvents CheckChecksumsAfterDownloadsCheckbox As CheckBox
End Class
