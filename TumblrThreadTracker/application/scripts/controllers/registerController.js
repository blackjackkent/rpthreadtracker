'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('RegisterController', [
    '$scope', '$location', 'sessionService', function($scope, $location, sessionService) {
        var success = function () {
            sessionService.login($scope.username, $scope.Password)
                .then(function() {
                    $scope.loading = false;
                    $location.path('/');
                });
            },
            fail = function (response) {
                $scope.loading = false;
                if (response && response.data) {
                    $scope.genericError = response.data;
                } else {
                    $scope.genericError = "Error registering account. Please try again later.";
                }
            };
        $scope.setBodyClass('signin-page');

        $scope.register = function () {
            $scope.loading = true;
            $scope.error = "";
            if (!$scope.registerForm.$valid || $scope.confirmPassword != $scope.Password) {
                return;
            }
            sessionService.register($scope.username, $scope.email, $scope.Password, $scope.confirmPassword).then(success, fail);
        };
    }
]);