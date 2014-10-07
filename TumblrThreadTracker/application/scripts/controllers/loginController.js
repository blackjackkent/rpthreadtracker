'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('LoginController', [
    '$scope', '$location', 'sessionService', function($scope, $location, sessionService) {
        var success = function() {
                $location.path('/');
            },
            fail = function() {
                $scope.error = "Incorrect username or password.";
            };
        sessionService.isLoggedIn().then(function(isLoggedIn) {
            if (isLoggedIn) {
                $location.path('/');
            }
        });
        $scope.setBodyClass('signin-page');

        $scope.login = function ($element) {
            $scope.username = loginForm.username.value;
            $scope.password = loginForm.password.value;
            sessionService.login($scope.username, $scope.password).then(success, fail);
        };
    }
]);