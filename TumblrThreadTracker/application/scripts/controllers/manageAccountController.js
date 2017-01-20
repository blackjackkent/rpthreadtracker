(function() {
	'use strict';
	angular.module('rpthreadtracker')
		.controller('ManageAccountController',
		[
			'$scope', '$http', '$location', 'sessionService', 'exportService', 'pageId', 'TrackerNotification',
			manageAccountController
		]);

	function manageAccountController($scope, $http, $location, sessionService, exportService, pageId, TrackerNotification) {
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
				.withMessage('Account updated.')
				.withType('success')
				.show();
		}

		function passwordFailure() {
			new TrackerNotification()
				.withMessage('There was a problem updating your account.')
				.withType('error')
				.show();
		}

		function changePassword() {
			if (!$scope.changePasswordForm.$valid) {
				showChangePasswordValidationError();
				return;
			}
			sessionService.changePassword($scope.oldPassword, $scope.newPassword, $scope.confirmNewPassword)
				.then(passwordSuccess, passwordFailure);
		}

		function showChangePasswordValidationError() {
			var notification = new TrackerNotification()
				.withType('error')
				.withMessage('');
			if ($scope.changePasswordForm.oldPassword.$error.required) {
				notification.appendMessage('You must enter your current password.');
			}
			if ($scope.changePasswordForm.newPassword.$error.required) {
				notification.appendMessage('You must enter your new password.');
			}
			if ($scope.changePasswordForm.confirmNewPassword.$error.required) {
				notification.appendMessage('You must confirm your new password.');
			}
			if ($scope.newPassword != $scope.confirmNewPassword) {
				notification.appendMessage('Your new passwords must match.');
			}
			notification.show();
		}

		function exportThreads() {
			$scope.exportLoading = true;
			exportService.exportThreads($scope.includeArchived, $scope.includeHiatused)
				.then(exportSuccess, exportFailure);
		}

		function exportSuccess() {
			$scope.exportLoading = false;
		}

		function exportFailure() {
			new TrackerNotification()
				.withMessage('There was a problem exporting your threads.')
				.withType('error')
				.show();
			$scope.exportLoading = false;
		}
	}
})();