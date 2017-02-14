'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('LoginController',
		[
			'$controller', '$scope', '$location', 'sessionService', 'notificationService',
			'NOTIFICATION_TYPES', 'BodyClass', loginController
		]);

	/** @this loginController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len
	function loginController($controller, $scope, $location, sessionService, notificationService, NOTIFICATION_TYPES, BodyClass) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		vm.login = login;
		redirectIfLoggedIn();
		BodyClass.set('signin-page');

		function redirectIfLoggedIn() {
			sessionService.isLoggedIn().then(function(isLoggedIn) {
				if (isLoggedIn) {
					$location.path('/');
				}
			});
		}

		function login() {
			vm.loading = true;
			if (angular.isDefined(vm.loginForm)) {
				vm.username = vm.loginForm.username.value;
				vm.password = vm.loginForm.password.value;
			}
			sessionService.login(vm.username, vm.password).then(success, fail);
		}

		function success() {
			vm.loading = false;
			$location.path('/');
		}

		function fail() {
			vm.loading = false;
			var type = NOTIFICATION_TYPES.LOGIN_FAILURE;
			notificationService.show(type);
		}
	}
}());
