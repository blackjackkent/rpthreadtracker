'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('ManageAccountController',
		[
			'$scope', '$controller', '$location', 'sessionService', 'exportService', 'pageId',
			'TrackerNotification', 'BodyClass', manageAccountController
		]);

	/** @this manageAccountController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function manageAccountController($scope, $controller, $location, sessionService, exportService, pageId, TrackerNotification, BodyClass) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		sessionService.loadUser(vm);
		BodyClass.set('');
		vm.pageId = pageId;
		vm.changePassword = changePassword;
		vm.exportThreads = exportThreads;
		vm.includeHiatused = false;
		vm.includeArchived = false;

		function passwordSuccess() {
			vm.changePasswordForm.$setPristine();
			vm.oldPassword = '';
			vm.newPassword = '';
			vm.confirmNewPassword = '';
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
			if (!vm.changePasswordForm.$valid) {
				showChangePasswordValidationError();
				return;
			}
			sessionService.changePassword(vm.oldPassword, vm.newPassword, vm.confirmNewPassword)
				.then(passwordSuccess, passwordFailure);
		}

		function showChangePasswordValidationError() {
			var notification = new TrackerNotification()
				.withType('error')
				.withMessage('');
			if (vm.changePasswordForm.oldPassword.$error.required) {
				notification.appendMessage('You must enter your current password.');
			}
			if (vm.changePasswordForm.newPassword.$error.required) {
				notification.appendMessage('You must enter your new password.');
			}
			if (vm.changePasswordForm.confirmNewPassword.$error.required) {
				notification.appendMessage('You must confirm your new password.');
			}
			if (vm.newPassword !== vm.confirmNewPassword) {
				notification.appendMessage('Your new passwords must match.');
			}
			notification.show();
		}

		function exportThreads() {
			vm.exportLoading = true;
			exportService.exportThreads(vm.includeArchived, vm.includeHiatused)
				.then(exportSuccess, exportFailure);
		}

		function exportSuccess() {
			vm.exportLoading = false;
		}

		function exportFailure() {
			new TrackerNotification()
				.withMessage('There was a problem exporting your threads.')
				.withType('error')
				.show();
			vm.exportLoading = false;
		}
	}
}());
