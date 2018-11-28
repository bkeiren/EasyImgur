# ![](https://i.imgur.com/YmUWm8r.png) EasyImgur

EasyImgur is a small and simple tool to easily upload images to imgur.com from your desktop.

## Table of Contents

* [Using EasyImgur](#using-easyimgur)
  * [Downloading and running](#downloading-and-running)
  * [Uploading from the tray](#uploading-from-the-tray)
  * [Uploading with the explorer context menu](#uploading-with-the-explorer-context-menu)
* [Using EasyImgur to upload images to your Imgur account](#using-easyimgur-to-upload-images-to-your-imgur-account)
* [Viewing uploaded images](#viewing-uploaded-images)
* [Settings](#settings)
* [Running in portable mode](#running-in-portable-mode)
* [Command Line Parameters](#command-line-parameters)

## Using EasyImgur

### Downloading and running

[Download EasyImgur](https://github.com/Rycul/EasyImgur/releases) (zip archive) and extract the archive contents to a folder.

Run *EasyImgur.exe* to start the application. The EasyImgur icon will appear in your system tray (if this is your first time running EasyImgur, EI will save its location and start automatically next time you start your computer):
> ![](https://i.imgur.com/HPVTZJz.png)

### Uploading from the tray

Open the EasyImgur menu by right-clicking on the tray icon:
> ![](https://i.imgur.com/wjLl8o2.png)

**Upload a file or a clipboard image** (a screenshot or a text URL that you've copied, for example) anonymously by using the *Upload from file...* and *Upload from clipboard* options.

### Uploading with the explorer context menu

You can also upload images or folders with images using the context menu of your file explorer. Note that you will need to tick **Enable context menu** in the settings to access these features.

**Upload a file** by right clicking it and selecting **Upload to Imgur** or **Upload to Imgur (anonymous)** from the context menu.
> ![](https://i.imgur.com/0CrGjVJ.png)

**Upload an album** from your file explorer by right clicking a folder and selecting **Upload to Imgur as album** or **Upload to Imgur as album (anonymous)**. Any pictures in the folder (**excluding** the pictures in subfolders) will be uploaded and placed into an album.
> ![](https://i.imgur.com/EvKCFug.png)

## Using EasyImgur to upload images to your Imgur account

To upload images to your Imgur account, you must first authorize EasyImgur to access your account.

1. Open the settings dialog by selecting *Settings* from the context menu shown when right-clicking on the EasyImgur icon in the tray. This opens the EasyImgur window. Go to the *Account* tab:
    > ![](https://i.imgur.com/7h9i2Qi.png)

2. Select *Authorize this app...*. This will open the Imgur application authorization page in your browser. If you are already logged in, you will see this message:
    > ![](https://i.imgur.com/ZgYVU6D.png)

3. Select *Allow* and copy the code that is displayed on the next page.

4. Go back to the EasyImgur window. You will see that a popup has been opened:
    > ![](https://i.imgur.com/99IRt3S.png)
   
   Paste the code in the text box and select *OK*. The EasyImgur application may appear unresponsive while it is receiving authorization from Imgur.

5. A balloon popup will be shown to inform you that the authorization was successful. The EasyImgur window will now display *Status: Authorized*:
    > ![](https://i.imgur.com/OrXgcjx.png)
    
    **NOTE**: To revoke access at any time, you can visit https://imgur.com/account/settings/apps, where you can manage all apps that have access to your account. Keep in mind that the *Forget Tokens* button in EasyImgur only discards the current codes needed to access your account. **If you suspect that someone has gained access to the authorized tokens, you need to revoke access from the aforementioned Imgur settings page!**

6. You can now use the *Upload from file... (to account)* and *Upload from clipboard (to account)* options from the EasyImgur menu.

**NOTE**: EasyImgur will periodically retrieve new authorization codes from Imgur. When this happens, a balloon popup will appear to inform you that EasyImgur has successfully done so (or not, if that should be the case). This popup can be disabled from the settings tab.

## Viewing uploaded images

Uploaded images (and their URLs) can be found under the ***History*** tab. From here you can view your previously uploaded images and their details. This tab also contains convenient controls that allow you to:
  - Remove an uploaded image from Imgur.
  - Remove an uploaded image from your local history database.
  - Open an image's link in your browser.

<!-- TODO: Screenshot of history tab -->

## Settings

EasyImgur has a number of different customizable settings. These can be accessed from the *General* tab in the EasyImgur settings window:

![Settings tab screenshot](https://i.imgur.com/9WeCEzm.png)

#### Copy links to clipboard
Determines whether links to images that have successfully been uploaded are automatically placed on your clipboard (ready to be pasted somewhere with Ctrl+V).

#### Clear clipboard immediately on upload
Determines whether the clipboard will be cleared after uploading from it.

#### Upload multiple images as gallery
Determines whether multiple images upload using the file dialog window are uploaded as separate images or contained in an album.

#### Preferred image format
This option gives a hint to Imgur as to what image format is preferred. Note that it only hints at this by providing the source image in a certain format. Imgur can (and sometimes will) change the format to something else if it chooses to do so.

#### Use this ... format
These define two strings that are used as the title and description for every uploaded image. The strings can contain special symbols which are converted to set values before uploading. For a complete set of symbols, click the *Format help* button.

##### Example:

Format used:

    Image_%n%_%date%_%time%
    
Title of the uploaded image:

    Image_0_19-08-2013_13:37:00
    
In this example, `%n%` is converted to an integer denoting how many images have currently been uploaded, `%date` is converted to the current date in DD-MM-YYYY format, and `%time%` is converted to the current time in HH-MM-SS format.

#### Launch EasyImgur at Windows start
This determines whether EasyImgur should have a registry key set to allow it to be started when Windows is started. Note that if you move the EasyImgur.exe file, the registry path will become incorrect and a manual start of EasyImgur is required to restore it (EasyImgur detects this and can take care of that when it starts).

#### Show notification on token refresh
If you have authorized EasyImgur to use your Imgur account, it can notify you each time it refreshes its authentication tokens. Enabling this option will allow EasyImgur to show a balloon tooltip pop up from your icon tray each time it refreshes its tokens.

#### Show notification on startup
EasyImgur will by default show a notification when it starts up to let you know it's ready for use. You can disable this from the settings tab.

#### Enable context menu
This option determines whether the following options are added to the file context menu (right click menu) to allow even easier uploading!:
    - Upload to Imgur
    - Upload to Imgur (anonymous)
	
## Running in portable mode

EasyImgur can be run in portable mode to allow you to take your settings and account authorization tokens with you on (for example) a USB drive! To run EasyImgur in portable mode simply start it with the command line switch `/portable`:

    EasyImgur.exe /portable

In portable mode your history, logs, and settings are saved to the local EasyImgur.exe folder.

## Command Line Parameters

Please refer to [the wiki](https://github.com/bkeiren/EasyImgur/wiki/Command-Line-Parameters) for a list of supported command-line parameters.
