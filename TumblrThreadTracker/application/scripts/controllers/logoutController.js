'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('LogoutController', [
    '$scope', '$location', 'blogService', 'threadService', 'sessionService', function($scope, $location, blogService, threadService, sessionService) {
        blogService.flushBlogs();
        threadService.flushThreads();
        $scope.user = null;
        $scope.userId = null;
        sessionService.logout();
        $location.path('/');
    }
]);