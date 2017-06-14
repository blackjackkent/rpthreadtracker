'use strict';
(function() {
	angular.module('rpthreadtracker')
        .controller('ManageAccountController', [
	'$scope', '$controller', '$location', 'sessionService', 'exportService', 'pageId',
	'notificationService', 'NOTIFICATION_TYPES', 'BodyClass', '$mdDialog', manageAccountController
]);

    /** @this manageAccountController */
    // eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function manageAccountController($scope, $controller, $location, sessionService, exportService, pageId, notificationService, NOTIFICATION_TYPES, BodyClass, $mdDialog) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		sessionService.loadUser(vm);
		BodyClass.set('');
		vm.pageId = pageId;
		vm.changePassword = changePassword;
		vm.exportThreads = exportThreads;
		vm.enableAllowMarkQueued = enableAllowMarkQueued;
		vm.disableAllowMarkQueued = disableAllowMarkQueued;
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

		function enableAllowMarkQueued() {
			var message = '<p>Enabling this tool will add a "Mark Queued" option to each thread'
				+ ' in the tracker. When a thread in the tracker is marked as "queued", <br />'
				+ 'that thread will be moved to a separate "Queued Threads" tracker page, '
				+ 'accessible from the left-hand menu. It will stay there until the tracker <br />'
				+ 'detects a new post on the thread, at which point the thread will automatically'
				+ ' be moved back to your main thread display.</p>'
				+ '<p>Most of the time, this process works exactly as expected. However, '
				+ '<strong>there are possible scenarios where a thread could get stuck <br />'
				+ 'on the Queued Threads page, so it is advised that you check this page '
				+ 'regularly for accuracy</strong>.</p>'
				+ '<p>Click OK to acknowledge this information and activate the '
				+ '"Mark Queued" option.</p>';
			var confirm = $mdDialog.confirm()
				.title('Important Info Regarding This Tool')
				.htmlContent(message)
				.ok('OK')
				.cancel('Cancel');
			$mdDialog.show(confirm).then(function() {
				vm.user.AllowMarkQueued = true;
				sessionService.updateUser(vm.user);
			});
		}

		function disableAllowMarkQueued() {
			vm.user.AllowMarkQueued = false;
			sessionService.updateUser(vm.user);
		}
	}
}());
