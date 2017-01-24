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
			if (threads.length === 0 || force) {
				broadcastLoadedThreadEvent(threads);
				broadcastAllThreadsLoaded();
			}
			threads = [];
			getThreadIds().then(loadThreadIdsSuccess, loadThreadIdsFailure);
		}

		function loadThreadIdsSuccess(ids) {
			var queue = [];
			angular.forEach(ids, function(value) {
				queue.push(getThread(value));
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
		
		function getThread(id) {
			var deferred = $q.defer(),
				config = {
					url: "/api/Thread/" + id,
					method: "GET"
				},
				success = function(response) {
					threads.push(response.data);
					broadcastLoadedThreadEvent(threads);
					deferred.resolve(true);
				};
			$http(config).then(success);
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

		function unsubscribeOnComplete(callback) {
			var index = allThreadsLoadedSubscribers.indexOf(callback);
			if (index > -1) {
				allThreadsLoadedSubscribers.splice(index, 1);
			}
		}

		function unsubscribeOnArchiveUpdate(callback) {
			var index = loadedArchiveThreadEventSubscribers.indexOf(callback);
			if (index > -1) {
				loadedArchiveThreadEventSubscribers.splice(index, 1);
			}
		}

		return {
			flushThreads: flushThreads,
			loadThreads: loadThreads,
			subscribeLoadedThreadEvent: subscribeLoadedThreadEvent,
			subscribeAllThreadsLoaded: subscribeAllThreadsLoaded,
			subscribeLoadedArchiveThreadEvent: subscribeLoadedArchiveThreadEvent,
			unsubscribeLoadedThreadEvent: unsubscribeLoadedThreadEvent,
			unsubscribeOnComplete: unsubscribeOnComplete,
			unsubscribeOnArchiveUpdate: unsubscribeOnArchiveUpdate
		};
	}
})();