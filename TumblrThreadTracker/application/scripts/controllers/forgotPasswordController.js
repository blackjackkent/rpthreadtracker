'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('ForgotPasswordController', [
    '$scope', '$location', 'sessionService', function($scope, $location, sessionService) {
        var success = function () {
                $scope.loading = false;
                $scope.success = "Success. Check your email box for a temporary password.";
            },
            fail = function () {
                $scope.loading = false;
                $scope.error = "Unknown error. Please try again later.";
            };
        $scope.setBodyClass('signin-page');

        $scope.submitForgotPassword = function() {
            $scope.error = "";
            $scope.success = "";
            $scope.loading = true;
            sessionService.submitForgotPassword($scope.username).then(success, fail);
        };
    }
]);