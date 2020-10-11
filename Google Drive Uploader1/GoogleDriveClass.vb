Imports System.IO
Imports System.Threading
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Drive.v3
Imports Google.Apis.Drive.v3.Data
Imports Google.Apis.Services
Imports Google.Apis.Util.Store

Public Class GoogleDriveClass
    Shared ReadOnly Scopes As String() = {DriveService.Scope.DriveFile, DriveService.Scope.Drive}
    Shared SoftwareName As String = String.Empty
    Public service As DriveService
    Public FolderList As List(Of String) = New List(Of String)
    Public FolderListID As List(Of String) = New List(Of String)
    Public FileSizeList As New List(Of Long?)
    Public FileMIMEList As New List(Of String)
    Public FileList As List(Of String) = New List(Of String)
    Public FileListID As List(Of String) = New List(Of String)
    Public FileModifiedTimeList As New List(Of Date?)
    Public FileCreatedTimeList As New List(Of Date?)
    Public FileMD5List As New List(Of String)
    Public previousFolder As List(Of String) = New List(Of String)
    Public connected As Boolean = False
    Public currentFolder As String = "root"
    Public currentFolderName As String = "My Drive"
    Public credential As UserCredential
    Public Sub New(ByVal AppName As String)
        SoftwareName = AppName
        Dim SectretsFile As String = String.Empty
        If IO.File.Exists("client_secret.json") Then
            SectretsFile = "client_secret.json"
        ElseIf IO.File.Exists("credentials.json") Then
            SectretsFile = "credentials.json"
        End If
        If Not SectretsFile = String.Empty Then
            Using stream = New FileStream(SectretsFile, FileMode.Open, FileAccess.Read)
                Dim credPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                credPath = Path.Combine(credPath, ".credentials/" & SoftwareName)
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, New FileDataStore(credPath, True)).Result
            End Using
            service = New DriveService(New BaseClientService.Initializer() With {
                .HttpClientInitializer = credential,
                .ApplicationName = SoftwareName
            })
            connected = True
        Else
            connected = False
        End If
    End Sub
    Private Async Function getToken(ByVal credentials As UserCredential) As Task(Of String)
        Return Await credentials.GetAccessTokenForRequestAsync()
    End Function
    Public Function GetFolderName(ByVal Id As String) As String
        If String.IsNullOrEmpty(Id) = False Then
            Try
                Dim getRequest As FilesResource.GetRequest = service.Files.[Get](Id)
                Dim folderName As Data.File = getRequest.Execute()
                Return folderName.Name
            Catch
                Return Translations.MsgAndDialogLang("folder_id_incorrect")
            End Try
        Else
            Return Translations.MsgAndDialogLang("folder_id_incorrect")
        End If
    End Function
    Public Function GoBack(ByVal OrderBy As String) As Boolean
        Dim succeeded As Boolean = True
        If previousFolder.Count > 0 Then
            succeeded = GetData(previousFolder(previousFolder.Count - 1), OrderBy, True)
            If succeeded Then
                previousFolder.RemoveAt(previousFolder.Count - 1)
            End If
        End If
        Return succeeded
    End Function
    Public Function GetFileMetadata(ByVal fileId As String) As Data.File
        Dim FileMetadata As FilesResource.GetRequest = service.Files.Get(fileId)
        FileMetadata.Fields = "id, name, size, createdTime, modifiedTime, md5Checksum, mimeType"
        Return FileMetadata.Execute()
    End Function
    Public Function GetData(ByVal folderId As String, ByVal OrderBy As String, ByVal Optional goingBack As Boolean = False, ByVal Optional refreshing As Boolean = False) As Boolean
        FolderList.Clear()
        FolderListID.Clear()
        FileList.Clear()
        FileListID.Clear()
        FileSizeList.Clear()
        FileModifiedTimeList.Clear()
        FileCreatedTimeList.Clear()
        FileMD5List.Clear()
        FileMIMEList.Clear()
        Dim listRequestQString As String
        Dim listRequestQFolderString As String
        If folderId = "trash" Then
            currentFolderName = "Trash"
            listRequestQString = "mimeType!='application/vnd.google-apps.folder' and trashed = true"
            listRequestQFolderString = "mimeType='application/vnd.google-apps.folder' and trashed = true"
        Else
            listRequestQString = "mimeType!='application/vnd.google-apps.folder' and '" & folderId & "' in parents and trashed = false"
            listRequestQFolderString = "mimeType='application/vnd.google-apps.folder' and '" & folderId & "' in parents and trashed = false"
        End If
        Dim PageToken1 As String = String.Empty
        Do
            Dim listRequest As FilesResource.ListRequest = service.Files.List()
            listRequest.Fields = "nextPageToken, files(id, name, size, createdTime, modifiedTime, md5Checksum, mimeType)"
            listRequest.Q = listRequestQString
            listRequest.OrderBy = OrderBy
            listRequest.PageToken = PageToken1
            Try
                Dim files = listRequest.Execute()
                If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
                    For Each file In files.Files
                        FileList.Add(file.Name)
                        FileListID.Add(file.Id)
                        If file.Size IsNot Nothing Then FileSizeList.Add(file.Size) Else FileSizeList.Add(0)
                        FileModifiedTimeList.Add(file.ModifiedTime)
                        FileCreatedTimeList.Add(file.CreatedTime)
                        If file.Md5Checksum IsNot Nothing Then FileMD5List.Add(file.Md5Checksum) Else FileMD5List.Add("")
                        FileMIMEList.Add(file.MimeType)
                    Next
                End If
                PageToken1 = files.NextPageToken
            Catch ex As Exception
                MsgBox("Error")
                Return False
            End Try
        Loop While PageToken1 IsNot Nothing
        Dim PageToken2 As String = String.Empty
        Do
            Dim listRequest As FilesResource.ListRequest = service.Files.List()
            listRequest.Fields = "nextPageToken, files(id, name)"
            listRequest.Q = listRequestQFolderString
            listRequest.OrderBy = OrderBy
            listRequest.PageToken = PageToken2
            Try
                Dim files = listRequest.Execute()
                If files.Files IsNot Nothing AndAlso files.Files.Count > 0 Then
                    For Each file In files.Files
                        FolderList.Add(file.Name)
                        FolderListID.Add(file.Id)
                    Next
                End If
                PageToken2 = files.NextPageToken
            Catch
                Return False
            End Try
        Loop While PageToken2 IsNot Nothing
        If Not refreshing AndAlso Not goingBack AndAlso folderId <> "root" AndAlso folderId <> "trash" Then
            previousFolder.Add(currentFolder)
        End If
        If folderId <> "trash" Then
            currentFolder = folderId
            currentFolderName = GetFolderName(currentFolder)
        Else
            currentFolderName = Translations.MsgAndDialogLang("trash")
        End If
        Return True
    End Function

    Public Sub DownloadFile(ByVal Id As String, ByVal stream As FileStream)
        Dim getRequest As FilesResource.GetRequest = service.Files.[Get](Id)
        getRequest.Download(stream)
    End Sub
End Class
