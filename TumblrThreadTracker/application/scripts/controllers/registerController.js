'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('RegisterController', [
    '$scope', '$location', 'sessionService', function($scope, $location, sessionService) {
        var success = function() {
                $location.path('/');
            },
            fail = function() {
                $scope.genericError = "Error registering account. Please try again later.";
            };
        $scope.setBodyClass('signin-page');

        $scope.register = function() {
            $scope.error = "";
            if (!$scope.registerForm.$valid || $scope.confirmPassword != $scope.Password) {
                return;
            }
            sessionService.register($scope.username, $scope.email, $scope.Password, $scope.confirmPassword).then(success, fail);
        };
    }
]);