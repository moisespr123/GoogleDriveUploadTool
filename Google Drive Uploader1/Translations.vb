Public Class Translations
    Public Shared Sub EnglishLanguage()
        Form1.FileSizeUploadLabel.Text = "File Size:"
        Form1.ProcessedFileSizeUploadLabel.Text = "Processed:"
        Form1.UploadInstructionsLabel.Text = "Drag and Drop Files to add them to the list"
        Form1.AboutLabel.Text = "By Moisés Cardona" & vbNewLine & "v1.8.6"
        Form1.UploadStatusLabel.Text = "Status:"
        Form1.UploadPercentLabel.Text = "Percent: "
        Form1.FilesLabel.Text = "Files:"
        Form1.UploadToThisFolderIDLabel.Text = "Upload to this folder ID (""root"" to upload to root folder):"
        Form1.UploadTimeLeftLabel.Text = "Time Left: "
        Form1.FolderNameLabel.Text = "Folder Name:"
        Form1.FolderLabel.Text = "Folders:"
        Form1.FileNameLabel.Text = "File Name:"
        Form1.FileIdLabel.Text = "File ID:"
        Form1.DateCreatedLabel.Text = "Date Created:"
        Form1.DateModifiedLabel.Text = "Date Modified:"
        Form1.MD5ChecksumLabel.Text = "MD5 Checksum:"
        Form1.MIMETypeLabel.Text = "MIME Type:"
        Form1.FileSizeLabel.Text = "File Size:"
        Form1.SaveChecksumFileButton.Text = "Save Checksum File"
        Form1.UploadButton.Text = "Upload"
        Form1.ClearUploadQueueButton.Text = "Clear List"
        Form1.RefreshListButton.Text = "Refresh List"
        Form1.DownloadFileButton.Text = "Download File"
        Form1.RemoveSelectedFilesFromList.Text = "Remove selected file(s) from list"
        Form1.SaveSelectedFilesChecksumButton.Text = "Save Checksums for Selected Files"
        Form1.GetFolderIdNameButton.Text = "Get Folder Name"
        Form1.BackButton.Text = "Back"
        Form1.GroupBox2.Text = "File Information:"
        If Form1.viewing_trash = False Then
            Form1.ViewTrashButton.Text = "View Trash"
        ElseIf Form1.viewing_trash = True Then
            Form1.ViewTrashButton.Text = "View Drive"
        End If
        Form1.CreateNewFolderButton.Text = "Create New Folder"
        Form1.UploadToSelectedFolderButton.Text = "Upload selected file(s) to current folder"
        Form1.DeselectItemFromUploadQueueButton.Text = "Deselect"
        Form1.btnLogout.Text = "Logout"
        Form1.ActionsToolStripMenuItem.Text = "Actions"
        Form1.ChecksumsOptionsToolStripMenuItem.Text = "Checksum Options"
        Form1.CreateNewFolderToolStripMenuItem.Text = "Create New Folder"
        Form1.EncodeFileFor.Text = "Encode file for"
        Form1.GetRawDownloadURLToolStripMenuItem.Text = "Get Raw Download URL"
        Form1.RenameToolStripMenuItem.Text = "Rename"
        Form1.SelectedFileToolStripMenuItem1.Text = "Selected File"
        Form1.SelectedFolderToolStripMenuItem1.Text = "Selected Folder"
        Form1.RefreshListToolStripMenuItem.Text = "Refresh List"
        Form1.MoveToTrashToolStripMenuItem.Text = "Move to Trash"
        Form1.SelectedFilesToolStripMenuItem.Text = "Selected file(s)"
        Form1.SelectedFoldersToolStripMenuItem.Text = "Selected folder(s)"
        Form1.RestoreToolStripMenuItem.Text = "Restore"
        Form1.SelectedFilesToolStripMenuItem1.Text = "Selected file(s)"
        Form1.SelectedFoldersToolStripMenuItem1.Text = "Selected folder(s)"
        Form1.SaveChecksumToolStripMenuItem.Text = "Save Checksum(s)"
        Form1.SaveChecksumsToolStripMenuItem.Text = "Save Checksums"
        Form1.SaveChecksumsToolStripMenuItem1.Text = "Save Checksums"
        Form1.SelectedFilesToolStripMenuItem2.Text = "Selected file(s)"
        Form1.SelectedFolderToolStripMenuItem2.Text = "Selected folder"
        Form1.CopyFileToRAMBeforeUploadingToolStripMenuItem.Text = "Copy File to RAM before uploading if there's enough Free Memory available"
        Form1.DonationsToolStripMenuItem.Text = "Donations"
        Form1.FileToolStripMenuItem.Text = "File"
        Form1.UploadToolStripMenuItem.Text = "Upload"
        Form1.FileToolStripMenuItem1.Text = "File(s)"
        Form1.FolderToolStripMenuItem.Text = "Folder"
        Form1.DownloadToolStripMenuItem.Text = "Download"
        Form1.DownloadToolStripMenuItem1.Text = "Download"
        Form1.DownloadToolStripMenuItem2.Text = "Download"
        Form1.MoveToolStripMenuItem.Text = "Move"
        Form1.MoveToolStripMenuItem1.Text = "Move"
        Form1.SelectedFileToolStripMenuItem.Text = "Selected File(s)"
        Form1.SelectedFolderToolStripMenuItem.Text = "Selected Folder"
        Form1.HelpToolStripMenuItem.Text = "Help"
        Form1.OpenInBrowserToolStripMenuItem.Text = "Open in Browser"
        Form1.OpenInBrowserToolStripMenuItem1.Text = "Open in Browser"
        Form1.OptionsToolStripMenuItem.Text = "Options"
        Form1.OrderByToolStripMenuItem.Text = "Order By"
        Form1.OrderByComboBox.Items.Clear()
        Form1.OrderByComboBox.Items.AddRange({"Created Time", "Folder", "Modified By Me Time", "Modified Time", "Name", "Natural Name", "Quota Bytes Used", "Recency", "Shared With Me Time", "Starred", "Viewed By Me Time"})
        Form1.OrderByComboBox.SelectedIndex = My.Settings.SortByIndex
        Form1.DescendingOrderToolStripMenuItem.Text = "Descending Order"
        Form1.PreserveFileModifiedDateToolStripMenuItem.Text = "Preserve File Modification Date"
        Form1.SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Text = "Save checksums as checksums.md5"
        Form1.StartUploadsAutomaticallyToolStripMenuItem.Text = "Start Uploads Automatically"
        Form1.SpecifyChunkSizeToolStripMenuItem.Text = "Specify Chunk Size"
        Form1.UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Text = "Update File and Folder views after an upload finishes"
        Form1.ReadmeToolStripMenuItem.Text = "Readme / Help"
        Form1.LoggedInAs.Text = "Logged In As:"
        Form1.UsedSpaceText.Text = "Used Space:"
        Form1.TotalSpaceText.Text = "Total Space:"
        Form1.FreeSpaceText.Text = "Free Space:"
    End Sub

    Public Shared Sub TChineseLanguage()
        Form1.FileSizeUploadLabel.Text = "文件大小:"
        Form1.ProcessedFileSizeUploadLabel.Text = "Processed:"
        Form1.UploadInstructionsLabel.Text = "請將文件拖到下方"
        Form1.AboutLabel.Text = "By Moisés Cardona" & vbNewLine & "v1.8.6" & vbNewLine & "Translated by mic4126"
        Form1.UploadStatusLabel.Text = "狀態:"
        Form1.UploadPercentLabel.Text = "百份比: "
        Form1.FilesLabel.Text = "文件:"
        Form1.UploadToThisFolderIDLabel.Text = "上傳到此文件夾ID (""root"" 指上傳到根目錄):"
        Form1.UploadTimeLeftLabel.Text = "餘下時間: "
        Form1.FolderNameLabel.Text = "文件夾名稱:"
        Form1.FileNameLabel.Text = "文件名稱:"
        Form1.FileIdLabel.Text = "文件ID:"
        Form1.DateCreatedLabel.Text = "新建日期:"
        Form1.DateModifiedLabel.Text = "修改日期:"
        Form1.MD5ChecksumLabel.Text = "MD5 校驗碼:"
        Form1.MIMETypeLabel.Text = "MIME Type:"
        Form1.FileSizeLabel.Text = "文件大小:"
        Form1.SaveChecksumFileButton.Text = "儲存校驗碼"
        Form1.UploadButton.Text = "上傳"
        Form1.ClearUploadQueueButton.Text = "清除列表"
        Form1.RefreshListButton.Text = "更新列表"
        Form1.DownloadFileButton.Text = "下載文件"
        Form1.RemoveSelectedFilesFromList.Text = "由列表中移除已選文件"
        Form1.SaveSelectedFilesChecksumButton.Text = "儲存已選文件校驗碼"
        Form1.GetFolderIdNameButton.Text = "獲取文件夾名稱"
        Form1.BackButton.Text = "返回"
        Form1.GroupBox2.Text = "File Information:"
        If Form1.viewing_trash = False Then
            Form1.ViewTrashButton.Text = "查看垃圾桶"
        ElseIf Form1.viewing_trash = True Then
            Form1.ViewTrashButton.Text = "回到Google Drive"
        End If
        Form1.CreateNewFolderButton.Text = "新增文件夾"
        Form1.UploadToSelectedFolderButton.Text = "Upload selected file(s) to current folder"
        Form1.DeselectItemFromUploadQueueButton.Text = "Deselect"
        Form1.btnLogout.Text = "登岀"
        Form1.ActionsToolStripMenuItem.Text = "Actions"
        Form1.ChecksumsOptionsToolStripMenuItem.Text = "Checksum Options"
        Form1.CreateNewFolderToolStripMenuItem.Text = "Create New Folder"
        Form1.EncodeFileFor.Text = "Encode file for"
        Form1.GetRawDownloadURLToolStripMenuItem.Text = "Get Raw Download URL"
        Form1.RenameToolStripMenuItem.Text = "Rename"
        Form1.SelectedFileToolStripMenuItem1.Text = "Selected File"
        Form1.SelectedFolderToolStripMenuItem1.Text = "Selected Folder"
        Form1.RefreshListToolStripMenuItem.Text = "Refresh List"
        Form1.MoveToTrashToolStripMenuItem.Text = "Move to Trash"
        Form1.SelectedFilesToolStripMenuItem.Text = "Selected file(s)"
        Form1.SelectedFoldersToolStripMenuItem.Text = "Selected folder(s)"
        Form1.RestoreToolStripMenuItem.Text = "Restore"
        Form1.SelectedFilesToolStripMenuItem1.Text = "Selected file(s)"
        Form1.SelectedFoldersToolStripMenuItem1.Text = "Selected folder(s)"
        Form1.SaveChecksumToolStripMenuItem.Text = "Save Checksum(s)"
        Form1.SaveChecksumsToolStripMenuItem.Text = "Save Checksums"
        Form1.SaveChecksumsToolStripMenuItem1.Text = "Save Checksums"
        Form1.SelectedFilesToolStripMenuItem2.Text = "Selected file(s)"
        Form1.SelectedFolderToolStripMenuItem2.Text = "Selected folder"
        Form1.CopyFileToRAMBeforeUploadingToolStripMenuItem.Text = "Copy File to RAM before uploading if there's enough Free Memory available"
        Form1.DonationsToolStripMenuItem.Text = "捐款"
        Form1.FileToolStripMenuItem.Text = "File"
        Form1.UploadToolStripMenuItem.Text = "Upload"
        Form1.FileToolStripMenuItem1.Text = "File(s)"
        Form1.FolderToolStripMenuItem.Text = "Folder"
        Form1.DownloadToolStripMenuItem.Text = "Download"
        Form1.DownloadToolStripMenuItem1.Text = "Download"
        Form1.DownloadToolStripMenuItem2.Text = "Download"
        Form1.MoveToolStripMenuItem.Text = "Move"
        Form1.MoveToolStripMenuItem1.Text = "Move"
        Form1.SelectedFileToolStripMenuItem.Text = "Selected File(s)"
        Form1.SelectedFolderToolStripMenuItem.Text = "Selected Folder"
        Form1.HelpToolStripMenuItem.Text = "Help"
        Form1.OpenInBrowserToolStripMenuItem.Text = "Open in Browser"
        Form1.OpenInBrowserToolStripMenuItem1.Text = "Open in Browser"
        Form1.OptionsToolStripMenuItem.Text = "Options"
        Form1.OrderByToolStripMenuItem.Text = "Order By"
        Form1.OrderByComboBox.Items.Clear()
        Form1.OrderByComboBox.Items.AddRange({"Created Time", "Folder", "Modified By Me Time", "Modified Time", "Name", "Natural Name", "Quota Bytes Used", "Recency", "Shared With Me Time", "Starred", "Viewed By Me Time"})
        Form1.OrderByComboBox.SelectedIndex = My.Settings.SortByIndex
        Form1.DescendingOrderToolStripMenuItem.Text = "Descending Order"
        Form1.PreserveFileModifiedDateToolStripMenuItem.Text = "Preserve File Modification Date"
        Form1.SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Text = "Save checksums as checksums.md5"
        Form1.StartUploadsAutomaticallyToolStripMenuItem.Text = "Start Uploads Automatically"
        Form1.SpecifyChunkSizeToolStripMenuItem.Text = "Specify Chunk Size"
        Form1.UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Text = "Update File and Folder views after an upload finishes"
        Form1.ReadmeToolStripMenuItem.Text = "Readme / Help"
        Form1.LoggedInAs.Text = "Logged In As:"
        Form1.UsedSpaceText.Text = "Used Space:"
        Form1.TotalSpaceText.Text = "Total Space:"
        Form1.FreeSpaceText.Text = "Free Space:"
    End Sub

    Public Shared Sub SpanishLanguage()
        Form1.FileSizeUploadLabel.Text = "Tamaño:"
        Form1.ProcessedFileSizeUploadLabel.Text = "Procesado:"
        Form1.UploadInstructionsLabel.Text = "Arrastre archivos aquí para añadirlos a la lista"
        Form1.AboutLabel.Text = "Por Moisés Cardona" & vbNewLine & "v1.8.6"
        Form1.UploadStatusLabel.Text = "Estado:"
        Form1.UploadPercentLabel.Text = "Porcentaje: "
        Form1.FilesLabel.Text = "Archivos:"
        Form1.UploadToThisFolderIDLabel.Text = "Subir a este ID de directorio (""root"" para subir a la raíz):"
        Form1.UploadTimeLeftLabel.Text = "Tiempo Est."
        Form1.FolderNameLabel.Text = "Nombre de la Carpeta:"
        Form1.FolderLabel.Text = "Carpetas:"
        Form1.FileNameLabel.Text = "Nombre:"
        Form1.FileIdLabel.Text = "ID:"
        Form1.DateCreatedLabel.Text = "Fecha Creada:"
        Form1.DateModifiedLabel.Text = "Fecha Modificada:"
        Form1.MD5ChecksumLabel.Text = "Checksum MD5:"
        Form1.MIMETypeLabel.Text = "Tipo MIME:"
        Form1.FileSizeLabel.Text = "Tamaño:"
        Form1.SaveChecksumFileButton.Text = "Guardar Archivo MD5"
        Form1.UploadButton.Text = "Subir"
        Form1.ClearUploadQueueButton.Text = "Borrar Lista"
        Form1.RefreshListButton.Text = "Refrescar Lista"
        Form1.DownloadFileButton.Text = "Descargar Archivo"
        Form1.RemoveSelectedFilesFromList.Text = "Remover archivo(s) de la lista"
        Form1.SaveSelectedFilesChecksumButton.Text = "Guardar Checksums de los archivos"
        Form1.GetFolderIdNameButton.Text = "Obtener Nombre de la Carpeta"
        Form1.BackButton.Text = "Atrás"
        If Form1.viewing_trash = False Then
            Form1.ViewTrashButton.Text = "Ver Basura"
        ElseIf Form1.viewing_trash = True Then
            Form1.ViewTrashButton.Text = "Ver Drive"
        End If
        Form1.CreateNewFolderButton.Text = "Crear Carpeta"
        Form1.UploadToSelectedFolderButton.Text = "Subir archivo(s) a esta carpeta"
        Form1.DeselectItemFromUploadQueueButton.Text = "Deseleccionar"
        Form1.btnLogout.Text = "Cerrar Sesión"
        Form1.GroupBox2.Text = "Información del archivo:"
        Form1.ActionsToolStripMenuItem.Text = "Acciones"
        Form1.ChecksumsOptionsToolStripMenuItem.Text = "Opciones de Checksums"
        Form1.CreateNewFolderToolStripMenuItem.Text = "Crear nueva carpeta"
        Form1.GetRawDownloadURLToolStripMenuItem.Text = "Obtener enlace cruda de descarga"
        Form1.EncodeFileFor.Text = "Guardar archivo para"
        Form1.RenameToolStripMenuItem.Text = "Renombrar"
        Form1.SelectedFileToolStripMenuItem1.Text = "Archivo seleccionado"
        Form1.SelectedFolderToolStripMenuItem1.Text = "Carpeta seleccionada"
        Form1.RefreshListToolStripMenuItem.Text = "Refrescar Lista"
        Form1.MoveToTrashToolStripMenuItem.Text = "Mover a la Basura"
        Form1.SelectedFilesToolStripMenuItem.Text = "Archivo(s) seleccionados"
        Form1.SelectedFoldersToolStripMenuItem.Text = "Carpeta(s) seleccionadas"
        Form1.RestoreToolStripMenuItem.Text = "Restaurar"
        Form1.SelectedFilesToolStripMenuItem1.Text = "Archivo(s) seleccionados"
        Form1.SelectedFoldersToolStripMenuItem1.Text = "Carpeta(s) seleccionadas"
        Form1.SaveChecksumToolStripMenuItem.Text = "Guardar Checksum(s)"
        Form1.SaveChecksumsToolStripMenuItem1.Text = "Guardar Checksums"
        Form1.SaveChecksumsToolStripMenuItem.Text = "Guardar Checksums"
        Form1.SelectedFilesToolStripMenuItem2.Text = "Archivo(s) seleccionados"
        Form1.SelectedFolderToolStripMenuItem2.Text = "Carpeta seleccionada"
        Form1.CopyFileToRAMBeforeUploadingToolStripMenuItem.Text = "Copiar archivo a memoria antes de subirlo si hay memoria disponible"
        Form1.DonationsToolStripMenuItem.Text = "Donar"
        Form1.FileToolStripMenuItem.Text = "Archivo"
        Form1.UploadToolStripMenuItem.Text = "Subir"
        Form1.FileToolStripMenuItem1.Text = "Archivo(s)"
        Form1.FolderToolStripMenuItem.Text = "Carpeta"
        Form1.DownloadToolStripMenuItem.Text = "Descargar"
        Form1.DownloadToolStripMenuItem1.Text = "Descargar"
        Form1.DownloadToolStripMenuItem2.Text = "Descargar"
        Form1.MoveToolStripMenuItem.Text = "Mover"
        Form1.MoveToolStripMenuItem1.Text = "Mover"
        Form1.SelectedFileToolStripMenuItem.Text = "Archivo(s) seleccionado(s)"
        Form1.SelectedFolderToolStripMenuItem.Text = "Carpeta seleccionada"
        Form1.HelpToolStripMenuItem.Text = "Ayuda"
        Form1.OpenInBrowserToolStripMenuItem.Text = "Abrir en Navegador"
        Form1.OpenInBrowserToolStripMenuItem1.Text = "Abrir en Navegador"
        Form1.OrderByToolStripMenuItem.Text = "Ordenar por"
        Form1.OrderByComboBox.Items.Clear()
        Form1.OrderByComboBox.Items.AddRange({"Fecha de creación", "Carpeta", "Modificado por mí", "Fecha de Modificación", "Nombre", "Nombre Natural", "Espacio usado", "Recientes", "Compartidos conmigo", "Estrellado", "Fecha de Acceso/Visto"})
        Form1.OrderByComboBox.SelectedIndex = My.Settings.SortByIndex
        Form1.DescendingOrderToolStripMenuItem.Text = "Ordenar Descendiente"
        Form1.PreserveFileModifiedDateToolStripMenuItem.Text = "Preservar fecha de modificación"
        Form1.SaveCheckumsAsChecksumsmd5ToolStripMenuItem.Text = "Guardar checksums como checksums.md5"
        Form1.StartUploadsAutomaticallyToolStripMenuItem.Text = "Subir archivos automáticamente"
        Form1.SpecifyChunkSizeToolStripMenuItem.Text = "Especificar tamaño de pedazos"
        Form1.UpdateFileAndFolderViewsAfterAnUploadFinishesToolStripMenuItem.Text = "Actualizar vista de archivos y carpetas al terminar de subir un archivo"
        Form1.ReadmeToolStripMenuItem.Text = "Léeme / Ayuda"
        Form1.LoggedInAs.Text = "Usuario:"
        Form1.UsedSpaceText.Text = "Espacio Usado:"
        Form1.TotalSpaceText.Text = "Espacio Total:"
        Form1.FreeSpaceText.Text = "Espacio Libre:"
    End Sub

    Public Shared Function MsgAndDialogLang(tag As String) As String
        Select Case tag
            Case "client_secrets_not_found"
                Select Case My.Settings.Language
                    Case "English"
                        Return "client_secret.json file not found. Please follow Step 1 in this guide: https://developers.google.com/drive/v3/web/quickstart/dotnet" & vbCr & vbCrLf & "This file should be located in the folder where this software is located."
                    Case "Spanish"
                        Return "El archivo client_secret.json no fue encontrado. Por favor, siga el Paso 1 de esta guía: https://developers.google.com/drive/v3/web/quickstart/dotnet" & vbCr & vbCrLf & "Este archivo debe estar localizado en la carpeta donde se encuentra este programa."
                    Case "TChinese"
                        Return "client_secret.json 檔案找不到.請做: https://developers.google.com/drive/v3/web/quickstart/dotnet" & "的第一歩" & vbCr & vbCrLf & "請將client_secret.json放到軟體的根目錄."
                    Case Else
                End Select
            Case "folder_id_incorrect"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Folder ID is incorrect."
                    Case "Spanish"
                        Return "El ID de la carpeta es incorrecto."
                    Case "TChinese"
                        Return "Folder ID is incorrect."
                    Case Else
                End Select
            Case "downloads_finished"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Download(s) Finished!"
                    Case "Spanish"
                        Return "Archivo(s) descargado(s)"
                    Case "TChinese"
                        Return "Download(s) Finished!"
                    Case Else
                End Select
            Case "folder_invaild"
                Select Case My.Settings.Language
                    Case "English"
                        Return "The specified folder is invalid. Do you want to change the folder? If you select No, your files will be uploaded to the root of Google Drive"
                    Case "Spanish"
                        Return "La carpeta especificada es invalida. Desea cambiar la carpeta? Si presiona No, sus archivos serán subidos a la raíz de Google Drive"
                    Case "TChinese"
                        Return "The specified folder is invalid. Do you want to change the folder? If you select No, your files will be uploaded to the root of Google Drive"
                    Case Else

                End Select
            Case "upload_finish"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Uploads Finished!"
                    Case "Spanish"
                        Return "Los archivos han terminado de subir."
                    Case "TChinese"
                        Return "完成上傳"
                    Case Else

                End Select
            Case "uploadstatus_copytoram"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Copying to RAM"
                    Case "Spanish"
                        Return "Copiando a RAM"
                    Case "TChinese"
                        Return "Copying to RAM"
                End Select
            Case "uploadstatus_complete"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Completed!!"
                    Case "Spanish"
                        Return "¡Completado!"
                    Case "TChinese"
                        Return "完成!!"
                End Select
            Case "uploadstatus_downloading"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Downloading..."
                    Case "Spanish"
                        Return "Descargando..."
                    Case "TChinese"
                        Return "下載中..."

                End Select
            Case "uploadstatus_starting"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Starting..."
                    Case "Spanish"
                        Return "Comenzando..."
                    Case "TChinese"
                        Return "開始中..."
                End Select
            Case "uploadstatus_uploading"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Uploading..."
                    Case "Spanish"
                        Return "Subiendo..."
                    Case "TChinese"
                        Return "上傳中..."

                End Select
            Case "uploadstatus_retry"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Retrying..."
                    Case "Spanish"
                        Return "Intentando..."
                    Case "TChinese"
                        Return "重試中..."

                End Select
            Case "uploadstatus_failed"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Failed..."
                    Case "Spanish"
                        Return "Error..."
                    Case "TChinese"
                        Return "出錯了..."

                End Select
            Case "resume_upload_question"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Resume previous upload?{0}{0}{1}"
                    Case "Spanish"
                        Return "¿Resumir carga anterior?{0}{0}{1}"
                    Case "TChinese"
                        Return "要不要恢復上次未完成上傳?{0}{0}{1}"
                End Select
            Case "resume_upload"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Resume Upload"
                    Case "Spanish"
                        Return "Resumir"
                    Case "TChinese"
                        Return "恢復上載"
                End Select
            Case "location_browse"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Browse for a location to save the file:"
                    Case "Spanish"
                        Return "Busque un lugar para descargar el archivo:"
                    Case "TChinese"
                        Return "請選擇地方儲存:"
                End Select
            Case "enter_name_for_folder"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Enter a name for the new folder:"
                    Case "Spanish"
                        Return "Escriba un nombre para la nueva carpeta:"
                    Case "TChinese"
                        Return "請為新文件夾改名:"
                End Select
            Case "enter_new_name"
                Select Case My.Settings.Language
                    Case "English"
                        Return "New Name:"
                    Case "Spanish"
                        Return "Nuevo nombre:"
                    Case "TChinese"
                        Return "New Name:"
                End Select
            Case "create_new_folder"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Create new Folder"
                    Case "Spanish"
                        Return "Crear nueva carpeta"
                    Case "TChinese"
                        Return "增加新文件夾"
                End Select
            Case "rename_dialog"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Rename"
                    Case "Spanish"
                        Return "Renombrar"
                    Case "TChinese"
                        Return "Rename"
                End Select
            Case "checksum_location"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Browse for a location to save the checksum file:"
                    Case "Spanish"
                        Return "Busque un lugar para guardar el archivo del checksum:"
                    Case "TChinese"
                        Return "請選擇地方儲存校驗碼:"
                End Select
            Case "confirm_move_selected_folder2trash"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you really want to move the selected folders to the Trash?"
                    Case "Spanish"
                        Return "¿Está seguro de querer mover las carpetas seleccionadas a la Basura?"
                    Case "TChinese"
                        Return "Do you really want to move the selected folders to the Trash?"
                End Select
            Case "folders_moved2trash"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Folders moved to trash"
                    Case "Spanish"
                        Return "Las carpetas se movieron a la basura."
                    Case "TChinese"
                        Return "文件夾已移到垃圾桶"
                End Select
            Case "folder_moved2trash"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Folder moved to trash"
                    Case "Spanish"
                        Return "La carpeta se movió a la basura"
                    Case "TChinese"
                        Return "文件夾已移到垃圾桶"
                End Select
            Case "checksums_saved"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Checksum(s) Saved!"
                    Case "Spanish"
                        Return "Los Checksums han sido guardados"
                    Case "TChinese"
                        Return "???"
                End Select
            Case "confirm_move_folder2trash_part1"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you really want to move the folder "
                    Case "Spanish"
                        Return "¿Está seguro de querer mover la carpeta "
                    Case "TChinese"
                        Return "你真係想將此文件夾 "
                End Select
            Case "confirm_move_folder2trash_part2"
                Select Case My.Settings.Language
                    Case "English"
                        Return " to the Trash?"
                    Case "Spanish"
                        Return " a la Basura?"
                    Case "TChinese"
                        Return " 移到垃圾桶?"
                End Select
            Case "confirm_restore_selected_files"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you want to restore the selected files?"
                    Case "Spanish"
                        Return "¿Está seguro de querer restaurar los archivos seleccionados?"
                    Case "TChinese"
                        Return "你真係想還原所選的文件夾?"
                End Select
            Case "confirm_restore_selected_folders"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you want to restore the selected folders?"
                    Case "Spanish"
                        Return "¿Está seguro de querer restaurar las carpetas seleccionados?"
                    Case "TChinese"
                        Return "你真係想還原所選的文件夾?"
                End Select
            Case "restore_folder_part1"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you want to restore the folder "
                    Case "Spanish"
                        Return "¿Está seguro de querer restaurar la carpeta "
                    Case "TChinese"
                        Return "你真係想還原文件夾"
                End Select
            Case "restore_folder_part2"
                Select Case My.Settings.Language
                    Case "English"
                        Return "?"
                    Case "Spanish"
                        Return "?"
                    Case "TChinese"
                        Return "?"
                End Select
            Case "folder_restored"
                Select Case My.Settings.Language
                    Case "English"
                        Return "The Folder has been restored"
                    Case "Spanish"
                        Return "La carpeta ha sido restaurada"
                    Case "TChinese"
                        Return "文件夾巳還原"
                End Select
            Case "folders_restored"
                Select Case My.Settings.Language
                    Case "English"
                        Return "The Folders have been restored"
                    Case "Spanish"
                        Return "Las carpetas han sido restauradas"
                    Case "TChinese"
                        Return "文件夾巳還原"
                End Select
            Case "move_selected_file2trash"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you really want to move the selected files to the Trash?"
                    Case "Spanish"
                        Return "¿Está seguro de querer mover los archivos seleccionados a la Basura?"
                    Case "TChinese"
                        Return "Do you really want to move the selected files to the Trash?"
                End Select
            Case "file_moved2trash"
                Select Case My.Settings.Language
                    Case "English"
                        Return "File moved to trash"
                    Case "Spanish"
                        Return "El archivo se movió a la basura."
                    Case "TChinese"
                        Return "檔案已移到垃圾桶"
                End Select
            Case "files_moved2trash"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Files moved to trash"
                    Case "Spanish"
                        Return "Los archivos se movieron a la basura."
                    Case "TChinese"
                        Return "檔案已移到垃圾桶"
                End Select
            Case "move_file2trash_part1"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you really want to move the file "
                    Case "Spanish"
                        Return "¿Está seguro de querer mover el archivo "
                    Case "TChinese"
                        Return "你真係想將 "
                End Select
            Case "move_file2trash_part2"
                Select Case My.Settings.Language
                    Case "English"
                        Return " to the Trash?"
                    Case "Spanish"
                        Return " a la Basura?"
                    Case "TChinese"
                        Return " 移到垃圾桶?"
                End Select
            Case "confirm_restrore_selected_file"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you want to restore the selected files?"
                    Case "Spanish"
                        Return "¿Está seguro de querer restaurar los archivos seleccionados?"
                    Case "TChinese"
                        Return "Do you want to restore the selected files?"
                End Select
            Case "file_restored"
                Select Case My.Settings.Language
                    Case "English"
                        Return "The File has been restored"
                    Case "Spanish"
                        Return "El archivo ha sido restaurado"
                    Case "TChinese"
                        Return "文件巳還原"
                End Select
            Case "files_restored"
                Select Case My.Settings.Language
                    Case "English"
                        Return "The Files have been restored"
                    Case "Spanish"
                        Return "Los archivos han sido restaurados"
                    Case "TChinese"
                        Return "文件巳還原"
                End Select
            Case "restore_file_part1"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Do you want to restore the file "
                    Case "Spanish"
                        Return "¿Desea restaurar el archivo "
                    Case "TChinese"
                        Return "Do you want to restore the file "
                End Select
            Case "restore_file_part2"
                Select Case My.Settings.Language
                    Case "English"
                        Return "?"
                    Case "Spanish"
                        Return "?"
                    Case "TChinese"
                        Return "?"
                End Select
            Case "logged_out"
                Select Case My.Settings.Language
                    Case "English"
                        Return "You have been logged out. The software will now close"
                    Case "Spanish"
                        Return "Tu sesión ha sido cerrada. El programa cerrará ahora"
                    Case "TChinese"
                        Return "You have been logged out. The software will now close"
                End Select
            Case "files_txt"
                Select Case My.Settings.Language
                    Case "English"
                        Return " files"
                    Case "Spanish"
                        Return " archivos"
                    Case "TChinese"
                        Return " files"
                    Case Else
                End Select
            Case "file_txt"
                Select Case My.Settings.Language
                    Case "English"
                        Return " file"
                    Case "Spanish"
                        Return " archivo"
                    Case "TChinese"
                        Return " file"
                    Case Else
                End Select
            Case "no_file_selected"
                Select Case My.Settings.Language
                    Case "English"
                        Return "No file selected"
                    Case "Spanish"
                        Return "No seleccionó un archivo"
                    Case "TChinese"
                        Return "No file selected"
                    Case Else
                End Select
            Case "no_files_selected"
                Select Case My.Settings.Language
                    Case "English"
                        Return "No file(s) selected"
                    Case "Spanish"
                        Return "No seleccionó archivo(s)"
                    Case "TChinese"
                        Return "No file(s) selected"
                    Case Else
                End Select
            Case "no_folders_selected"
                Select Case My.Settings.Language
                    Case "English"
                        Return "No folder(s) selected"
                    Case "Spanish"
                        Return "No seleccionó carpeta(s)"
                    Case "TChinese"
                        Return "No folder(s) selected"
                    Case Else
                End Select
            Case "file_moved"
                Select Case My.Settings.Language
                    Case "English"
                        Return "File Moved."
                    Case "Spanish"
                        Return "El archivo sido movido."
                    Case "TChinese"
                        Return "File Moved."
                    Case Else
                End Select
            Case "files_moved"
                Select Case My.Settings.Language
                    Case "English"
                        Return "Files Moved."
                    Case "Spanish"
                        Return "Los archivos han sido movidos."
                    Case "TChinese"
                        Return "Files Moved."
                    Case Else
                End Select
            Case Else
                Return "Error Typo " & tag
        End Select
        Return tag & " not found"
    End Function
End Class
