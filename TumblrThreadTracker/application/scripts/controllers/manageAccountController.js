'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('ManageAccountController', [
    '$scope', '$http', '$location', 'sessionService', 'exportService', 'pageId', 'TrackerNotification', function ($scope, $http, $location, sessionService, exportService, pageId, TrackerNotification) {
        $scope.setBodyClass('');
        $scope.pageId = pageId;
        $scope.changePassword = changePassword;
        $scope.exportThreads = exportThreads;

        function passwordSuccess() {
            $scope.changePasswordForm.$setPristine();
            $scope.oldPassword = '';
            $scope.newPassword = '';
            $scope.confirmNewPassword = '';
            new TrackerNotification()
                .withMessage("Account updated.")
                .withType("success")
                .show();
        }
        function passwordFailure() {
            new TrackerNotification()
                .withMessage("There was a problem updating your account.")
                .withType("error")
                .show();
        }
        function changePassword() {
            if (!$scope.changePasswordForm.$valid) {
                return;
            }
            sessionService.changePassword($scope.oldPassword, $scope.newPassword, $scope.confirmNewPassword).then(passwordSuccess, passwordFailure);
        };
        function exportThreads() {
            $scope.exportLoading = true;
            exportService.exportThreads($scope.includeArchived, $scope.includeHiatused).then(exportSuccess, exportFailure);
        }
        function exportSuccess() {
            $scope.exportLoading = false;
        }
        function exportFailure() {
            new TrackerNotification()
                .withMessage("There was a problem exporting your threads.")
                .withType("error")
                .show();
            $scope.exportLoading = false;
        }
    }
]);