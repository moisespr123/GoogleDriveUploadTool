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
        Me.AuthorizationHeaderLabel = New System.Windows.Forms.Label()
        Me.AuthorizationHeaderTextBox = New System.Windows.Forms.TextBox()
        Me.wget = New System.Windows.Forms.RadioButton()
        Me.curl = New System.Windows.Forms.RadioButton()
        Me.SuspendLayout()
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBox1.Location = New System.Drawing.Point(12, 12)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.ReadOnly = True
        Me.RichTextBox1.Size = New System.Drawing.Size(776, 433)
        Me.RichTextBox1.TabIndex = 0
        Me.RichTextBox1.Text = ""
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.Location = New System.Drawing.Point(113, 524)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(674, 23)
        Me.SaveButton.TabIndex = 1
        Me.SaveButton.Text = "Save script"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'DownloadIfNotExistCheckbox
        '
        Me.DownloadIfNotExistCheckbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DownloadIfNotExistCheckbox.AutoSize = True
        Me.DownloadIfNotExistCheckbox.Location = New System.Drawing.Point(12, 477)
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
        Me.CheckChecksumsAfterDownloadsCheckbox.Location = New System.Drawing.Point(206, 477)
        Me.CheckChecksumsAfterDownloadsCheckbox.Name = "CheckChecksumsAfterDownloadsCheckbox"
        Me.CheckChecksumsAfterDownloadsCheckbox.Size = New System.Drawing.Size(211, 17)
        Me.CheckChecksumsAfterDownloadsCheckbox.TabIndex = 3
        Me.CheckChecksumsAfterDownloadsCheckbox.Text = "Check File checksums after downloads"
        Me.CheckChecksumsAfterDownloadsCheckbox.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 501)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Save to path:"
        '
        'pathTxt
        '
        Me.pathTxt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pathTxt.Location = New System.Drawing.Point(90, 498)
        Me.pathTxt.Name = "pathTxt"
        Me.pathTxt.Size = New System.Drawing.Size(697, 20)
        Me.pathTxt.TabIndex = 5
        Me.pathTxt.Text = "."
        '
        'AuthorizationHeaderLabel
        '
        Me.AuthorizationHeaderLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AuthorizationHeaderLabel.AutoSize = True
        Me.AuthorizationHeaderLabel.Location = New System.Drawing.Point(9, 454)
        Me.AuthorizationHeaderLabel.Name = "AuthorizationHeaderLabel"
        Me.AuthorizationHeaderLabel.Size = New System.Drawing.Size(109, 13)
        Me.AuthorizationHeaderLabel.TabIndex = 6
        Me.AuthorizationHeaderLabel.Text = "Authorization Header:"
        '
        'AuthorizationHeaderTextBox
        '
        Me.AuthorizationHeaderTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AuthorizationHeaderTextBox.Location = New System.Drawing.Point(127, 451)
        Me.AuthorizationHeaderTextBox.Name = "AuthorizationHeaderTextBox"
        Me.AuthorizationHeaderTextBox.ReadOnly = True
        Me.AuthorizationHeaderTextBox.Size = New System.Drawing.Size(660, 20)
        Me.AuthorizationHeaderTextBox.TabIndex = 7
        '
        'wget
        '
        Me.wget.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.wget.AutoSize = True
        Me.wget.Checked = True
        Me.wget.Location = New System.Drawing.Point(16, 527)
        Me.wget.Name = "wget"
        Me.wget.Size = New System.Drawing.Size(48, 17)
        Me.wget.TabIndex = 8
        Me.wget.TabStop = True
        Me.wget.Text = "wget"
        Me.wget.UseVisualStyleBackColor = True
        '
        'curl
        '
        Me.curl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.curl.AutoSize = True
        Me.curl.Location = New System.Drawing.Point(65, 527)
        Me.curl.Name = "curl"
        Me.curl.Size = New System.Drawing.Size(42, 17)
        Me.curl.TabIndex = 9
        Me.curl.TabStop = True
        Me.curl.Text = "curl"
        Me.curl.UseVisualStyleBackColor = True
        '
        'Download_URLs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 556)
        Me.Controls.Add(Me.curl)
        Me.Controls.Add(Me.wget)
        Me.Controls.Add(Me.AuthorizationHeaderTextBox)
        Me.Controls.Add(Me.AuthorizationHeaderLabel)
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
    Friend WithEvents AuthorizationHeaderLabel As Label
    Friend WithEvents AuthorizationHeaderTextBox As TextBox
    Friend WithEvents wget As RadioButton
    Friend WithEvents curl As RadioButton
End Class
