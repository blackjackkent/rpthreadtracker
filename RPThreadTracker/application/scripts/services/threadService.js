'use strict';
(function() {
	angular.module('rpthreadtracker')
		.factory('threadService',
		[
			'$q', '$http', threadService
		]);
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function threadService($q, $http) {
		var loadedThreadEventSubscribers = [],
			allThreadsLoadedSubscribers = [],
			loadedArchiveThreadEventSubscribers = [],
			threads = [],
			archivedThreads = [];

		function flushThreads() {
			threads = [];
			archivedThreads = [];
		}

		function loadThreads(force) {
			if (threads.length !== 0 && !force) {
				broadcastLoadedThreadEvent(threads);
				broadcastAllThreadsLoaded();
				return;
			}
			threads = [];
			getThreadIds().then(function(data) {
				loadThreadIdsSuccess(data, threads);
			}, loadThreadIdsFailure);
		}

		function loadArchivedThreads(force) {
			if (archivedThreads.length !== 0 && !force) {
				broadcastLoadedArchiveThreadEvent(archivedThreads);
				broadcastAllThreadsLoaded();
				return;
			}
			archivedThreads = [];
			getThreadIds(true).then(function(data) {
				loadThreadIdsSuccess(data, archivedThreads);
			}, loadThreadIdsFailure);
		}

		function loadThreadIdsSuccess(ids, threadArray) {
			var queue = [];
			angular.forEach(ids, function(value) {
				queue.push(getThread(value, threadArray));
			});
			$q.all(queue).then(function() {
				broadcastAllThreadsLoaded();
			});
		}

		function loadThreadIdsFailure() {
			broadcastAllThreadsLoaded();
		}

		function getThreadIds(isArchived, isHiatused) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/Thread?isArchived=' + isArchived + '&isHiatusedBlog=' + isHiatused,
					'method': 'GET'
				};
			function success(response) {
				deferred.resolve(response.data);
			}
			function error(data) {
				deferred.reject(data);
			}
			$http(config).then(success, error);
			return deferred.promise;
		}

		function getThread(id, threadArray, isArchived) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/Thread/' + id,
					'method': 'GET'
				};
			function success(response) {
				threadArray.push(response.data);
				if (isArchived) {
					broadcastLoadedArchiveThreadEvent(threadArray);
				} else {
					broadcastLoadedThreadEvent(threadArray);
				}
				deferred.resolve(true);
			}
			function error() {
				deferred.reject(false);
			}
			$http(config).then(success, error);
			return deferred.promise;
		}

		function getStandaloneThread(id) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/Thread/' + id,
					'method': 'GET'
				};
			function success(response) {
				deferred.resolve(response.data);
			}
			function error() {
				deferred.reject(false);
			}
			$http(config).then(success, error);
			return deferred.promise;
		}

		function addNewThread(thread) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/Thread',
					'data': thread,
					'method': 'POST'
				};
			function success() {
				deferred.resolve(true);
			}
			function error() {
				deferred.reject(false);
			}
			$http(config).then(success, error);
			return deferred.promise;
		}

		function editThread(thread) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/Thread',
					'data': thread,
					'method': 'PUT'
				};
			function success() {
				deferred.resolve(true);
			}
			function error() {
				deferred.reject(false);
			}
			$http(config).then(success, error);
			return deferred.promise;
		}

		function untrackThreads(threads) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/Thread/Delete',
					'data': threads,
					'method': 'PUT'
				};
			function success() {
				deferred.resolve(true);
			}
			function error() {
				deferred.reject(false);
			}
			$http(config).then(success, error);
			return deferred.promise;
		}

		function archiveThreads(threads) {
			var deferred = $q.defer();
			var queue = [];
			angular.forEach(threads, function(thread) {
				thread.IsArchived = true;
				queue.push(editThread(thread));
			});
			$q.all(queue).then(function() {
				deferred.resolve(true);
			});
			return deferred.promise;
		}

		function unarchiveThreads(threads) {
			var deferred = $q.defer();
			var queue = [];
			angular.forEach(threads, function(thread) {
				thread.IsArchived = false;
				queue.push(editThread(thread));
			});
			$q.all(queue).then(function() {
				deferred.resolve(true);
			});
			return deferred.promise;
		}

		function broadcastLoadedThreadEvent(data) {
			angular.forEach(loadedThreadEventSubscribers,
				function(callback) {
					callback(data);
				});
		}

		function broadcastAllThreadsLoaded() {
			angular.forEach(allThreadsLoadedSubscribers,
				function(callback) {
					callback();
				});
		}

		function broadcastLoadedArchiveThreadEvent(data) {
			angular.forEach(loadedArchiveThreadEventSubscribers,
				function(callback) {
					callback(data);
				});
		}

		function subscribeLoadedThreadEvent(callback) {
			loadedThreadEventSubscribers.push(callback);
		}

		function subscribeAllThreadsLoaded(callback) {
			allThreadsLoadedSubscribers.push(callback);
		}

		function subscribeLoadedArchiveThreadEvent(callback) {
			loadedArchiveThreadEventSubscribers.push(callback);
		}

		function unsubscribeLoadedThreadEvent(callback) {
			var index = loadedThreadEventSubscribers.indexOf(callback);
			if (index > -1) {
				loadedThreadEventSubscribers.splice(index, 1);
			}
		}

		function unsubscribeAllThreadsLoaded(callback) {
			var index = allThreadsLoadedSubscribers.indexOf(callback);
			if (index > -1) {
				allThreadsLoadedSubscribers.splice(index, 1);
			}
		}

		function unsubscribeLoadedArchiveThreadEvent(callback) {
			var index = loadedArchiveThreadEventSubscribers.indexOf(callback);
			if (index > -1) {
				loadedArchiveThreadEventSubscribers.splice(index, 1);
			}
		}

		return {
			'flushThreads': flushThreads,
			'loadThreads': loadThreads,
			'loadArchivedThreads': loadArchivedThreads,
			'untrackThreads': untrackThreads,
			'archiveThreads': archiveThreads,
			'unarchiveThreads': unarchiveThreads,
			'addNewThread': addNewThread,
			'editThread': editThread,
			'getStandaloneThread': getStandaloneThread,
			'subscribeLoadedThreadEvent': subscribeLoadedThreadEvent,
			'subscribeAllThreadsLoaded': subscribeAllThreadsLoaded,
			'subscribeLoadedArchiveThreadEvent': subscribeLoadedArchiveThreadEvent,
			'unsubscribeLoadedThreadEvent': unsubscribeLoadedThreadEvent,
			'unsubscribeAllThreadsLoaded': unsubscribeAllThreadsLoaded,
			'unsubscribeLoadedArchiveThreadEvent': unsubscribeLoadedArchiveThreadEvent
		};
	}
}());
