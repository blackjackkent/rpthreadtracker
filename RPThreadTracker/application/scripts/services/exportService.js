'use strict';
(function() {
	angular.module('rpthreadtracker')
		.factory('exportService',
		[
			'$q', '$http', exportService
		]);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function exportService($q, $http) {
		function exportThreads(includeArchived, includeHiatused) {
			var deferred = $q.defer();
			/* eslint-disable */
			// Adapted from http://stackoverflow.com/a/24129082
			var httpPath = '/api/Export?includeArchived=' + 
				includeArchived + '&includeHiatused=' + includeHiatused;
			$http.get(httpPath, {'responseType': 'arraybuffer'}).then(function(response) {
				var octetStreamMime = 'application/octet-stream';
				var success = false;
				var headers = response.headers();
				var filename = headers['x-filename'] || 'download.bin';
				var contentType = headers['content-type'] || octetStreamMime;
				try {
					var blob = new Blob([response.data], {'type': contentType});
					if (navigator.msSaveBlob) {
						navigator.msSaveBlob(blob, filename);
					} else {
						var saveBlob = navigator.webkitSaveBlob|| navigator.mozSaveBlob || navigator.saveBlob;
						if (saveBlob === undefined) {
							throw 'Not supported';
						}
						saveBlob(blob, filename);
					}
					success = true;
				} catch (ex) {
					console.log('saveBlob method failed with the following exception:');
					console.log(ex);
				}

				if (!success) {
					var urlCreator = window.URL || window.webkitURL || window.mozURL || window.msURL;
					if (urlCreator) {
						var link = document.createElement('a');
						if ('download' in link) {
							try {
								var blob = new Blob([response.data], {'type': contentType});
								var url = urlCreator.createObjectURL(blob);
								link.setAttribute('href', url);
								link.setAttribute('download', filename);
								var event = document.createEvent('MouseEvents');
								event.initMouseEvent('click', true, true, window, 1, 0,
									0, 0, 0, false, false, false, false, 0, null);
								link.dispatchEvent(event);
								success = true;
							} catch (ex) {
								console.log('Download link method with simulated click failed with the following exception:');
								console.log(ex);
							}
						}

						if (!success) {
							try {
								var blob = new Blob([response.data], {'type': octetStreamMime});
								var url = urlCreator.createObjectURL(blob);
								window.location = url;
								success = true;
							} catch (ex) {
								console.log('Download link method with window.location failed with the following exception:');
								console.log(ex);
							}
						}
					}
				}
				if (!success) {
					// Fallback to window.open method
					console.log('No methods worked for saving the arraybuffer, using last resort window.open');
					window.open(httpPath, '_blank', '');
				}
				deferred.resolve();
			},
			function() {
				deferred.reject();
			});
			/* eslint-enable */
			return deferred.promise;
		}
		return {'exportThreads': exportThreads};
	}
}());
