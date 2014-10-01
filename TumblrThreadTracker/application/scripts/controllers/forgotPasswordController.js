'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('ForgotPasswordController', [
    '$scope', '$location', 'sessionService', function($scope, $location, sessionService) {
        var success = function() {
                $scope.success = "Success. Check your email box for a temporary password.";
            },
            fail = function() {
                $scope.error = "Unknown error. Please try again later.";
            };
        $scope.setBodyClass('signin-page');

        $scope.submitForgotPassword = function() {
            $scope.error = "";
            $scope.success = "";
            sessionService.submitForgotPassword($scope.username).then(success, fail);
        };
    }
]);