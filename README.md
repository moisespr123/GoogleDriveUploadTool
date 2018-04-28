# Google Drive Upload Tool
A tool for Windows to upload files to Google Drive. It resumes uploads in case of an error or failure. Perfect for uploading large files or if your connection is unstable. Additionally, files uploaded with this tool will preserve their Modified Date. Also, the software lets you browse and download your Drive contents.

# How to use?
1. Before using this software, follow the Step 1 (Step 1: Turn on the Drive API) in this quick start guide from Google: [https://developers.google.com/drive/v3/web/quickstart/dotnet](https://developers.google.com/drive/v3/web/quickstart/dotnet).
2. After following the steps, launch the software. It will launch a web tab in your browser to allow the software to access your account.
3. Start uploading or downloading a file!

# How to upload a file or folder?
1. drag and drop one or more file or folder.
2. press "Upload". 

**-OR-**

Go to File -> Upload -> Select File or Folder

* You don't need to perform step 2 if you enabled the "Start Uploads Automatically" in the Options menu
* When uploading folders that contains subfolders and subfiles, The original file and folder structure will be kept in Google Drive.
* If an upload is interrupted or the software or the PC crashes, you'll be prompted to continue the upload where it left of if you are uploading the same file as before. If you attempt to upload another file after the first one got interrumpted, you will lose the ability to resume the previous file. If the software is still open and the connection is interrupted, it will attempt to continue the upload.

# How to select a folder?
* Just browse for a folder in the Folder Browser listbox.
* If you already added a file to the Upload list and want to change the location, browse for the folder and press the "Upload selected file(s) to current folder" button.

# How to create a folder?
1. Browse to the location you wish to create the new folder.
2. Press "Create New Folder" or Go to Actions -> Create New Folder
3. Type a Folder Name and press OK.



# How to check if Folder ID is correct?
1. Type Folder ID in the Text Box.
2. Press the "Get Folder Name" button.
3. If the Folder ID is correct, you'll see the Folder Name in the Text Box below the Folder ID. If the Folder ID is incorrect or not found, a message will be shown telling that the Folder ID is incorrect.

# How to download a file?
1. Choose a file from the list.
2. Press the "Download File" button, press ALT + D, or go to File -> Download -> Selected File(s).
3. Choose a folder and if you want, change the filename and press "Save".
4. The download will start.

# Can I download more than one file at a time?
Absolutely! Select the files you want to download and press ALT + D, or go to File -> Download -> Selected File(s). Then, pick a folder to save the files.

# Can I download a full folder?
Absolutely! Just select a folder and press ALT + D, or go to File -> Download -> Selected Folder. Then, browse for the location to download all the content that is in that folder.

# How to save the Checksum of a file?
1. Select a file.
2. Press the "Save Checksum File" button, press ALT + C in your keyboard, or go to Actions -> Save Checksums -> Selected File(s).
3. Browse for a location to save the file.

# How to save the Checksum of more than one file?
1. Select the files by pressing Ctrl or Shift in your keyboard, or press ALT + A to select all of the files.
2. Press the "Save Checksums for Selected Files" button, press ALT + C in your keyboard, or go to Actions -> Save Checksums -> Selected File(s).
3. Browse for a location to save the file.

# How to save the Checksum of the content of a folder?
1. Select the folder you want to save the checksums of its contents.
2. Press ALT + C in your keyboard, or go to Actions -> Save Checksums -> Selected Folder
3. Browse for a location to save the file.

These are the checksums for your files stored in Google Drive. You can use them to verify if the uploaded file matches the local file.

# How to move one or more files or folders to Trash?
1. Select the file(s) or folder(s) to send to Trash.
2. Press the "Delete" key, or go to Actions -> Move to Trash -> Selected File(s) or Selected Folder(s).

# How to restore one or more files or folders in the Trash?
1. Press the "View Trash" button.
2. Select the file(s) or folder(s) to restore.
3. Press ALT + R in your keyboard, or go to Actions -> Restore -> Selected File(s) or Selected Folder(s).

# Keyboard Shortcuts
* **Enter**: Enter a folder.
* **Delete**: Move file or folder to trash.
* **F5**: Refresh file and folder list.
* **CTRL + A**: Select all files or folders.
* **CTRL + C**: Save selected file(s) or folder checksums.
* **CTRL + D**: Download selected file(s) or entire folder.
* **CTRL + R**: When not in trash, rename a file or folder. When in trash, restore a file or folder.

# How to open and compile the project?
This project was made using Visual Studio 2017, in the Visual Basic .NET language. You'll need to have the Windows Desktop components installed to be able to open and edit this project to fit your needs and to compile it. It also uses the Google Drive API which is available in the NuGet Package Manager. You may also be prompted to download the NuGet packages for this project.

The feature "Copy File to RAM before Uploading" depends on a code written in C# called MemoryTributary. You can get the code from here: https://www.codeproject.com/articles/348590/a-replacement-for-memorystream. You then need to make a DLL file and load it to this project.

Enjoy!!!

# Changelog:
You can view the Changelog [by clicking here](https://github.com/moisesmcardona/GoogleDriveUploadTool/blob/master/Google%20Drive%20Uploader1/Changelog.txt).

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
