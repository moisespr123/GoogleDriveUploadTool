<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId:="service")>
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.UploadInstructionsLabel = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.UploadsListBox = New System.Windows.Forms.ListBox()
        Me.FolderListBox = New System.Windows.Forms.ListBox()
        Me.FoldersContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenInBrowserToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.DownloadToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MoveToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveChecksumsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FolderLabel = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.SaveChecksumFileButton = New System.Windows.Forms.Button()
        Me.FileSizeTextbox = New System.Windows.Forms.TextBox()
        Me.FileSizeLabel = New System.Windows.Forms.Label()
        Me.MIMETypeTextbox = New System.Windows.Forms.TextBox()
        Me.MIMETypeLabel = New System.Windows.Forms.Label()
        Me.MD5ChecksumTextbox = New System.Windows.Forms.TextBox()
        Me.MD5ChecksumLabel = New System.Windows.Forms.Label()
        Me.DateModifiedTextbox = New System.Windows.Forms.TextBox()
        Me.DateModifiedLabel = New System.Windows.Forms.Label()
        Me.DateCreatedTextbox = New System.Windows.Forms.TextBox()
        Me.DateCreatedLabel = New System.Windows.Forms.Label()
        Me.FileIDTextbox = New System.Windows.Forms.TextBox()
        Me.FileIdLabel = New System.Windows.Forms.Label()
        Me.FileNameTextBox = New System.Windows.Forms.TextBox()
        Me.FileNameLabel = New System.Windows.Forms.Label()
        Me.BackButton = New System.Windows.Forms.Button()
        Me.CreateNewFolderButton = New System.Windows.Forms.Button()
        Me.SaveFileDialog2 = New System.Windows.Forms.SaveFileDialog()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.ViewTrashButton = New System.Windows.Forms.Button()
        Me.UploadToSelectedFolderButton = New System.Windows.Forms.Button()
        Me.CurrentFolderLabel = New System.Windows.Forms.Label()
        Me.GoToRootLink = New System.Windows.Forms.LinkLabel()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UploadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FileToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DownloadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectedFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectedFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ActionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CreateNewFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectedFileToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectedFolderToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.RefreshListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveChecksumsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectedFilesToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectedFolderToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MoveToTrashToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectedFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectedFoldersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestoreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectedFilesToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectedFoldersToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PreserveFileModifiedDateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChecksumsOptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveCheckumsAsChecksumsmd5ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EncodeFileFor = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChecksumsEncodeFormatComboBox = New System.Windows.Forms.ToolStripComboBox()
        Me.OrderByToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OrderByComboBox = New System.Windows.Forms.ToolStripComboBox()
        Me.DescendingOrderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StartUploadsAutomaticallyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SpecifyChunkSizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UploadChunkToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RAMChunkToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DownloadChunkToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyFileToRAMBeforeUploadingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReadmeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DonationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.UploadButton = New System.Windows.Forms.Button()
        Me.FileSizeUploadLabel = New System.Windows.Forms.Label()
        Me.ProcessedFileSizeUploadLabel = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.FileSizeFromCurrentUploadLabel = New System.Windows.Forms.Label()
        Me.ProcessedFileSizeFromCurrentUploadLabel = New System.Windows.Forms.Label()
        Me.GetFolderIdNameButton = New System.Windows.Forms.Button()
        Me.AboutLabel = New System.Windows.Forms.Label()
        Me.FolderNameTextbox = New System.Windows.Forms.TextBox()
        Me.UploadStatusLabel = New System.Windows.Forms.Label()
        Me.FolderNameLabel = New System.Windows.Forms.Label()
        Me.StatusLabel = New System.Windows.Forms.Label()
        Me.UploadPercentLabel = New System.Windows.Forms.Label()
        Me.PercentLabel = New System.Windows.Forms.Label()
        Me.UploadToThisFolderIDLabel = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TChineseRButton = New System.Windows.Forms.RadioButton()
        Me.SpanishRButton = New System.Windows.Forms.RadioButton()
        Me.EnglishRButton = New System.Windows.Forms.RadioButton()
        Me.FolderIDTextBox = New System.Windows.Forms.TextBox()
        Me.TimeRemainingLabel = New System.Windows.Forms.Label()
        Me.UploadTimeLeftLabel = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.DeselectItemFromUploadQueueButton = New System.Windows.Forms.Button()
        Me.RemoveSelectedFilesFromList = New System.Windows.Forms.Button()
        Me.ClearUploadQueueButton = New System.Windows.Forms.Button()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.FileCount = New System.Windows.Forms.Label()
        Me.DownloadFileButton = New System.Windows.Forms.Button()
        Me.RefreshListButton = New System.Windows.Forms.Button()
        Me.SaveSelectedFilesChecksumButton = New System.Windows.Forms.Button()
        Me.FilesListBox = New System.Windows.Forms.ListBox()
        Me.FilesContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenInBrowserToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DownloadToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveChecksumToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VerifyChecksumToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetRawDownloadURLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FilesLabel = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.LoggedInAs = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LoggedUsername = New System.Windows.Forms.ToolStripStatusLabel()
        Me.UsedSpaceText = New System.Windows.Forms.ToolStripStatusLabel()
        Me.UsedSpace = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel7 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.FreeSpaceText = New System.Windows.Forms.ToolStripStatusLabel()
        Me.FreeSpace = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TotalSpaceText = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TotalSpace = New System.Windows.Forms.ToolStripStatusLabel()
        Me.FoldersContextMenu.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.FilesContextMenu.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'UploadInstructionsLabel
        '
        Me.UploadInstructionsLabel.AutoSize = True
        Me.UploadInstructionsLabel.Location = New System.Drawing.Point(3, 0)
        Me.UploadInstructionsLabel.Name = "UploadInstructionsLabel"
        Me.UploadInstructionsLabel.Size = New System.Drawing.Size(205, 13)
        Me.UploadInstructionsLabel.TabIndex = 9
        Me.UploadInstructionsLabel.Text = "Drag and Drop Files to add them to the list"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.Multiselect = True
        '
        'UploadsListBox
        '
        Me.UploadsListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UploadsListBox.FormattingEnabled = True
        Me.UploadsListBox.HorizontalScrollbar = True
        Me.UploadsListBox.Location = New System.Drawing.Point(6, 16)
        Me.UploadsListBox.Name = "UploadsListBox"
        Me.UploadsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.UploadsListBox.Size = New System.Drawing.Size(491, 199)
        Me.UploadsListBox.TabIndex = 28
        '
        'FolderListBox
        '
        Me.FolderListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FolderListBox.ContextMenuStrip = Me.FoldersContextMenu
        Me.FolderListBox.FormattingEnabled = True
        Me.FolderListBox.HorizontalScrollbar = True
        Me.FolderListBox.Location = New System.Drawing.Point(6, 16)
        Me.FolderListBox.Name = "FolderListBox"
        Me.FolderListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.FolderListBox.Size = New System.Drawing.Size(491, 173)
        Me.FolderListBox.TabIndex = 39
        '
        'FoldersContextMenu
        '
        Me.FoldersContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenInBrowserToolStripMenuItem1, Me.DownloadToolStripMenuItem2, Me.MoveToolStripMenuItem1, Me.RenameToolStripMenuItem2, Me.SaveChecksumsToolStripMenuItem1})
        Me.FoldersContextMenu.Name = "FoldersContextMenu"
        Me.FoldersContextMenu.Size = New System.Drawing.Size(171, 114)
        '
        'OpenInBrowserToolStripMenuItem1
        '
        Me.OpenInBrowserToolStripMenuItem1.Name = "OpenInBrowserToolStripMenuItem1"
        Me.OpenInBrowserToolStripMenuItem1.Size = New System.Drawing.Size(170, 22)
        Me.OpenInBrowserToolStripMenuItem1.Text = "Open in Browser"
        '
        'DownloadToolStripMenuItem2
        '
        Me.DownloadToolStripMenuItem2.Name = "DownloadToolStripMenuItem2"
        Me.DownloadToolStripMenuItem2.Size = New System.Drawing.Size(170, 22)
        Me.DownloadToolStripMenuItem2.Text = "Download"
        '
        'MoveToolStripMenuItem1
        '
        Me.MoveToolStripMenuItem1.Name = "MoveToolStripMenuItem1"
        Me.MoveToolStripMenuItem1.Size = New System.Drawing.Size(170, 22)
        Me.MoveToolStripMenuItem1.Text = "Move"
        '
        'RenameToolStripMenuItem2
        '
        Me.RenameToolStripMenuItem2.Name = "RenameToolStripMenuItem2"
        Me.RenameToolStripMenuItem2.Size = New System.Drawing.Size(170, 22)
        Me.RenameToolStripMenuItem2.Text = "Rename"
        '
        'SaveChecksumsToolStripMenuItem1
        '
        Me.SaveChecksumsToolStripMenuItem1.Name = "SaveChecksumsToolStripMenuItem1"
        Me.SaveChecksumsToolStripMenuItem1.Size = New System.Drawing.Size(170, 22)
        Me.SaveChecksumsToolStripMenuItem1.Text = "Save Checksum(s)"
        '
        'FolderLabel
        '
        Me.FolderLabel.AutoSize = True
        Me.FolderLabel.Location = New System.Drawing.Point(3, 0)
        Me.FolderLabel.Name = "FolderLabel"
        Me.FolderLabel.Size = New System.Drawing.Size(44, 13)
        Me.FolderLabel.TabIndex = 38
        Me.FolderLabel.Text = "Folders:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.SaveChecksumFileButton)
        Me.GroupBox2.Controls.Add(Me.FileSizeTextbox)
        Me.GroupBox2.Controls.Add(Me.FileSizeLabel)
        Me.GroupBox2.Controls.Add(Me.MIMETypeTextbox)
        Me.GroupBox2.Controls.Add(Me.MIMETypeLabel)
        Me.GroupBox2.Controls.Add(Me.MD5ChecksumTextbox)
        Me.GroupBox2.Controls.Add(Me.MD5ChecksumLabel)
        Me.GroupBox2.Controls.Add(Me.DateModifiedTextbox)
        Me.GroupBox2.Controls.Add(Me.DateModifiedLabel)
        Me.GroupBox2.Controls.Add(Me.DateCreatedTextbox)
        Me.GroupBox2.Controls.Add(Me.DateCreatedLabel)
        Me.GroupBox2.Controls.Add(Me.FileIDTextbox)
        Me.GroupBox2.Controls.Add(Me.FileIdLabel)
        Me.GroupBox2.Controls.Add(Me.FileNameTextBox)
        Me.GroupBox2.Controls.Add(Me.FileNameLabel)
        Me.GroupBox2.Location = New System.Drawing.Point(1030, 27)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(281, 326)
        Me.GroupBox2.TabIndex = 40
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "File Information:"
        '
        'SaveChecksumFileButton
        '
        Me.SaveChecksumFileButton.Location = New System.Drawing.Point(141, 291)
        Me.SaveChecksumFileButton.Name = "SaveChecksumFileButton"
        Me.SaveChecksumFileButton.Size = New System.Drawing.Size(134, 23)
        Me.SaveChecksumFileButton.TabIndex = 14
        Me.SaveChecksumFileButton.Text = "Save Checksum File"
        Me.SaveChecksumFileButton.UseVisualStyleBackColor = True
        '
        'FileSizeTextbox
        '
        Me.FileSizeTextbox.Location = New System.Drawing.Point(6, 111)
        Me.FileSizeTextbox.Name = "FileSizeTextbox"
        Me.FileSizeTextbox.Size = New System.Drawing.Size(269, 20)
        Me.FileSizeTextbox.TabIndex = 13
        '
        'FileSizeLabel
        '
        Me.FileSizeLabel.AutoSize = True
        Me.FileSizeLabel.Location = New System.Drawing.Point(3, 94)
        Me.FileSizeLabel.Name = "FileSizeLabel"
        Me.FileSizeLabel.Size = New System.Drawing.Size(49, 13)
        Me.FileSizeLabel.TabIndex = 12
        Me.FileSizeLabel.Text = "File Size:"
        '
        'MIMETypeTextbox
        '
        Me.MIMETypeTextbox.Location = New System.Drawing.Point(6, 150)
        Me.MIMETypeTextbox.Name = "MIMETypeTextbox"
        Me.MIMETypeTextbox.Size = New System.Drawing.Size(269, 20)
        Me.MIMETypeTextbox.TabIndex = 11
        '
        'MIMETypeLabel
        '
        Me.MIMETypeLabel.AutoSize = True
        Me.MIMETypeLabel.Location = New System.Drawing.Point(3, 133)
        Me.MIMETypeLabel.Name = "MIMETypeLabel"
        Me.MIMETypeLabel.Size = New System.Drawing.Size(65, 13)
        Me.MIMETypeLabel.TabIndex = 10
        Me.MIMETypeLabel.Text = "MIME Type:"
        '
        'MD5ChecksumTextbox
        '
        Me.MD5ChecksumTextbox.Location = New System.Drawing.Point(6, 265)
        Me.MD5ChecksumTextbox.Name = "MD5ChecksumTextbox"
        Me.MD5ChecksumTextbox.Size = New System.Drawing.Size(269, 20)
        Me.MD5ChecksumTextbox.TabIndex = 9
        '
        'MD5ChecksumLabel
        '
        Me.MD5ChecksumLabel.AutoSize = True
        Me.MD5ChecksumLabel.Location = New System.Drawing.Point(3, 250)
        Me.MD5ChecksumLabel.Name = "MD5ChecksumLabel"
        Me.MD5ChecksumLabel.Size = New System.Drawing.Size(86, 13)
        Me.MD5ChecksumLabel.TabIndex = 8
        Me.MD5ChecksumLabel.Text = "MD5 Checksum:"
        '
        'DateModifiedTextbox
        '
        Me.DateModifiedTextbox.Location = New System.Drawing.Point(6, 228)
        Me.DateModifiedTextbox.Name = "DateModifiedTextbox"
        Me.DateModifiedTextbox.Size = New System.Drawing.Size(269, 20)
        Me.DateModifiedTextbox.TabIndex = 7
        '
        'DateModifiedLabel
        '
        Me.DateModifiedLabel.AutoSize = True
        Me.DateModifiedLabel.Location = New System.Drawing.Point(3, 211)
        Me.DateModifiedLabel.Name = "DateModifiedLabel"
        Me.DateModifiedLabel.Size = New System.Drawing.Size(76, 13)
        Me.DateModifiedLabel.TabIndex = 6
        Me.DateModifiedLabel.Text = "Date Modified:"
        '
        'DateCreatedTextbox
        '
        Me.DateCreatedTextbox.Location = New System.Drawing.Point(6, 189)
        Me.DateCreatedTextbox.Name = "DateCreatedTextbox"
        Me.DateCreatedTextbox.Size = New System.Drawing.Size(269, 20)
        Me.DateCreatedTextbox.TabIndex = 5
        '
        'DateCreatedLabel
        '
        Me.DateCreatedLabel.AutoSize = True
        Me.DateCreatedLabel.Location = New System.Drawing.Point(3, 172)
        Me.DateCreatedLabel.Name = "DateCreatedLabel"
        Me.DateCreatedLabel.Size = New System.Drawing.Size(73, 13)
        Me.DateCreatedLabel.TabIndex = 4
        Me.DateCreatedLabel.Text = "Date Created:"
        '
        'FileIDTextbox
        '
        Me.FileIDTextbox.Location = New System.Drawing.Point(6, 72)
        Me.FileIDTextbox.Name = "FileIDTextbox"
        Me.FileIDTextbox.Size = New System.Drawing.Size(269, 20)
        Me.FileIDTextbox.TabIndex = 3
        '
        'FileIdLabel
        '
        Me.FileIdLabel.AutoSize = True
        Me.FileIdLabel.Location = New System.Drawing.Point(3, 55)
        Me.FileIdLabel.Name = "FileIdLabel"
        Me.FileIdLabel.Size = New System.Drawing.Size(40, 13)
        Me.FileIdLabel.TabIndex = 2
        Me.FileIdLabel.Text = "File ID:"
        '
        'FileNameTextBox
        '
        Me.FileNameTextBox.Location = New System.Drawing.Point(6, 33)
        Me.FileNameTextBox.Name = "FileNameTextBox"
        Me.FileNameTextBox.Size = New System.Drawing.Size(269, 20)
        Me.FileNameTextBox.TabIndex = 1
        '
        'FileNameLabel
        '
        Me.FileNameLabel.AutoSize = True
        Me.FileNameLabel.Location = New System.Drawing.Point(3, 16)
        Me.FileNameLabel.Name = "FileNameLabel"
        Me.FileNameLabel.Size = New System.Drawing.Size(57, 13)
        Me.FileNameLabel.TabIndex = 0
        Me.FileNameLabel.Text = "File Name:"
        '
        'BackButton
        '
        Me.BackButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BackButton.Location = New System.Drawing.Point(6, 192)
        Me.BackButton.Name = "BackButton"
        Me.BackButton.Size = New System.Drawing.Size(75, 23)
        Me.BackButton.TabIndex = 41
        Me.BackButton.Text = "Back"
        Me.BackButton.UseVisualStyleBackColor = True
        '
        'CreateNewFolderButton
        '
        Me.CreateNewFolderButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CreateNewFolderButton.Location = New System.Drawing.Point(87, 192)
        Me.CreateNewFolderButton.Name = "CreateNewFolderButton"
        Me.CreateNewFolderButton.Size = New System.Drawing.Size(108, 23)
        Me.CreateNewFolderButton.TabIndex = 43
        Me.CreateNewFolderButton.Text = "Create new folder"
        Me.CreateNewFolderButton.UseVisualStyleBackColor = True
        '
        'btnLogout
        '
        Me.btnLogout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLogout.Location = New System.Drawing.Point(1198, 456)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(113, 25)
        Me.btnLogout.TabIndex = 45
        Me.btnLogout.Text = "Logout"
        Me.btnLogout.UseVisualStyleBackColor = True
        '
        'ViewTrashButton
        '
        Me.ViewTrashButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ViewTrashButton.Location = New System.Drawing.Point(1030, 456)
        Me.ViewTrashButton.Name = "ViewTrashButton"
        Me.ViewTrashButton.Size = New System.Drawing.Size(101, 25)
        Me.ViewTrashButton.TabIndex = 45
        Me.ViewTrashButton.Text = "View Trash"
        Me.ViewTrashButton.UseVisualStyleBackColor = True
        '
        'UploadToSelectedFolderButton
        '
        Me.UploadToSelectedFolderButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UploadToSelectedFolderButton.Location = New System.Drawing.Point(301, 192)
        Me.UploadToSelectedFolderButton.Name = "UploadToSelectedFolderButton"
        Me.UploadToSelectedFolderButton.Size = New System.Drawing.Size(196, 23)
        Me.UploadToSelectedFolderButton.TabIndex = 46
        Me.UploadToSelectedFolderButton.Text = "Upload selected file(s) to current folder"
        Me.UploadToSelectedFolderButton.UseVisualStyleBackColor = True
        Me.UploadToSelectedFolderButton.Visible = False
        '
        'CurrentFolderLabel
        '
        Me.CurrentFolderLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.CurrentFolderLabel.AutoSize = True
        Me.CurrentFolderLabel.Location = New System.Drawing.Point(108, 0)
        Me.CurrentFolderLabel.Name = "CurrentFolderLabel"
        Me.CurrentFolderLabel.Size = New System.Drawing.Size(96, 13)
        Me.CurrentFolderLabel.TabIndex = 48
        Me.CurrentFolderLabel.Text = "CurrentFolderLabel"
        '
        'GoToRootLink
        '
        Me.GoToRootLink.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GoToRootLink.AutoSize = True
        Me.GoToRootLink.Location = New System.Drawing.Point(430, 0)
        Me.GoToRootLink.Name = "GoToRootLink"
        Me.GoToRootLink.Size = New System.Drawing.Size(59, 13)
        Me.GoToRootLink.TabIndex = 49
        Me.GoToRootLink.TabStop = True
        Me.GoToRootLink.Text = "Go to Root"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ActionsToolStripMenuItem, Me.OptionsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1318, 24)
        Me.MenuStrip1.TabIndex = 52
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UploadToolStripMenuItem, Me.DownloadToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'UploadToolStripMenuItem
        '
        Me.UploadToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem1, Me.FolderToolStripMenuItem})
        Me.UploadToolStripMenuItem.Name = "UploadToolStripMenuItem"
        Me.UploadToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.UploadToolStripMenuItem.Text = "Upload"
        '
        'FileToolStripMenuItem1
        '
        Me.FileToolStripMenuItem1.Name = "FileToolStripMenuItem1"
        Me.FileToolStripMenuItem1.Size = New System.Drawing.Size(107, 22)
        Me.FileToolStripMenuItem1.Text = "File(s)"
        '
        'FolderToolStripMenuItem
        '
        Me.FolderToolStripMenuItem.Name = "FolderToolStripMenuItem"
        Me.FolderToolStripMenuItem.Size = New System.Drawing.Size(107, 22)
        Me.FolderToolStripMenuItem.Text = "Folder"
        '
        'DownloadToolStripMenuItem
        '
        Me.DownloadToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectedFileToolStripMenuItem, Me.SelectedFolderToolStripMenuItem})
        Me.DownloadToolStripMenuItem.Name = "DownloadToolStripMenuItem"
        Me.DownloadToolStripMenuItem.ShowShortcutKeys = False
        Me.DownloadToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.DownloadToolStripMenuItem.Text = "Download"
        '
        'SelectedFileToolStripMenuItem
        '
        Me.SelectedFileToolStripMenuItem.Name = "SelectedFileToolStripMenuItem"
        Me.SelectedFileToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.SelectedFileToolStripMenuItem.Text = "Selected File(s)"
        '
        'SelectedFolderToolStripMenuItem
        '
        Me.SelectedFolderToolStripMenuItem.Name = "SelectedFolderToolStripMenuItem"
        Me.SelectedFolderToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.SelectedFolderToolStripMenuItem.Text = "Selected Folder"
        '
        'ActionsToolStripMenuItem
        '
        Me.ActionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CreateNewFolderToolStripMenuItem, Me.RenameToolStripMenuItem, Me.RefreshListToolStripMenuItem, Me.SaveChecksumsToolStripMenuItem, Me.ToolStripSeparator1, Me.MoveToTrashToolStripMenuItem, Me.RestoreToolStripMenuItem})
        Me.ActionsToolStripMenuItem.Name = "ActionsToolStripMenuItem"
        Me.ActionsToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.ActionsToolStripMenuItem.Text = "Actions"
        '
        'CreateNewFolderToolStripMenuItem
        '
        Me.CreateNewFolderToolStripMenuItem.Name = "CreateNewFolderToolStripMenuItem"
        Me.CreateNewFolderToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.CreateNewFolderToolStripMenuItem.Text = "Create New Folder"
        '
        'RenameToolStripMenuItem
        '
        Me.RenameToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectedFileToolStripMenuItem1, Me.SelectedFolderToolStripMenuItem1})
        Me.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem"
        Me.RenameToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.RenameToolStripMenuItem.Text = "Rename"
        '
        'SelectedFileToolStripMenuItem1
        '
        Me.SelectedFileToolStripMenuItem1.Name = "SelectedFileToolStripMenuItem1"
        Me.SelectedFileToolStripMenuItem1.Size = New System.Drawing.Size(154, 22)
        Me.SelectedFileToolStripMenuItem1.Text = "Selected File"
        '
        'SelectedFolderToolStripMenuItem1
        '
        Me.SelectedFolderToolStripMenuItem1.Name = "SelectedFolderToolStripMenuItem1"
        Me.SelectedFolderToolStripMenuItem1.Size = New System.Drawing.Size(154, 22)
        Me.SelectedFolderToolStripMenuItem1.Text = "Selected Folder"
        '
        'RefreshListToolStripMenuItem
        '
        Me.RefreshListToolStripMenuItem.Name = "RefreshListToolStripMenuItem"
        Me.RefreshListToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.RefreshListToolStripMenuItem.Text = "Refresh List"
        '
        'SaveChecksumsToolStripMenuItem
        '
        Me.SaveChecksumsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectedFilesToolStripMenuItem2, Me.SelectedFolderToolStripMenuItem2})
        Me.SaveChecksumsToolStripMenuItem.Name = "SaveChecksumsToolStripMenuItem"
        Me.SaveChecksumsToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.SaveChecksumsToolStripMenuItem.Text = "Save Checksums"
        '
        'SelectedFilesToolStripMenuItem2
        '
        Me.SelectedFilesToolStripMenuItem2.Name = "SelectedFilesToolStripMenuItem2"
        Me.SelectedFilesToolStripMenuItem2.Size = New System.Drawing.Size(152, 22)
        Me.SelectedFilesToolStripMenuItem2.Text = "Selected file(s)"
        '
        'SelectedFolderToolStripMenuItem2
        '
        Me.SelectedFolderToolStripMenuItem2.Name = "SelectedFolderToolStripMenuItem2"
        Me.SelectedFolderToolStripMenuItem2.Size = New System.Drawing.Size(152, 22)
        Me.SelectedFolderToolStripMenuItem2.Text = "Selected folder"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(168, 6)
        '
        'MoveToTrashToolStripMenuItem
        '
        Me.MoveToTrashToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectedFilesToolStripMenuItem, Me.SelectedFoldersToolStripMenuItem})
        Me.MoveToTrashToolStripMenuItem.Name = "MoveToTrashToolStripMenuItem"
        Me.MoveToTrashToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.MoveToTrashToolStripMenuItem.Text = "Move to Trash"
        '
        'SelectedFilesToolStripMenuItem
        '
        Me.SelectedFilesToolStripMenuItem.Name = "SelectedFilesToolStripMenuItem"
        Me.SelectedFilesToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.SelectedFilesToolStripMenuItem.Text = "Selected file(s)"
        '
        'SelectedFoldersToolStripMenuItem
        '
        Me.SelectedFoldersToolStripMenuItem.Name = "SelectedFoldersToolStripMenuItem"
        Me.SelectedFoldersToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.SelectedFoldersToolStripMenuItem.Text = "Selected folder(s)"
        '
        'RestoreToolStripMenuItem
        '
        Me.RestoreToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectedFilesToolStripMenuItem1, Me.SelectedFoldersToolStripMenuItem1})
        Me.RestoreToolStripMenuItem.Enabled = False
        Me.RestoreToolStripMenuItem.Name = "RestoreToolStripMenuItem"
        Me.RestoreToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.RestoreToolStripMenuItem.Text = "Restore"
        '
        'SelectedFilesToolStripMenuItem1
        '
        Me.SelectedFilesToolStripMenuItem1.Name = "SelectedFilesToolStripMenuItem1"
        Me.SelectedFilesToolStripMenuItem1.Size = New System.Drawing.Size(165, 22)
        Me.SelectedFilesToolStripMenuItem1.Text = "Selected file(s)"
        '
        'SelectedFoldersToolStripMenuItem1
        '
        Me.SelectedFoldersToolStripMenuItem1.Name = "SelectedFoldersToolStripMenuItem1"
        Me.SelectedFoldersToolStripMenuItem1.Size = New System.Drawing.Size(165, 22)
        Me.SelectedFoldersToolStripMenuItem1.Text = "Selected folder(s)"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PreserveFileModifiedDateToolStripMenuItem, Me.ChecksumsOptionsToolStripMenuItem, Me.OrderByToolStripMenuItem, Me.StartUploadsAutomaticallyToolStripMenuItem, Me.UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem, Me.SpecifyChunkSizeToolStripMenuItem, Me.CopyFileToRAMBeforeUploadingToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'PreserveFileModifiedDateToolStripMenuItem
        '
        Me.PreserveFileModifiedDateToolStripMenuItem.CheckOnClick = True
        Me.PreserveFileModifiedDateToolStripMenuItem.Name = "PreserveFileModifiedDateToolStripMenuItem"
        Me.PreserveFileModifiedDateToolStripMenuItem.Size = New System.Drawing.Size(474, 22)
        Me.PreserveFileModifiedDateToolStripMenuItem.Text = "Preserve File Modified Date"
        '
        'ChecksumsOptionsToolStripMenuItem
        '
        Me.ChecksumsOptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveCheckumsAsChecksumsmd5ToolStripMenuItem, Me.EncodeFileFor})
        Me.ChecksumsOptionsToolStripMenuItem.Name = "ChecksumsOptionsToolStripMenuItem"
        Me.ChecksumsOptionsToolStripMenuItem.Size = New System.Drawing.Size(474, 22)
        Me.ChecksumsOptionsToolStripMenuItem.Text = "Checksums Options"
        '
        'SaveCheckumsAsChecksumsmd5ToolStripMenuItem
        '
        Me.SaveCheckumsAsChecksumsmd5ToolStripMenuItem.CheckOnClick = True
        Me.SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Name = "SaveCheckumsAsChecksumsmd5ToolStripMenuItem"
        Me.SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Size = New System.Drawing.Size(263, 22)
        Me.SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Text = "Save checksums as checksums.md5"
        '
        'EncodeFileFor
        '
        Me.EncodeFileFor.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChecksumsEncodeFormatComboBox})
        Me.EncodeFileFor.Name = "EncodeFileFor"
        Me.EncodeFileFor.Size = New System.Drawing.Size(263, 22)
        Me.EncodeFileFor.Text = "Encode file for"
        '
        'ChecksumsEncodeFormatComboBox
        '
        Me.ChecksumsEncodeFormatComboBox.Items.AddRange(New Object() {"Windows", "Mac", "Linux"})
        Me.ChecksumsEncodeFormatComboBox.Name = "ChecksumsEncodeFormatComboBox"
        Me.ChecksumsEncodeFormatComboBox.Size = New System.Drawing.Size(121, 23)
        '
        'OrderByToolStripMenuItem
        '
        Me.OrderByToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OrderByComboBox, Me.DescendingOrderToolStripMenuItem})
        Me.OrderByToolStripMenuItem.Name = "OrderByToolStripMenuItem"
        Me.OrderByToolStripMenuItem.Size = New System.Drawing.Size(474, 22)
        Me.OrderByToolStripMenuItem.Text = "Sort By"
        '
        'OrderByComboBox
        '
        Me.OrderByComboBox.Items.AddRange(New Object() {"Created Time", "Folder", "Modified By Me Time", "Modified Time", "Name", "Natural Name", "Quota Bytes Used", "Recency", "Shared With Me Time", "Starred", "Viewed By Me Time"})
        Me.OrderByComboBox.Name = "OrderByComboBox"
        Me.OrderByComboBox.Size = New System.Drawing.Size(121, 23)
        '
        'DescendingOrderToolStripMenuItem
        '
        Me.DescendingOrderToolStripMenuItem.CheckOnClick = True
        Me.DescendingOrderToolStripMenuItem.Name = "DescendingOrderToolStripMenuItem"
        Me.DescendingOrderToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.DescendingOrderToolStripMenuItem.Text = "Descending Order"
        '
        'StartUploadsAutomaticallyToolStripMenuItem
        '
        Me.StartUploadsAutomaticallyToolStripMenuItem.CheckOnClick = True
        Me.StartUploadsAutomaticallyToolStripMenuItem.Name = "StartUploadsAutomaticallyToolStripMenuItem"
        Me.StartUploadsAutomaticallyToolStripMenuItem.Size = New System.Drawing.Size(474, 22)
        Me.StartUploadsAutomaticallyToolStripMenuItem.Text = "Start Uploads Automatically"
        '
        'UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem
        '
        Me.UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.CheckOnClick = True
        Me.UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Name = "UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem"
        Me.UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Size = New System.Drawing.Size(474, 22)
        Me.UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Text = "Update File and Folder views after an upload finishes"
        '
        'SpecifyChunkSizeToolStripMenuItem
        '
        Me.SpecifyChunkSizeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UploadChunkToolStripMenuItem, Me.RAMChunkToolStripMenuItem, Me.DownloadChunkToolStripMenuItem})
        Me.SpecifyChunkSizeToolStripMenuItem.Name = "SpecifyChunkSizeToolStripMenuItem"
        Me.SpecifyChunkSizeToolStripMenuItem.Size = New System.Drawing.Size(474, 22)
        Me.SpecifyChunkSizeToolStripMenuItem.Text = "Specify Chunk Size"
        '
        'UploadChunkToolStripMenuItem
        '
        Me.UploadChunkToolStripMenuItem.Name = "UploadChunkToolStripMenuItem"
        Me.UploadChunkToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.UploadChunkToolStripMenuItem.Text = "Upload Chunk"
        '
        'RAMChunkToolStripMenuItem
        '
        Me.RAMChunkToolStripMenuItem.Name = "RAMChunkToolStripMenuItem"
        Me.RAMChunkToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.RAMChunkToolStripMenuItem.Text = "RAM Chunk"
        '
        'DownloadChunkToolStripMenuItem
        '
        Me.DownloadChunkToolStripMenuItem.Name = "DownloadChunkToolStripMenuItem"
        Me.DownloadChunkToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.DownloadChunkToolStripMenuItem.Text = "Download Chunk"
        '
        'CopyFileToRAMBeforeUploadingToolStripMenuItem
        '
        Me.CopyFileToRAMBeforeUploadingToolStripMenuItem.CheckOnClick = True
        Me.CopyFileToRAMBeforeUploadingToolStripMenuItem.Name = "CopyFileToRAMBeforeUploadingToolStripMenuItem"
        Me.CopyFileToRAMBeforeUploadingToolStripMenuItem.Size = New System.Drawing.Size(474, 22)
        Me.CopyFileToRAMBeforeUploadingToolStripMenuItem.Text = "Copy File to RAM before uploading if there's enough Free Memory available"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReadmeToolStripMenuItem, Me.DonationsToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.H), System.Windows.Forms.Keys)
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'ReadmeToolStripMenuItem
        '
        Me.ReadmeToolStripMenuItem.Name = "ReadmeToolStripMenuItem"
        Me.ReadmeToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.ReadmeToolStripMenuItem.Text = "Readme / Help"
        '
        'DonationsToolStripMenuItem
        '
        Me.DonationsToolStripMenuItem.Name = "DonationsToolStripMenuItem"
        Me.DonationsToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.DonationsToolStripMenuItem.Text = "Donations"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel3, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 27)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 463.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1024, 463)
        Me.TableLayoutPanel1.TabIndex = 53
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.Panel4, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Panel3, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(506, 457)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.UploadButton)
        Me.Panel4.Controls.Add(Me.FileSizeUploadLabel)
        Me.Panel4.Controls.Add(Me.ProcessedFileSizeUploadLabel)
        Me.Panel4.Controls.Add(Me.ProgressBar1)
        Me.Panel4.Controls.Add(Me.FileSizeFromCurrentUploadLabel)
        Me.Panel4.Controls.Add(Me.ProcessedFileSizeFromCurrentUploadLabel)
        Me.Panel4.Controls.Add(Me.GetFolderIdNameButton)
        Me.Panel4.Controls.Add(Me.AboutLabel)
        Me.Panel4.Controls.Add(Me.FolderNameTextbox)
        Me.Panel4.Controls.Add(Me.UploadStatusLabel)
        Me.Panel4.Controls.Add(Me.FolderNameLabel)
        Me.Panel4.Controls.Add(Me.StatusLabel)
        Me.Panel4.Controls.Add(Me.UploadPercentLabel)
        Me.Panel4.Controls.Add(Me.PercentLabel)
        Me.Panel4.Controls.Add(Me.UploadToThisFolderIDLabel)
        Me.Panel4.Controls.Add(Me.GroupBox1)
        Me.Panel4.Controls.Add(Me.FolderIDTextBox)
        Me.Panel4.Controls.Add(Me.TimeRemainingLabel)
        Me.Panel4.Controls.Add(Me.UploadTimeLeftLabel)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(3, 260)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(500, 194)
        Me.Panel4.TabIndex = 55
        '
        'UploadButton
        '
        Me.UploadButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UploadButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UploadButton.Location = New System.Drawing.Point(261, 24)
        Me.UploadButton.Name = "UploadButton"
        Me.UploadButton.Size = New System.Drawing.Size(227, 39)
        Me.UploadButton.TabIndex = 2
        Me.UploadButton.Text = "Upload"
        Me.UploadButton.UseVisualStyleBackColor = True
        '
        'FileSizeUploadLabel
        '
        Me.FileSizeUploadLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FileSizeUploadLabel.AutoSize = True
        Me.FileSizeUploadLabel.Location = New System.Drawing.Point(11, 98)
        Me.FileSizeUploadLabel.Name = "FileSizeUploadLabel"
        Me.FileSizeUploadLabel.Size = New System.Drawing.Size(49, 13)
        Me.FileSizeUploadLabel.TabIndex = 3
        Me.FileSizeUploadLabel.Text = "File Size:"
        '
        'ProcessedFileSizeUploadLabel
        '
        Me.ProcessedFileSizeUploadLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ProcessedFileSizeUploadLabel.AutoSize = True
        Me.ProcessedFileSizeUploadLabel.Location = New System.Drawing.Point(11, 111)
        Me.ProcessedFileSizeUploadLabel.Name = "ProcessedFileSizeUploadLabel"
        Me.ProcessedFileSizeUploadLabel.Size = New System.Drawing.Size(60, 13)
        Me.ProcessedFileSizeUploadLabel.TabIndex = 4
        Me.ProcessedFileSizeUploadLabel.Text = "Processed:"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(14, 127)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(463, 23)
        Me.ProgressBar1.TabIndex = 5
        '
        'FileSizeFromCurrentUploadLabel
        '
        Me.FileSizeFromCurrentUploadLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FileSizeFromCurrentUploadLabel.AutoSize = True
        Me.FileSizeFromCurrentUploadLabel.Location = New System.Drawing.Point(80, 98)
        Me.FileSizeFromCurrentUploadLabel.Name = "FileSizeFromCurrentUploadLabel"
        Me.FileSizeFromCurrentUploadLabel.Size = New System.Drawing.Size(27, 13)
        Me.FileSizeFromCurrentUploadLabel.TabIndex = 6
        Me.FileSizeFromCurrentUploadLabel.Text = "N/A"
        '
        'ProcessedFileSizeFromCurrentUploadLabel
        '
        Me.ProcessedFileSizeFromCurrentUploadLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ProcessedFileSizeFromCurrentUploadLabel.AutoSize = True
        Me.ProcessedFileSizeFromCurrentUploadLabel.Location = New System.Drawing.Point(80, 111)
        Me.ProcessedFileSizeFromCurrentUploadLabel.Name = "ProcessedFileSizeFromCurrentUploadLabel"
        Me.ProcessedFileSizeFromCurrentUploadLabel.Size = New System.Drawing.Size(27, 13)
        Me.ProcessedFileSizeFromCurrentUploadLabel.TabIndex = 7
        Me.ProcessedFileSizeFromCurrentUploadLabel.Text = "N/A"
        '
        'GetFolderIdNameButton
        '
        Me.GetFolderIdNameButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GetFolderIdNameButton.Location = New System.Drawing.Point(261, 69)
        Me.GetFolderIdNameButton.Name = "GetFolderIdNameButton"
        Me.GetFolderIdNameButton.Size = New System.Drawing.Size(162, 23)
        Me.GetFolderIdNameButton.TabIndex = 37
        Me.GetFolderIdNameButton.Text = "Get Folder Name" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Obtener Nombre de la carpeta"
        Me.GetFolderIdNameButton.UseVisualStyleBackColor = True
        '
        'AboutLabel
        '
        Me.AboutLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AboutLabel.AutoSize = True
        Me.AboutLabel.Location = New System.Drawing.Point(11, 153)
        Me.AboutLabel.Name = "AboutLabel"
        Me.AboutLabel.Size = New System.Drawing.Size(98, 26)
        Me.AboutLabel.TabIndex = 11
        Me.AboutLabel.Text = "By Moisés Cardona" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "v1.10"
        '
        'FolderNameTextbox
        '
        Me.FolderNameTextbox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FolderNameTextbox.Location = New System.Drawing.Point(14, 72)
        Me.FolderNameTextbox.Name = "FolderNameTextbox"
        Me.FolderNameTextbox.ReadOnly = True
        Me.FolderNameTextbox.Size = New System.Drawing.Size(241, 20)
        Me.FolderNameTextbox.TabIndex = 36
        '
        'UploadStatusLabel
        '
        Me.UploadStatusLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UploadStatusLabel.AutoSize = True
        Me.UploadStatusLabel.Location = New System.Drawing.Point(336, 111)
        Me.UploadStatusLabel.Name = "UploadStatusLabel"
        Me.UploadStatusLabel.Size = New System.Drawing.Size(40, 13)
        Me.UploadStatusLabel.TabIndex = 12
        Me.UploadStatusLabel.Text = "Status:"
        '
        'FolderNameLabel
        '
        Me.FolderNameLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FolderNameLabel.AutoSize = True
        Me.FolderNameLabel.Location = New System.Drawing.Point(11, 56)
        Me.FolderNameLabel.Name = "FolderNameLabel"
        Me.FolderNameLabel.Size = New System.Drawing.Size(70, 13)
        Me.FolderNameLabel.TabIndex = 35
        Me.FolderNameLabel.Text = "Folder Name:"
        '
        'StatusLabel
        '
        Me.StatusLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StatusLabel.AutoSize = True
        Me.StatusLabel.Location = New System.Drawing.Point(399, 111)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(27, 13)
        Me.StatusLabel.TabIndex = 13
        Me.StatusLabel.Text = "N/A"
        '
        'UploadPercentLabel
        '
        Me.UploadPercentLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UploadPercentLabel.AutoSize = True
        Me.UploadPercentLabel.Location = New System.Drawing.Point(337, 98)
        Me.UploadPercentLabel.Name = "UploadPercentLabel"
        Me.UploadPercentLabel.Size = New System.Drawing.Size(44, 13)
        Me.UploadPercentLabel.TabIndex = 14
        Me.UploadPercentLabel.Text = "Percent"
        '
        'PercentLabel
        '
        Me.PercentLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PercentLabel.AutoSize = True
        Me.PercentLabel.Location = New System.Drawing.Point(399, 98)
        Me.PercentLabel.Name = "PercentLabel"
        Me.PercentLabel.Size = New System.Drawing.Size(21, 13)
        Me.PercentLabel.TabIndex = 15
        Me.PercentLabel.Text = "0%"
        '
        'UploadToThisFolderIDLabel
        '
        Me.UploadToThisFolderIDLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.UploadToThisFolderIDLabel.AutoSize = True
        Me.UploadToThisFolderIDLabel.Location = New System.Drawing.Point(11, 10)
        Me.UploadToThisFolderIDLabel.Name = "UploadToThisFolderIDLabel"
        Me.UploadToThisFolderIDLabel.Size = New System.Drawing.Size(264, 13)
        Me.UploadToThisFolderIDLabel.TabIndex = 23
        Me.UploadToThisFolderIDLabel.Text = "Upload to this folder ID (""root"" to upload to root folder):"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.TChineseRButton)
        Me.GroupBox1.Controls.Add(Me.SpanishRButton)
        Me.GroupBox1.Controls.Add(Me.EnglishRButton)
        Me.GroupBox1.Location = New System.Drawing.Point(272, 153)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(216, 37)
        Me.GroupBox1.TabIndex = 27
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Language / Idioma"
        '
        'TChineseRButton
        '
        Me.TChineseRButton.AutoSize = True
        Me.TChineseRButton.Location = New System.Drawing.Point(137, 14)
        Me.TChineseRButton.Name = "TChineseRButton"
        Me.TChineseRButton.Size = New System.Drawing.Size(73, 17)
        Me.TChineseRButton.TabIndex = 2
        Me.TChineseRButton.TabStop = True
        Me.TChineseRButton.Text = "正體中文"
        Me.TChineseRButton.UseVisualStyleBackColor = True
        '
        'SpanishRButton
        '
        Me.SpanishRButton.AutoSize = True
        Me.SpanishRButton.Location = New System.Drawing.Point(71, 14)
        Me.SpanishRButton.Name = "SpanishRButton"
        Me.SpanishRButton.Size = New System.Drawing.Size(63, 17)
        Me.SpanishRButton.TabIndex = 1
        Me.SpanishRButton.Text = "Spanish"
        Me.SpanishRButton.UseVisualStyleBackColor = True
        '
        'EnglishRButton
        '
        Me.EnglishRButton.AutoSize = True
        Me.EnglishRButton.Location = New System.Drawing.Point(6, 14)
        Me.EnglishRButton.Name = "EnglishRButton"
        Me.EnglishRButton.Size = New System.Drawing.Size(59, 17)
        Me.EnglishRButton.TabIndex = 0
        Me.EnglishRButton.Text = "English"
        Me.EnglishRButton.UseVisualStyleBackColor = True
        '
        'FolderIDTextBox
        '
        Me.FolderIDTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FolderIDTextBox.Location = New System.Drawing.Point(14, 26)
        Me.FolderIDTextBox.Name = "FolderIDTextBox"
        Me.FolderIDTextBox.Size = New System.Drawing.Size(241, 20)
        Me.FolderIDTextBox.TabIndex = 24
        '
        'TimeRemainingLabel
        '
        Me.TimeRemainingLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.TimeRemainingLabel.AutoSize = True
        Me.TimeRemainingLabel.Location = New System.Drawing.Point(227, 111)
        Me.TimeRemainingLabel.Name = "TimeRemainingLabel"
        Me.TimeRemainingLabel.Size = New System.Drawing.Size(49, 13)
        Me.TimeRemainingLabel.TabIndex = 26
        Me.TimeRemainingLabel.Text = "00:00:00"
        '
        'UploadTimeLeftLabel
        '
        Me.UploadTimeLeftLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.UploadTimeLeftLabel.AutoSize = True
        Me.UploadTimeLeftLabel.Location = New System.Drawing.Point(164, 111)
        Me.UploadTimeLeftLabel.Name = "UploadTimeLeftLabel"
        Me.UploadTimeLeftLabel.Size = New System.Drawing.Size(57, 13)
        Me.UploadTimeLeftLabel.TabIndex = 25
        Me.UploadTimeLeftLabel.Text = "Time Left: "
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.DeselectItemFromUploadQueueButton)
        Me.Panel3.Controls.Add(Me.UploadsListBox)
        Me.Panel3.Controls.Add(Me.RemoveSelectedFilesFromList)
        Me.Panel3.Controls.Add(Me.UploadInstructionsLabel)
        Me.Panel3.Controls.Add(Me.ClearUploadQueueButton)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(3, 3)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(500, 251)
        Me.Panel3.TabIndex = 2
        '
        'DeselectItemFromUploadQueueButton
        '
        Me.DeselectItemFromUploadQueueButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DeselectItemFromUploadQueueButton.Enabled = False
        Me.DeselectItemFromUploadQueueButton.Location = New System.Drawing.Point(261, 218)
        Me.DeselectItemFromUploadQueueButton.Name = "DeselectItemFromUploadQueueButton"
        Me.DeselectItemFromUploadQueueButton.Size = New System.Drawing.Size(89, 23)
        Me.DeselectItemFromUploadQueueButton.TabIndex = 47
        Me.DeselectItemFromUploadQueueButton.Text = "Deselect"
        Me.DeselectItemFromUploadQueueButton.UseVisualStyleBackColor = True
        '
        'RemoveSelectedFilesFromList
        '
        Me.RemoveSelectedFilesFromList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RemoveSelectedFilesFromList.Location = New System.Drawing.Point(6, 218)
        Me.RemoveSelectedFilesFromList.Name = "RemoveSelectedFilesFromList"
        Me.RemoveSelectedFilesFromList.Size = New System.Drawing.Size(168, 23)
        Me.RemoveSelectedFilesFromList.TabIndex = 29
        Me.RemoveSelectedFilesFromList.Text = "Remove selected file(s) from list"
        Me.RemoveSelectedFilesFromList.UseVisualStyleBackColor = True
        '
        'ClearUploadQueueButton
        '
        Me.ClearUploadQueueButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ClearUploadQueueButton.Location = New System.Drawing.Point(180, 218)
        Me.ClearUploadQueueButton.Name = "ClearUploadQueueButton"
        Me.ClearUploadQueueButton.Size = New System.Drawing.Size(75, 23)
        Me.ClearUploadQueueButton.TabIndex = 31
        Me.ClearUploadQueueButton.Text = "Clear List"
        Me.ClearUploadQueueButton.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 1
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.Panel2, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.Panel1, 0, 1)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(515, 3)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 2
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(506, 457)
        Me.TableLayoutPanel3.TabIndex = 1
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.UploadToSelectedFolderButton)
        Me.Panel2.Controls.Add(Me.GoToRootLink)
        Me.Panel2.Controls.Add(Me.FolderListBox)
        Me.Panel2.Controls.Add(Me.CurrentFolderLabel)
        Me.Panel2.Controls.Add(Me.CreateNewFolderButton)
        Me.Panel2.Controls.Add(Me.FolderLabel)
        Me.Panel2.Controls.Add(Me.BackButton)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(500, 222)
        Me.Panel2.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.FileCount)
        Me.Panel1.Controls.Add(Me.DownloadFileButton)
        Me.Panel1.Controls.Add(Me.RefreshListButton)
        Me.Panel1.Controls.Add(Me.SaveSelectedFilesChecksumButton)
        Me.Panel1.Controls.Add(Me.FilesListBox)
        Me.Panel1.Controls.Add(Me.FilesLabel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 231)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(500, 223)
        Me.Panel1.TabIndex = 0
        '
        'FileCount
        '
        Me.FileCount.AutoSize = True
        Me.FileCount.Location = New System.Drawing.Point(108, 1)
        Me.FileCount.Name = "FileCount"
        Me.FileCount.Size = New System.Drawing.Size(37, 13)
        Me.FileCount.TabIndex = 45
        Me.FileCount.Text = "0 Files"
        '
        'DownloadFileButton
        '
        Me.DownloadFileButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DownloadFileButton.Location = New System.Drawing.Point(6, 193)
        Me.DownloadFileButton.Name = "DownloadFileButton"
        Me.DownloadFileButton.Size = New System.Drawing.Size(92, 23)
        Me.DownloadFileButton.TabIndex = 21
        Me.DownloadFileButton.Text = "Download File"
        Me.DownloadFileButton.UseVisualStyleBackColor = True
        '
        'RefreshListButton
        '
        Me.RefreshListButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RefreshListButton.Location = New System.Drawing.Point(104, 193)
        Me.RefreshListButton.Name = "RefreshListButton"
        Me.RefreshListButton.Size = New System.Drawing.Size(91, 23)
        Me.RefreshListButton.TabIndex = 22
        Me.RefreshListButton.Text = "Refresh List"
        Me.RefreshListButton.UseVisualStyleBackColor = True
        '
        'SaveSelectedFilesChecksumButton
        '
        Me.SaveSelectedFilesChecksumButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SaveSelectedFilesChecksumButton.Location = New System.Drawing.Point(198, 194)
        Me.SaveSelectedFilesChecksumButton.Name = "SaveSelectedFilesChecksumButton"
        Me.SaveSelectedFilesChecksumButton.Size = New System.Drawing.Size(196, 23)
        Me.SaveSelectedFilesChecksumButton.TabIndex = 44
        Me.SaveSelectedFilesChecksumButton.Text = "Save Checksums for Selected Files"
        Me.SaveSelectedFilesChecksumButton.UseVisualStyleBackColor = True
        Me.SaveSelectedFilesChecksumButton.Visible = False
        '
        'FilesListBox
        '
        Me.FilesListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FilesListBox.ContextMenuStrip = Me.FilesContextMenu
        Me.FilesListBox.FormattingEnabled = True
        Me.FilesListBox.HorizontalScrollbar = True
        Me.FilesListBox.Location = New System.Drawing.Point(6, 16)
        Me.FilesListBox.Name = "FilesListBox"
        Me.FilesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.FilesListBox.Size = New System.Drawing.Size(491, 173)
        Me.FilesListBox.TabIndex = 20
        '
        'FilesContextMenu
        '
        Me.FilesContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenInBrowserToolStripMenuItem, Me.DownloadToolStripMenuItem1, Me.MoveToolStripMenuItem, Me.RenameToolStripMenuItem1, Me.SaveChecksumToolStripMenuItem, Me.VerifyChecksumToolStripMenuItem, Me.GetRawDownloadURLToolStripMenuItem})
        Me.FilesContextMenu.Name = "FilesContextMenu"
        Me.FilesContextMenu.Size = New System.Drawing.Size(199, 158)
        '
        'OpenInBrowserToolStripMenuItem
        '
        Me.OpenInBrowserToolStripMenuItem.Name = "OpenInBrowserToolStripMenuItem"
        Me.OpenInBrowserToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.OpenInBrowserToolStripMenuItem.Text = "Open in Browser"
        '
        'DownloadToolStripMenuItem1
        '
        Me.DownloadToolStripMenuItem1.Name = "DownloadToolStripMenuItem1"
        Me.DownloadToolStripMenuItem1.Size = New System.Drawing.Size(198, 22)
        Me.DownloadToolStripMenuItem1.Text = "Download"
        '
        'MoveToolStripMenuItem
        '
        Me.MoveToolStripMenuItem.Name = "MoveToolStripMenuItem"
        Me.MoveToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.MoveToolStripMenuItem.Text = "Move"
        '
        'RenameToolStripMenuItem1
        '
        Me.RenameToolStripMenuItem1.Name = "RenameToolStripMenuItem1"
        Me.RenameToolStripMenuItem1.Size = New System.Drawing.Size(198, 22)
        Me.RenameToolStripMenuItem1.Text = "Rename"
        '
        'SaveChecksumToolStripMenuItem
        '
        Me.SaveChecksumToolStripMenuItem.Name = "SaveChecksumToolStripMenuItem"
        Me.SaveChecksumToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.SaveChecksumToolStripMenuItem.Text = "Save Checksum(s)"
        '
        'VerifyChecksumToolStripMenuItem
        '
        Me.VerifyChecksumToolStripMenuItem.Name = "VerifyChecksumToolStripMenuItem"
        Me.VerifyChecksumToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.VerifyChecksumToolStripMenuItem.Text = "Verify Checksum"
        '
        'GetRawDownloadURLToolStripMenuItem
        '
        Me.GetRawDownloadURLToolStripMenuItem.Name = "GetRawDownloadURLToolStripMenuItem"
        Me.GetRawDownloadURLToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.GetRawDownloadURLToolStripMenuItem.Text = "Get Raw Download URL"
        '
        'FilesLabel
        '
        Me.FilesLabel.AutoSize = True
        Me.FilesLabel.Location = New System.Drawing.Point(3, 0)
        Me.FilesLabel.Name = "FilesLabel"
        Me.FilesLabel.Size = New System.Drawing.Size(31, 13)
        Me.FilesLabel.TabIndex = 17
        Me.FilesLabel.Text = "Files:"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoggedInAs, Me.LoggedUsername, Me.UsedSpaceText, Me.UsedSpace, Me.ToolStripStatusLabel7, Me.FreeSpaceText, Me.FreeSpace, Me.TotalSpaceText, Me.TotalSpace})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 493)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1318, 22)
        Me.StatusStrip1.TabIndex = 54
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'LoggedInAs
        '
        Me.LoggedInAs.Name = "LoggedInAs"
        Me.LoggedInAs.Size = New System.Drawing.Size(79, 17)
        Me.LoggedInAs.Text = "Logged In As:"
        '
        'LoggedUsername
        '
        Me.LoggedUsername.Name = "LoggedUsername"
        Me.LoggedUsername.Size = New System.Drawing.Size(60, 17)
        Me.LoggedUsername.Text = "Username"
        '
        'UsedSpaceText
        '
        Me.UsedSpaceText.Name = "UsedSpaceText"
        Me.UsedSpaceText.Size = New System.Drawing.Size(70, 17)
        Me.UsedSpaceText.Text = "Used Space:"
        '
        'UsedSpace
        '
        Me.UsedSpace.Name = "UsedSpace"
        Me.UsedSpace.Size = New System.Drawing.Size(43, 17)
        Me.UsedSpace.Text = "0.0 MB"
        '
        'ToolStripStatusLabel7
        '
        Me.ToolStripStatusLabel7.Name = "ToolStripStatusLabel7"
        Me.ToolStripStatusLabel7.Size = New System.Drawing.Size(0, 17)
        '
        'FreeSpaceText
        '
        Me.FreeSpaceText.Name = "FreeSpaceText"
        Me.FreeSpaceText.Size = New System.Drawing.Size(66, 17)
        Me.FreeSpaceText.Text = "Free Space:"
        '
        'FreeSpace
        '
        Me.FreeSpace.Name = "FreeSpace"
        Me.FreeSpace.Size = New System.Drawing.Size(43, 17)
        Me.FreeSpace.Text = "0.0 MB"
        '
        'TotalSpaceText
        '
        Me.TotalSpaceText.Name = "TotalSpaceText"
        Me.TotalSpaceText.Size = New System.Drawing.Size(69, 17)
        Me.TotalSpaceText.Text = "Total Space:"
        '
        'TotalSpace
        '
        Me.TotalSpace.Name = "TotalSpace"
        Me.TotalSpace.Size = New System.Drawing.Size(43, 17)
        Me.TotalSpace.Text = "0.0 MB"
        '
        'Form1
        '
        Me.AllowDrop = true
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1318, 515)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.btnLogout)
        Me.Controls.Add(Me.ViewTrashButton)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.KeyPreview = true
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Google Drive Uploader & Explorer"
        Me.FoldersContextMenu.ResumeLayout(false)
        Me.GroupBox2.ResumeLayout(false)
        Me.GroupBox2.PerformLayout
        Me.MenuStrip1.ResumeLayout(false)
        Me.MenuStrip1.PerformLayout
        Me.TableLayoutPanel1.ResumeLayout(false)
        Me.TableLayoutPanel2.ResumeLayout(false)
        Me.Panel4.ResumeLayout(false)
        Me.Panel4.PerformLayout
        Me.GroupBox1.ResumeLayout(false)
        Me.GroupBox1.PerformLayout
        Me.Panel3.ResumeLayout(false)
        Me.Panel3.PerformLayout
        Me.TableLayoutPanel3.ResumeLayout(false)
        Me.Panel2.ResumeLayout(false)
        Me.Panel2.PerformLayout
        Me.Panel1.ResumeLayout(false)
        Me.Panel1.PerformLayout
        Me.FilesContextMenu.ResumeLayout(false)
        Me.StatusStrip1.ResumeLayout(false)
        Me.StatusStrip1.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents UploadInstructionsLabel As Label
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents UploadsListBox As ListBox
    Friend WithEvents FolderListBox As ListBox
    Friend WithEvents FolderLabel As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents FileIDTextbox As TextBox
    Friend WithEvents FileIdLabel As Label
    Friend WithEvents FileNameTextBox As TextBox
    Friend WithEvents FileNameLabel As Label
    Friend WithEvents MD5ChecksumTextbox As TextBox
    Friend WithEvents MD5ChecksumLabel As Label
    Friend WithEvents DateModifiedTextbox As TextBox
    Friend WithEvents DateModifiedLabel As Label
    Friend WithEvents DateCreatedTextbox As TextBox
    Friend WithEvents DateCreatedLabel As Label
    Friend WithEvents BackButton As Button
    Friend WithEvents CreateNewFolderButton As Button
    Friend WithEvents MIMETypeTextbox As TextBox
    Friend WithEvents MIMETypeLabel As Label
    Friend WithEvents FileSizeTextbox As TextBox
    Friend WithEvents FileSizeLabel As Label
    Friend WithEvents SaveChecksumFileButton As Button
    Friend WithEvents SaveFileDialog2 As SaveFileDialog
    Friend WithEvents btnLogout As Button
    Friend WithEvents ViewTrashButton As Button
    Friend WithEvents UploadToSelectedFolderButton As Button
    Friend WithEvents CurrentFolderLabel As Label
    Friend WithEvents GoToRootLink As LinkLabel
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UploadToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FileToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents FolderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DownloadToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectedFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectedFolderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PreserveFileModifiedDateToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StartUploadsAutomaticallyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveCheckumsAsChecksumsmd5ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReadmeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DonationsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OrderByToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OrderByComboBox As ToolStripComboBox
    Friend WithEvents DescendingOrderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SpecifyChunkSizeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyFileToRAMBeforeUploadingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ActionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CreateNewFolderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MoveToTrashToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectedFilesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectedFoldersToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RestoreToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectedFilesToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents SelectedFoldersToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents RenameToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectedFileToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents SelectedFolderToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents RefreshListToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents SaveChecksumsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectedFilesToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents SelectedFolderToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents UsedSpaceText As ToolStripStatusLabel
    Friend WithEvents UsedSpace As ToolStripStatusLabel
    Friend WithEvents TotalSpaceText As ToolStripStatusLabel
    Friend WithEvents TotalSpace As ToolStripStatusLabel
    Friend WithEvents LoggedInAs As ToolStripStatusLabel
    Friend WithEvents LoggedUsername As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel7 As ToolStripStatusLabel
    Friend WithEvents FreeSpaceText As ToolStripStatusLabel
    Friend WithEvents FreeSpace As ToolStripStatusLabel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents DeselectItemFromUploadQueueButton As Button
    Friend WithEvents RemoveSelectedFilesFromList As Button
    Friend WithEvents UploadButton As Button
    Friend WithEvents FileSizeUploadLabel As Label
    Friend WithEvents ProcessedFileSizeUploadLabel As Label
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents FileSizeFromCurrentUploadLabel As Label
    Friend WithEvents ProcessedFileSizeFromCurrentUploadLabel As Label
    Friend WithEvents GetFolderIdNameButton As Button
    Friend WithEvents AboutLabel As Label
    Friend WithEvents FolderNameTextbox As TextBox
    Friend WithEvents UploadStatusLabel As Label
    Friend WithEvents FolderNameLabel As Label
    Friend WithEvents StatusLabel As Label
    Friend WithEvents ClearUploadQueueButton As Button
    Friend WithEvents UploadPercentLabel As Label
    Friend WithEvents PercentLabel As Label
    Friend WithEvents UploadToThisFolderIDLabel As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents TChineseRButton As RadioButton
    Friend WithEvents SpanishRButton As RadioButton
    Friend WithEvents EnglishRButton As RadioButton
    Friend WithEvents FolderIDTextBox As TextBox
    Friend WithEvents TimeRemainingLabel As Label
    Friend WithEvents UploadTimeLeftLabel As Label
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents DownloadFileButton As Button
    Friend WithEvents RefreshListButton As Button
    Friend WithEvents SaveSelectedFilesChecksumButton As Button
    Friend WithEvents FilesListBox As ListBox
    Friend WithEvents FilesLabel As Label
    Friend WithEvents ChecksumsOptionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EncodeFileFor As ToolStripMenuItem
    Friend WithEvents ChecksumsEncodeFormatComboBox As ToolStripComboBox
    Friend WithEvents FileCount As Label
    Friend WithEvents FilesContextMenu As ContextMenuStrip
    Friend WithEvents OpenInBrowserToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DownloadToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents SaveChecksumToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GetRawDownloadURLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MoveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FoldersContextMenu As ContextMenuStrip
    Friend WithEvents OpenInBrowserToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents DownloadToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents MoveToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents SaveChecksumsToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents UploadChunkToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RAMChunkToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RenameToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents RenameToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents DownloadChunkToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VerifyChecksumToolStripMenuItem As ToolStripMenuItem
End Class
