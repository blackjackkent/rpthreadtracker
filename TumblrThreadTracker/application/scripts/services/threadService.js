(function() {
	"use strict";
	angular.module("rpthreadtracker")
		.service("threadService",
		[
			"$q", "$http", threadService
		]);

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

		function getThreadIds(isArchived) {
			var deferred = $q.defer(),
				config = {
					url: "/api/Thread?isArchived=" + isArchived,
					method: "GET"
				},
				success = function(response) {
					deferred.resolve(response.data);
				},
				error = function(data) {
					deferred.reject(data);
				};
			$http(config).then(success, error);
			return deferred.promise;
		}
		
		function getThread(id, threadArray, isArchived) {
			var deferred = $q.defer(),
				config = {
					url: "/api/Thread/" + id,
					method: "GET"
				},
				success = function(response) {
					threadArray.push(response.data);
					if (isArchived) {
						broadcastLoadedArchiveThreadEvent(threadArray);
					} else {
						broadcastLoadedThreadEvent(threadArray);
					}
					deferred.resolve(true);
				},
				error = function(response) {
					console.log(response);
				};
			$http(config).then(success, error);
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
			flushThreads: flushThreads,
			loadThreads: loadThreads,
			loadArchivedThreads: loadArchivedThreads,
			subscribeLoadedThreadEvent: subscribeLoadedThreadEvent,
			subscribeAllThreadsLoaded: subscribeAllThreadsLoaded,
			subscribeLoadedArchiveThreadEvent: subscribeLoadedArchiveThreadEvent,
			unsubscribeLoadedThreadEvent: unsubscribeLoadedThreadEvent,
			unsubscribeAllThreadsLoaded: unsubscribeAllThreadsLoaded,
			unsubscribeLoadedArchiveThreadEvent: unsubscribeLoadedArchiveThreadEvent
		};
	}
})();