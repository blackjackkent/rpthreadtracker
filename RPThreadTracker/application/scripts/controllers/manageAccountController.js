'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('ManageAccountController',
		[
			'$scope', '$controller', '$location', 'sessionService', 'exportService', 'pageId',
			'notificationService', 'NOTIFICATION_TYPES', 'BodyClass', manageAccountController
		]);

	/** @this manageAccountController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function manageAccountController($scope, $controller, $location, sessionService, exportService, pageId, notificationService, NOTIFICATION_TYPES, BodyClass) {
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
			var type = NOTIFICATION_TYPES.UPDATE_ACCOUNT_SUCCESS;
			notificationService.show(type);
		}

		function passwordFailure() {
			var type = NOTIFICATION_TYPES.UPDATE_ACCOUNT_FAILURE;
			notificationService.show(type);
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
			var type = NOTIFICATION_TYPES.CHANGE_PASSWORD_VALIDATION_ERROR;
			var extraData = {
				'oldPasswordRequired': vm.changePasswordForm.oldPassword.$error.required,
				'newPasswordRequired': vm.changePasswordForm.newPassword.$error.required,
				'confirmRequired': vm.changePasswordForm.confirmNewPassword.$error.required,
				'confirmMatch': vm.newPassword !== vm.confirmNewPassword
			};
			notificationService.show(type, extraData);
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
			var type = NOTIFICATION_TYPES.EXPORT_FAILURE;
			notificationService.show(type);
			vm.exportLoading = false;
		}
	}
}());
