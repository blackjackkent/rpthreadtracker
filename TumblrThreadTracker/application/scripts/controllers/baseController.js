'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('BaseController', [
    '$scope', 'sessionService', function ($scope, sessionService) {
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