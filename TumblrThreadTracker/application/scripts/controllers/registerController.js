'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('RegisterController',
		[
			'$scope', '$controller', '$location', 'sessionService',
			'notificationService', 'NOTIFICATION_TYPES', 'BodyClass', registerController
		]);

	/** @this registerController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len
	function registerController($scope, $controller, $location, sessionService, notificationService, NOTIFICATION_TYPES, BodyClass) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		vm.register = register;
		BodyClass.set('signin-page');

		function register() {
			vm.loading = true;
			if (!vm.registerForm.$valid || vm.confirmPassword !== vm.Password) {
				showValidationError();
				vm.loading = false;
				return;
			}
			var registerData = {
				'Username': vm.username,
				'Email': vm.email,
				'Password': vm.password,
				'ConfirmPassword': vm.confirmPassword
			};
			sessionService.register(registerData)
				.then(success, fail);
		}

		function success() {
			sessionService.login(vm.username, vm.Password)
				.then(function() {
					vm.loading = false;
					$location.path('/');
				});
		}

		function fail(response) {
			vm.loading = false;
			var specificErrorMessage = '';
			if (response.status === 400) {
				specificErrorMessage = response.data;
			}
			var type = NOTIFICATION_TYPES.REGISTER_FAILURE;
			var extraData = {'specificErrorMessage': specificErrorMessage};
			notificationService.show(type, extraData);
		}

		// eslint-disable-next-line max-statements
		function showValidationError() {
			var extraData = {
				'usernameRequired': vm.registerForm.username.$error.required,
				'emailRequired': vm.registerForm.email.$error.email
					|| vm.registerForm.email.$error.required,
				'passwordRequired': vm.registerForm.Password.$error.required,
				'confirmPasswordRequired': vm.registerForm.confirmPassword.$error.required,
				'passwordMatch': vm.confirmPassword !== vm.Password
			};
			var type = NOTIFICATION_TYPES.REGISTER_VALIDATION_ERROR;
			notificationService.show(type, extraData);
		}
	}
}());
