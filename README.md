# GDriveUploadTool
A tool for Windows to upload files to Google Drive. It resumes uploads in case of an error or failure. Perfect for uploading large files or if your connection is unstable. Also, the software lists the latest uploads and allows you to download them to a chosen folder.

# How to use?
1. Before using this software, follow the Step 1 (Step 1: Turn on the Drive API) in this quick start guide from Google: https://developers.google.com/drive/v3/web/quickstart/dotnet
2. After following the steps, launch the software. It will launch a web tab in your browser to allow the software to access your account.
3. Start uploading or downloading a file!

# How to upload a file?
You have 2 options to upload a file. 
* The first is to press the Browse button and select a file, then press Upload. 
* The second option is to drag and drop a file, then press Upload
* If an upload is interrumpted or the software or the PC crashes, you'll be prompted to continue the upload where it left of if you are uploading the same file as before. If you attempt to upload another file after the first one got interrumpted, you will lose the ability to resume the previous file.

# How to download a file?
1. Choose a file from the list
2. Press the "Download File" button.
3. Choose a folder and if you want, change the filename and press "Save"
4. The download will start

# How to open and compile the project?
This project was made using Visual Studio 2017, in the Visual Basic .NET language. You'll need to have the Windows Desktop components installed to be able to open and edit this project to fit your needs and to compile it. It also uses the Google Drive API which is available in the NuGet Package Manager. You may also be prompted to download the NuGet packages for this project.

Enjoy!!!

# Changelog:
v1.1 
- Lists files in ListBox
- Can download a selected file to a chosen directory
- Can drag and drop a file to upload

v1.0
- Initial Release
- Ability to upload a file and resume if interrumpted
