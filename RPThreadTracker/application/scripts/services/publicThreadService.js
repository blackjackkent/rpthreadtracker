'use strict';
(function() {
	angular.module('rpthreadtracker')
        .factory('publicThreadService',
		[
			'$q', '$http', publicThreadService
		]);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function publicThreadService($q, $http) {
		var loadedThreadEventSubscribers = [],
			threads = [];

		function loadThreads(userId, blogShortname, isArchive) {
			threads = [];
			getThreadIds(userId, blogShortname, isArchive).then(function(data) {
				loadThreadIdsSuccess(data, threads);
			});
		}

		function loadThreadIdsSuccess(ids) {
			angular.forEach(ids, function(value) {
				getThread(value);
			});
		}

		function getThreadIds(userId, blogShortname, isArchived) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/PublicThread?userId=' + userId 
						+ '&blogShortname=' + blogShortname 
						+ '&isArchived=' + isArchived,
					'method': 'GET'
				};
			function success(response) {
				deferred.resolve(response.data);
			}
			function error(data) {
				deferred.reject(data);
			}
			$http(config).then(success).catch(error);
			return deferred.promise;
		}

		function getThread(id) {
			var config = {
				'url': '/api/PublicThread/' + id,
				'method': 'GET'
			};
			function success(response) {
				threads.push(response.data);
				broadcastLoadedThreadEvent(threads);
			}
			$http(config).then(success);
		}

		function subscribeLoadedThreadEvent(callback) {
			loadedThreadEventSubscribers.push(callback);
		}

		function unsubscribeLoadedThreadEvent(callback) {
			var index = loadedThreadEventSubscribers.indexOf(callback);
			if (index > -1) {
				loadedThreadEventSubscribers.splice(index, 1);
			}
		}

		function broadcastLoadedThreadEvent(data) {
			angular.forEach(loadedThreadEventSubscribers, function(callback) {
				callback(data);
			});
		}

		return {
			'subscribeLoadedThreadEvent': subscribeLoadedThreadEvent,
			'unsubscribeLoadedThreadEvent': unsubscribeLoadedThreadEvent,
			'loadThreads': loadThreads
		};
	}
}());
