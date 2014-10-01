'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('ManageAccountController', [
    '$scope', '$location', 'sessionService', 'blogService', 'threadService', 'pageId', function($scope, $location, sessionService, blogService, threadService, pageId) {
        $scope.setBodyClass('');

        function success() {
            $scope.changePasswordForm.$setPristine();
            $scope.oldPassword = '';
            $scope.newPassword = '';
            $scope.confirmNewPassword = '';
            $scope.showSuccessMessage = true;
            $scope.genericError = '';
        }

        function failure() {
            $scope.genericError = "There was a problem updating your account.";
            $scope.showSuccessMessage = false;
        }

        $scope.pageId = pageId;
        $scope.changePassword = function() {
            if (!$scope.changePasswordForm.$valid) {
                return;
            }
            sessionService.changePassword($scope.oldPassword, $scope.newPassword, $scope.confirmNewPassword).then(success, failure);
        };
        sessionService.getUser().then(function(user) {
            $scope.userId = user.UserId;
            $scope.user = user;
        });
    }
]);