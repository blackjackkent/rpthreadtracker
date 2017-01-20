(function() {
    "use strict";
    angular.module("rpthreadtracker")
        .service("exportService",
        [
            "$q", "$http", exportService
        ]);

    function exportService($q, $http) {

        var exportThreads = function(includeArchived, includeHiatused) {
            var deferred = $q.defer();
            //adapted from http://stackoverflow.com/a/24129082
            var httpPath = "/api/Export?includeArchived=" + includeArchived + "&includeHiatused=" + includeHiatused;
            // Use an arraybuffer
            $http.get(httpPath, { responseType: "arraybuffer" })
                .success(function(data, status, headers) {
                    var octetStreamMime = "application/octet-stream";
                    var success = false;

                    // Get the headers
                    headers = headers();
                    // Get the filename from the x-filename header or default to "download.bin"
                    var filename = headers["x-filename"] || "download.bin";
                    // Determine the content type from the header or default to "application/octet-stream"
                    var contentType = headers["content-type"] || octetStreamMime;

                    try {
                        // Try using msSaveBlob if supported
                        var blob = new Blob([data], { type: contentType });
                        if (navigator.msSaveBlob)
                            navigator.msSaveBlob(blob, filename);
                        else {
                            // Try using other saveBlob implementations, if available
                            var saveBlob = navigator.webkitSaveBlob || navigator.mozSaveBlob || navigator.saveBlob;
                            if (saveBlob === undefined) throw "Not supported";
                            saveBlob(blob, filename);
                        }
                        success = true;
                    } catch (ex) {
                        console.log("saveBlob method failed with the following exception:");
                        console.log(ex);
                    }

                    if (!success) {
                        // Get the blob url creator
                        var urlCreator = window.URL || window.webkitURL || window.mozURL || window.msURL;
                        if (urlCreator) {
                            // Try to use a download link
                            var link = document.createElement("a");
                            if ("download" in link) {
                                // Try to simulate a click
                                try {
                                    // Prepare a blob URL
                                    var blob = new Blob([data], { type: contentType });
                                    var url = urlCreator.createObjectURL(blob);
                                    link.setAttribute("href", url);

                                    // Set the download attribute (Supported in Chrome 14+ / Firefox 20+)
                                    link.setAttribute("download", filename);

                                    // Simulate clicking the download link
                                    var event = document.createEvent("MouseEvents");
                                    event
                                        .initMouseEvent("click",
                                            true,
                                            true,
                                            window,
                                            1,
                                            0,
                                            0,
                                            0,
                                            0,
                                            false,
                                            false,
                                            false,
                                            false,
                                            0,
                                            null);
                                    link.dispatchEvent(event);
                                    success = true;
                                } catch (ex) {
                                    console
                                        .log("Download link method with simulated click failed with the following exception:");
                                    console.log(ex);
                                }
                            }

                            if (!success) {
                                // Fallback to window.location method
                                try {
                                    // Prepare a blob URL
                                    // Use application/octet-stream when using window.location to force download
                                    var blob = new Blob([data], { type: octetStreamMime });
                                    var url = urlCreator.createObjectURL(blob);
                                    window.location = url;
                                    success = true;
                                } catch (ex) {
                                    console
                                        .log("Download link method with window.location failed with the following exception:");
                                    console.log(ex);
                                }
                            }

                        }
                    }

                    if (!success) {
                        // Fallback to window.open method
                        console.log("No methods worked for saving the arraybuffer, using last resort window.open");
                        window.open(httpPath, "_blank", "");
                    }
                    deferred.resolve();
                })
                .error(function(data, status) {
                    deferred.reject();
                });
            return deferred.promise;
        };
        return {
            exportThreads: exportThreads
        };
    }
})();