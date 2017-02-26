'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('ForgotPasswordController',
		[
			'$scope', '$controller', '$location', 'sessionService', 'notificationService',
			'NOTIFICATION_TYPES', 'BodyClass', forgotPasswordController
		]);

	/** @this forgotPasswordController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function forgotPasswordController($scope, $controller, $location, sessionService, notificationService, NOTIFICATION_TYPES, BodyClass) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		BodyClass.set('signin-page');
		vm.submitForgotPassword = submitForgotPassword;

		function submitForgotPassword() {
			vm.error = '';
			vm.success = '';
			vm.loading = true;
			sessionService.submitForgotPassword(vm.username).then(success, fail);
		}

		function success() {
			vm.loading = false;
			var type = NOTIFICATION_TYPES.FORGOT_PASSWORD_SUCCESS;
			notificationService.show(type);
		}

		function fail() {
			vm.loading = false;
			var type = NOTIFICATION_TYPES.FORGOT_PASSWORD_FAILURE;
			notificationService.show(type);
		}
	}
}());
