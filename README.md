# Slidecopy
Take a picture on your phone and copy it to your computer's clipboard in seconds.

This functionality is provided by three components:
* An Android client to take a picture and upload it to the server
* A server (running on Node.js)
* A Windows client to download the picture and copy it to the clipboard

## Usage
**Disclaimer:** This project is not built to be secure or very reliable. Use at your own risk.

Assuming my development server is still running, you can grab the two clients from the [latest release](https://github.com/martindisch/Slidecopy/releases/latest) that are configured to use it. The Android client will request a unique code from the server and upload the picture you take with it. The Windows client will ask you for the same unique code on first startup and disappear to the notification area in the bottom right of your screen. It is advised to pull it out of the overflow collection to have it always visible. Double clicking on the icon will make the client attempt to download the latest picture you took and copy it to clipboard.

Keep in mind that this whole process is not at all secure. By simply trying other codes on the Windows client, it is possible for somebody else to download your current picture as well. Furthermore, the whole traffic between the server and the clients is not encrypted. Because the two clients from my release are configured to access my development server, the platform as a whole is not guaranteed to be reliable. There might be bugs or maybe I decide to stop running the server. So don't use this for private pictures or any application where you need to have reliable access to the picture you took.

## Build it yourself
In case you want to get your personal platform running, here's what you need to do.

### Server
The server runs on Node.js, so be sure to have that. There are a few dependencies from `npm`, more specifically [express](https://www.npmjs.com/package/express), [multer](https://www.npmjs.com/package/multer), [node-persist](https://www.npmjs.com/package/node-persist) and [date-and-time](https://www.npmjs.com/package/date-and-time). Have a look at [package.json](server/package.json). You can edit `app.listen(8080)` to have it run on a different port, but besides that there's not much else to know. Obviously you'll need a static public IP address.

### Android client
The Android client uses [android-async-http](https://github.com/loopj/android-async-http) and AppCompat, but they're both taken care of by gradle.

Because you might not want to expose the IP of your personal server on GitHub, it is loaded from an untracked string resource file. Just create `res/values/creds.xml` containing the two strings
```
<?xml version="1.0" encoding="utf-8"?>
<resources>
    <string name="upload_url">http://YOURIP:YOURPORT/upload</string>
    <string name="code_url">http://YOURIP:YOURPORT/code</string>
</resources>
```

### Windows client
The same goes for the Windows client, but here we're using a resource file. You need to create `creds.resx` in the root directory of the client and add two string values called `server_ip` and `server_port`.

## Libraries
This project uses the following libraries, graciously published by their authors.
* [express](https://www.npmjs.com/package/express)
* [multer](https://www.npmjs.com/package/multer)
* [node-persist](https://www.npmjs.com/package/node-persist)
* [date-and-time](https://www.npmjs.com/package/date-and-time)
* [android-async-http](https://github.com/loopj/android-async-http)
* Android AppCompat

## License
[MIT license](LICENSE)
