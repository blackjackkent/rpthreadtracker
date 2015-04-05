'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('PublicController', [
    '$scope', '$routeParams', '$filter', 'publicThreadService', function($scope, $routeParams, $filter, publicThreadService) {
        $scope.pageId = $routeParams.pageId;
        $scope.userId = $routeParams.userId;
        $scope.currentBlog = $routeParams.currentBlog;
        $scope.currentOrderBy = $routeParams.currentOrderBy;
        $scope.sortDescending = $routeParams.sortDescending;
        $scope.setBodyClass('centered-layout error-page');
        $scope.publicTitleString = buildPublicTitleString();

        function updateThreads(data) {
            $scope.threads = data;
            var filtered = $filter('isCurrentBlog')(data, $scope.currentBlog);
            filtered = $filter('isCorrectTurn')(filtered, $scope.pageId);
            $scope.threadCount = filtered.length;
        }

        function buildPublicTitleString() {
            var result = "";
            if ($scope.pageId == 'yourturn') {
                result += "Threads I Owe";
            } else if ($scope.pageId == 'theirturn') {
                result += "Threads Awaiting Reply";
            } else {
                result += "All Threads";
            }
            if ($scope.currentBlog != "") {
                result += " on " + $scope.currentBlog;
            }
            return result;
        }

        publicThreadService.subscribe(updateThreads);
        publicThreadService.getThreads($scope.userId, $scope.currentBlog);
    }
]);