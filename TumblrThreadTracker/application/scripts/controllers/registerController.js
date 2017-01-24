'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('RegisterController',
		[
			'$scope', '$controller', '$location', 'sessionService', 'TrackerNotification',
			'BodyClass', registerController
		]);

	/** @this registerController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len
	function registerController($scope, $controller, $location, sessionService, TrackerNotification, BodyClass) {
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
			sessionService.register(vm.username, vm.email, vm.Password, vm.confirmPassword)
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
			if (response && response.data) {
				new TrackerNotification()
					.withMessage('ERROR: ' + response.data)
					.withType('error')
					.show();
			} else {
				new TrackerNotification()
					.withMessage('Error registering account. Please try again later.')
					.withType('error')
					.show();
			}
		}

		// eslint-disable-next-line max-statements
		function showValidationError() {
			var notification = new TrackerNotification()
				.withType('error')
				.withMessage('');
			if (vm.registerForm.username.$error.required) {
				notification.appendMessage('You must enter a valid username.');
			}
			if (vm.registerForm.email.$error.email || vm.registerForm.email.$error.required) {
				notification.appendMessage('You must enter a valid email.');
			}
			if (vm.registerForm.Password.$error.required) {
				notification.appendMessage('You must enter a password.');
			}
			if (vm.registerForm.confirmPassword.$error.required) {
				notification.appendMessage('You must confirm your password.');
			}
			if (vm.confirmPassword !== vm.Password) {
				notification.appendMessage('Your passwords must match.');
			}
			notification.show();
		}
	}
}());
