(function() {
	"use strict";
	angular.module("rpthreadtracker")
		.controller("LoginController",
		[
			"$controller", "$scope", "$location", "sessionService", "TrackerNotification", 'BodyClass',
			loginController
		]);

	function loginController($controller, $scope, $location, sessionService, TrackerNotification, BodyClass) {
		angular.extend(this, $controller('BaseController as base', { $scope: $scope }));
		var vm = this;
		vm.login = login;
		redirectIfLoggedIn();
		BodyClass.set('signin-page');

		function redirectIfLoggedIn() {
			sessionService.isLoggedIn()
				.then(function(isLoggedIn) {
					if (isLoggedIn) {
						$location.path("/");
					}
				});
		}

		function login() {
			vm.loading = true;
			if (vm.loginForm != undefined) {
				vm.username = vm.loginForm.username.value;
				vm.password = vm.loginForm.password.value;
			}
			sessionService.login(vm.username, vm.password).then(success, fail);
		}

		function success() {
			vm.loading = false;
			$location.path("/");
		}

		function fail() {
			vm.loading = false;
			new TrackerNotification()
				.withMessage("Incorrect username or password.")
				.withType("error")
				.show();
		}
	}
})();