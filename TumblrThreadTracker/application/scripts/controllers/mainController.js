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
        $scope.pageId = pageId;
        $scope.displayPublicUrl = true;
        $scope.dashboardFilter = 'yourturn';

        threadService.subscribe(updateThreads);
        threadService.getThreads();
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

        $scope.untrackThread = function (userThreadId, threadTitle) {
            threadService.flushThreads();
            threadService.untrackThread(userThreadId).then(function() { threadService.getThreads(); });
            $scope.genericSuccess = threadTitle + " has been untracked.";
        };
        $scope.refreshThreads = function () { threadService.getThreads(true); };
        $scope.setDashboardFilter = function(filterString) {
            $scope.dashboardFilter = filterString;
            $analytics.eventTrack('Set Recent to ' + filterString, { category: 'Dashboard' });
        };

        $scope.$on("$destroy", function() {
            threadService.unsubscribe(updateThreads);
        });
    }
]);