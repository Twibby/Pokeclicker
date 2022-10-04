
mergeInto(LibraryManager.library, {
	// DownloadFile method does not open SaveFileDialog like standalone builds, its just allows user to download file
	// gameObjectNamePtr: Unique GameObject name. Required for calling back unity with SendMessage.
	// methodNamePtr: Callback method name on given GameObject.
	// filenamePtr: Filename with extension
	// byteArray: byte[]
	// byteArraySize: byte[].Length
	DownloadFileCustom: function(gameObjectNamePtr, methodNamePtr, filenamePtr, byteArray, byteArraySize) {
		gameObjectName = Pointer_stringify(gameObjectNamePtr);
		methodName = Pointer_stringify(methodNamePtr);
		filename = Pointer_stringify(filenamePtr);

		var bytes = new Uint8Array(byteArraySize);
		for (var i = 0; i < byteArraySize; i++) {
			bytes[i] = HEAPU8[byteArray + i];
		}

		var downloader = window.document.createElement('a');
		downloader.setAttribute('id', gameObjectName);
		downloader.href = window.URL.createObjectURL(new Blob([bytes], { type: 'application/octet-stream' }));
		downloader.download = filename;
		document.body.appendChild(downloader);
		downloader.click();
		document.body.removeChild(downloader);
		document.onmouseup = null;

		myGameInstance.SendMessage(gameObjectName, methodName, downloader.download);
	},
});