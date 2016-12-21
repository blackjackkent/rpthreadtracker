'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('LogoutController', [
    '$scope', '$location', 'blogService', 'threadService', 'sessionService', function($scope, $location, blogService, threadService, sessionService) {
        var success = function() {
            $location.path('/');
        };
        blogService.flushBlogs();
        threadService.flushThreads();
        $scope.user = null;
        $scope.userId = null;
        sessionService.logout();//.then(success);
        success();
    }
]);