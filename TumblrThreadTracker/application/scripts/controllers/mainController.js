'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('MainController', [
    '$scope', '$location', '$analytics', 'threadService', 'contextService', 'blogService', 'newsService', 'sessionService', 'pageId',
    function($scope, $location, $analytics, threadService, contextService, blogService, newsService, sessionService, pageId) {

        $scope.setBodyClass('');

        function updateThreads(data) {
            $scope.threads = data;
            $scope.myTurnCount = 0;
            $scope.theirTurnCount = 0;
            angular.forEach($scope.threads, function(thread) {
                $scope.myTurnCount += thread.IsMyTurn ? 1 : 0;
                $scope.theirTurnCount += thread.IsMyTurn ? 0 : 1;
            });
        }

        function getThreadById(id) {
            var result;
            angular.forEach($scope.threads, function (thread) {
                if (thread.UserThreadId == id) {
                    result = thread;
                    return;
                }
            });
            return result;
        }

        $scope.setCurrentBlog = function() {
            contextService.setCurrentBlog($scope.currentBlog);
            $analytics.eventTrack('Change Current Blog', { category: 'Private Thread View' });
        };
        $scope.setSortDescending = function() {
            contextService.setSortDescending($scope.sortDescending);
            $analytics.eventTrack('Change Sort Descending', { category: 'Private Thread View' });
        };
        $scope.setCurrentOrderBy = function() {
            contextService.setCurrentOrderBy($scope.currentOrderBy);
            $analytics.eventTrack('Change Order By', { category: 'Private Thread View' });
        };
        $scope.bulkAction = function() {
            var bulkAffected = [];
            for (var property in $scope.bulkItems) {
                if ($scope.bulkItems.hasOwnProperty(property) && $scope.bulkItems[property] == true) {
                    bulkAffected.push(property);
                }
            }
            if ($scope.bulkItemAction == "UntrackSelected") {
                $scope.untrackThreads(bulkAffected);
            } else if ($scope.bulkItemAction == "ArchiveSelected") {
                $scope.archiveThreads(bulkAffected);
            } else if ($scope.bulkItemAction == "UnarchiveSelected") {
                $scope.unarchiveThreads(bulkAffected);
            }
        }
        $scope.pageId = pageId;
        $scope.displayPublicUrl = true;
        $scope.dashboardFilter = 'yourturn';
        $scope.bulkItems = {};
        $scope.bulkItemAction = "UntrackSelected";

        if (pageId == "archived") {
            threadService.subscribeOnArchiveUpdate(updateThreads);
            threadService.getArchive();
        } else {
            threadService.subscribe(updateThreads);
            threadService.getThreads();
        }
        $scope.currentBlog = contextService.getCurrentBlog();
        $scope.sortDescending = contextService.getSortDescending();
        $scope.currentOrderBy = contextService.getCurrentOrderBy();
        sessionService.getUser().then(function(user) {
            $scope.userId = user.UserId;
            $scope.user = user;
        });
        blogService.getBlogs().then(function(blogs) {
            $scope.blogs = blogs;
        });

        newsService.getNews().then(function(news) {
            $scope.news = news;
        });

        $scope.untrackThreads = function (userThreadIds) {
            threadService.flushThreads();
            threadService.untrackThreads(userThreadIds).then(function() {
                threadService.getThreads();
                threadService.getArchive();
            });
            $scope.genericSuccess = "Threads untracked.";
        };
        $scope.archiveThreads = function(userThreadIds) {
            threadService.flushThreads();
            var threadsToArchive = [];
            angular.forEach(userThreadIds, function (id) {
                threadsToArchive.push(getThreadById(id));
            });
            threadService.editThreads(threadsToArchive, true).then(function () {
                threadService.getThreads();
                threadService.getArchive();
            });
            $scope.genericSuccess = "Threads archived.";
        };
        $scope.unarchiveThreads = function (userThreadIds) {
            threadService.flushThreads();
            var threadsToArchive = [];
            angular.forEach(userThreadIds, function (id) {
                threadsToArchive.push(getThreadById(id));
            });
            threadService.editThreads(threadsToArchive, false).then(function() {
                threadService.getThreads();
                threadService.getArchive();
            });
            $scope.genericSuccess = "Threads unarchived.";
        };
        $scope.refreshThreads = function () { threadService.getThreads(true); };
        $scope.setDashboardFilter = function(filterString) {
            $scope.dashboardFilter = filterString;
            $analytics.eventTrack('Set Recent to ' + filterString, { category: 'Dashboard' });
        };

        $scope.$on("$destroy", function() {
            threadService.unsubscribe(updateThreads);
            threadService.unsubscribeOnArchiveUpdate(updateThreads);
        });
    }
]);