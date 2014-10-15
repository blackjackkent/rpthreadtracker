'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('StaticController', [
    '$scope', 'sessionService', 'pageId', function($scope, sessionService, pageId) {
        $scope.setBodyClass('');
        $scope.pageId = pageId;
        $scope.publicView = true;
        sessionService.isLoggedIn().then(function(isLoggedIn) {
            if (isLoggedIn) {
                sessionService.getUser().then(function(user) {
                    $scope.userId = user.UserId;
                    $scope.user = user;
                });
            }
        });
    }
]);