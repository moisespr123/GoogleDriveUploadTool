# Google Drive Upload Tool
A tool for Windows to upload files to Google Drive. It resumes uploads in case of an error or failure. Perfect for uploading large files or if your connection is unstable. Additionally, files uploaded with this tool will preserve their Modified Date. Also, the software lists the latest uploads and allows you to download them to a chosen folder.

# How to use?
1. Before using this software, follow the Step 1 (Step 1: Turn on the Drive API) in this quick start guide from Google: https://developers.google.com/drive/v3/web/quickstart/dotnet
2. After following the steps, launch the software. It will launch a web tab in your browser to allow the software to access your account.
3. Start uploading or downloading a file!

# How to upload a file or folder?
1. drag and drop one or more file or folder
2. press "Upload". 

* When uploading folders that contains subfolders and subfiles, The original file and folder structure will be kept in Google Drive.
* If an upload is interrupted or the software or the PC crashes, you'll be prompted to continue the upload where it left of if you are uploading the same file as before. If you attempt to upload another file after the first one got interrumpted, you will lose the ability to resume the previous file. If the software is still open and the connection is interrupted, it will attempt to continue the upload.

# How to select a folder?
* Just browse for a folder in the Folder Browser listbox.
* If you already added a file to the Upload list and want to change the location, browse for the folder and press the "Upload selected file(s) to current folder" button.

# How to create a folder?
1. Browse to the location you wish to create the new folder
2. Press "Create New Folder""
3. Type a Folder Name and press OK

# How to check if Folder ID is correct?
1. Type Folder ID in the Text Box
2. Press the "Get Folder Name" button
3. If the Folder ID is correct, you'll see the Folder Name in the Text Box below the Folder ID. If the Folder ID is incorrect or not found, a message will be shown telling that the Folder ID is incorrect.

# How to download a file?
1. Choose a file from the list
2. Press the "Download File" button. Alternatively, press ALT + D.
3. Choose a folder and if you want, change the filename and press "Save"
4. The download will start

# Can I download more than one file at a time?
Absolutely! Select the files you want to download and press ALT + D. Then, pick a folder to save the files

# Can I download a full folder?
Absolutely! Just select a folder and press ALT + D. Then, browse for the location to download all the content that is in that folder.

# How to save a Checksum file?
1. Select a file
2. Press the "Save Checksum File" button

To save the checksums for more than 1 file:
1. Select more than 1 file holding either Ctrl or Shift with the keyboard and selecting the files with the mouse
2. A new button will appear right next to "Refresh List"
3. Press the new button called "Save Checksums for selected files".

To save the checksums for all files inside a folder:
1. Select the folder to save the checksums of its content
2. Press ALT + C
3. Browse for the location to save the checksum file

These are the checksums for your files stored in Google Drive. You can use them to verify if the uploaded file matches the local file.

# Keyboard Shortcuts
* **Enter**: Enter a folder
* **Delete**: Move file or folder to trash
* **F5**: Refresh file and folder list
* **ALT + A**: Select all files or folders
* **ALT + C**: Save selected file(s) or folder checksums
* **ALT + D**: Download selected file(s) or entire folder
* **ALT + R**: When not in trash, rename a file or folder. When in trash, restore a file or folder

# How to open and compile the project?
This project was made using Visual Studio 2017, in the Visual Basic .NET language. You'll need to have the Windows Desktop components installed to be able to open and edit this project to fit your needs and to compile it. It also uses the Google Drive API which is available in the NuGet Package Manager. You may also be prompted to download the NuGet packages for this project.

Enjoy!!!

# Changelog:
You can view the Changelog [by clicking here](https://github.com/moisesmcardona/GoogleDriveUploadTool/blob/master/Google%20Drive%20Uploader1/Changelog.txt).