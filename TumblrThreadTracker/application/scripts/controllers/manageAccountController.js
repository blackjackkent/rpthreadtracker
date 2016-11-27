'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('ManageAccountController', [
    '$scope', '$http', '$location', 'sessionService', 'exportService', 'pageId', function ($scope, $http, $location, sessionService, exportService, pageId) {
        $scope.setBodyClass('');

        function passwordSuccess() {
            $scope.changePasswordForm.$setPristine();
            $scope.oldPassword = '';
            $scope.newPassword = '';
            $scope.confirmNewPassword = '';
            $scope.showSuccessMessage = true;
            $scope.genericError = '';
        }

        function passwordFailure() {
            $scope.genericError = "There was a problem updating your account.";
            $scope.showSuccessMessage = false;
        }

        $scope.pageId = pageId;
        $scope.changePassword = function() {
            if (!$scope.changePasswordForm.$valid) {
                return;
            }
            sessionService.changePassword($scope.oldPassword, $scope.newPassword, $scope.confirmNewPassword).then(passwordSuccess, passwordFailure);
        };
        $scope.exportThreads = function() {
            $scope.exportError = "";
            $scope.exportLoading = true;
            exportService.exportThreads($scope.includeArchived, $scope.includeHiatused).then(exportSuccess, exportFailure);
        }
        function exportSuccess() {
            $scope.exportError = "";
            $scope.exportLoading = false;
        }

        function exportFailure() {
            $scope.exportError = "There was a problem exporting your threads.";
            $scope.exportLoading = false;
        }

        sessionService.getUser().then(function(user) {
            $scope.userId = user.UserId;
            $scope.user = user;
        });
    }
]);