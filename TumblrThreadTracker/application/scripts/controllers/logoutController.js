'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('LogoutController', [
    '$scope', '$location', 'blogService', 'threadService', 'sessionService', function($scope, $location, blogService, threadService, sessionService) {
        var success = function() {
            $location.path('/');
        };
        blogService.flushBlogs();
        threadService.flushThreads();
        sessionService.logout();//.then(success);
        success();
    }
]);