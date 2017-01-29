(function() {
    "use strict";
    angular.module("rpthreadtracker")
        .service("publicThreadService",
        [
            "$q", "$http", publicThreadService
        ]);

    function publicThreadService($q, $http) {
        var loadedThreadEventSubscribers = [],
            threads = [];

		function loadThreads(userId, blogShortname) {
			threads = [];
			getThreadIds(userId, blogShortname).then(function(data) {
				loadThreadIdsSuccess(data, threads);
			});
		}

		function loadThreadIdsSuccess(ids) {
			angular.forEach(ids, function(value) {
                getThread(value);
            });
		}

        function getThreadIds(userId, blogShortname) {
            var deferred = $q.defer(),
                config = {
                    url: "/api/PublicThread?userId=" + userId + "&blogShortname=" + blogShortname,
                    method: "GET"
                },
                success = function(response) {
                    deferred.resolve(response.data);
                },
                error = function(data) {
                    deferred.reject(data);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        }

        function getThreads(userId, blogShortname, force) {
            if (threads.length > 0 && !force) {
                broadcast(threads);
                return;
            }
            threads = [];
            getThreadIds(userId, blogShortname)
                .then(function(ids) {
                    angular.forEach(ids,
                        function(value, key) {
                            getThread(value);
                        });
                });
        };

        function getThread(id) {
            var config = {
                    url: "/api/PublicThread/" + id,
                    method: "GET"
                },
                success = function(response) {
                    threads.push(response.data);
                    broadcastLoadedThreadEvent(threads);
                };
            $http(config).then(success);
        };

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
            angular.forEach(loadedThreadEventSubscribers,
                function(callback) {
                    callback(data);
                });
        }

        return {
            subscribeLoadedThreadEvent: subscribeLoadedThreadEvent,
            unsubscribeLoadedThreadEvent: unsubscribeLoadedThreadEvent,
            loadThreads: loadThreads
        };
    }
})();