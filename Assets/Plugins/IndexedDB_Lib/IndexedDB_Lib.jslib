var IndexedDB_Lib = {
    $member: {
        db: null,
        dbname: "",
        inProgress: false,
        isError: false,
        bytes: null,
        json: null
    },

    $BeginProcess: function () {
        member.inProgress = true;
    },
    $EndProcess: function () {
        member.inProgress = false;
    },
    $GetObjectStoreRW: function () {
        var tr = member.db.transaction("data", "readwrite");
        return tr.objectStore("data");
    },
    $GetObjectStoreR: function () {
        var tr = member.db.transaction("data");
        return tr.objectStore("data");
    },
    $SetRequestCallback: function (req, message, onSuccess = null, onError = null) {
        req.onerror = function (event) {
            console.log("Failed : " + message);
            if (onError != null) {
                onError(event);
            }
            member.isError = true;
            EndProcess();
        }
        req.onsuccess = function (event) {
            console.log("Success : " + message);
            if (onSuccess != null) {
                onSuccess(event);
            }
            member.isError = false;
            EndProcess();
        }
    },

    OpenIndexedDB: function (pdbname) {
        BeginProcess();

        member.isError = false;

        var indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB;

        if (!indexedDB) {
            console.log("Not compatible IndexedDB");
            return;
        }
        var dbname = UTF8ToString(pdbname);
        member.dbname = dbname;

        var request = indexedDB.open(dbname);

        request.onerror = function (_event) {
            member.isError = true;
            console.log("open error");
            EndProcess();
        }
        request.onupgradeneeded = function (event) {
            member.db = event.target.result;
            member.db.createObjectStore("data");
            EndProcess();
        }
        request.onsuccess = function (event) {
            member.db = event.target.result;

            console.log("open success");
            EndProcess();
        }
    },
    CloseIndexedDB: function () {
        if (member.db !== null) {
            member.db.close();
            member.db = null;
        }
    },

    ClearDB: function () {
        if (member.db == null) {
            return;
        }
        BeginProcess();

        var store = GetObjectStoreRW();
        SetRequestCallback(store.clear(), "ClearDB");
    },

    SetBytesRequest: function (pkey, bytesPtr, size) {
        if (member.db == null) {
            return;
        }

        BeginProcess();
        var key = UTF8ToString(pkey);
        var bytes = new Uint8Array(size);
        for (var i = 0; i < size; ++i) {
            bytes[i] = HEAP8[bytesPtr + i];
        }

        var store = GetObjectStoreRW();
        var request = store.put(bytes, key);
        SetRequestCallback(request, "SetBytesRequest", null, function (_event) {
            member.bytes = null;
        });
    },
    GetBytesRequest: function (pkey) {
        if (member.db == null) {
            return;
        }
        BeginProcess();
        var key = UTF8ToString(pkey);

        var store = GetObjectStoreR();
        var request = store.get(key);
        SetRequestCallback(request, "GetBytes", function (event) {
            member.bytes = event.target.result;

        });
    },
    GetBytes: function () {
        if (member.bytes == null) {
            return null;
        }

        var bytes = member.bytes;

        // Add size to first 4 bytes of array
        var ptr = _malloc(bytes.byteLength + 4);
        HEAP32[ptr >> 2] = bytes.byteLength;
        for (var i = 0; i < bytes.byteLength; ++i) {
            HEAPU8[ptr + 4 + i] = bytes[i];
        }
        return ptr;
    },
    FreeBytes: function (ptr) {
        _free(ptr);
    },
    SetJsonRequest: function (pkey, jsonPtr) {
        if (member.db == null) {
            return;
        }
        BeginProcess();
        var key = UTF8ToString(pkey);

        var jsonStr = UTF8ToString(jsonPtr);
        var json = JSON.parse(jsonStr);
        var store = GetObjectStoreRW();
        var request = store.put(json, key);

        SetRequestCallback(request, "SetJsonRequest", null, function (_event) {
            member.json = null;
        });
    },
    GetJsonRequest: function (pkey) {
        if (member.db == null) {
            return;
        }
        BeginProcess();
        var key = UTF8ToString(pkey);

        var store = GetObjectStoreR();
        var request = store.get(key);
        SetRequestCallback(request, "GetJsonRequest", function (event) {
            member.json = event.target.result;
        });
    },
    GetJson: function () {
        if (member.json == null) {
            return "";
        }

        var jsonStr = JSON.stringify(member.json);
        var bufferSize = lengthBytesUTF8(jsonStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(jsonStr, buffer, bufferSize);
        return buffer;
    },
    IsFailed: function () {
        return member.isError;
    },
    InProgress: function () {
        return member.inProgress;
    }
}

autoAddDeps(IndexedDB_Lib, '$member');
autoAddDeps(IndexedDB_Lib, '$BeginProcess');
autoAddDeps(IndexedDB_Lib, '$EndProcess');
autoAddDeps(IndexedDB_Lib, '$GetObjectStoreRW');
autoAddDeps(IndexedDB_Lib, '$GetObjectStoreR');
autoAddDeps(IndexedDB_Lib, '$SetRequestCallback');

mergeInto(LibraryManager.library, IndexedDB_Lib);