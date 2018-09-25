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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pathTxt = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBox1.Location = New System.Drawing.Point(12, 12)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(776, 440)
        Me.RichTextBox1.TabIndex = 0
        Me.RichTextBox1.Text = ""
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.Location = New System.Drawing.Point(12, 509)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(775, 23)
        Me.SaveButton.TabIndex = 1
        Me.SaveButton.Text = "Save WGET script"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'DownloadIfNotExistCheckbox
        '
        Me.DownloadIfNotExistCheckbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DownloadIfNotExistCheckbox.AutoSize = True
        Me.DownloadIfNotExistCheckbox.Location = New System.Drawing.Point(12, 458)
        Me.DownloadIfNotExistCheckbox.Name = "DownloadIfNotExistCheckbox"
        Me.DownloadIfNotExistCheckbox.Size = New System.Drawing.Size(188, 17)
        Me.DownloadIfNotExistCheckbox.TabIndex = 2
        Me.DownloadIfNotExistCheckbox.Text = "Download only if file does not exist"
        Me.DownloadIfNotExistCheckbox.UseVisualStyleBackColor = True
        '
        'CheckChecksumsAfterDownloadsCheckbox
        '
        Me.CheckChecksumsAfterDownloadsCheckbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CheckChecksumsAfterDownloadsCheckbox.AutoSize = True
        Me.CheckChecksumsAfterDownloadsCheckbox.Location = New System.Drawing.Point(206, 458)
        Me.CheckChecksumsAfterDownloadsCheckbox.Name = "CheckChecksumsAfterDownloadsCheckbox"
        Me.CheckChecksumsAfterDownloadsCheckbox.Size = New System.Drawing.Size(211, 17)
        Me.CheckChecksumsAfterDownloadsCheckbox.TabIndex = 3
        Me.CheckChecksumsAfterDownloadsCheckbox.Text = "Check File checksums after downloads"
        Me.CheckChecksumsAfterDownloadsCheckbox.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 482)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Save to path:"
        '
        'pathTxt
        '
        Me.pathTxt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pathTxt.Location = New System.Drawing.Point(90, 479)
        Me.pathTxt.Name = "pathTxt"
        Me.pathTxt.Size = New System.Drawing.Size(697, 20)
        Me.pathTxt.TabIndex = 5
        '
        'Download_URLs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 537)
        Me.Controls.Add(Me.pathTxt)
        Me.Controls.Add(Me.Label1)
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
    Friend WithEvents Label1 As Label
    Friend WithEvents pathTxt As TextBox
End Class
