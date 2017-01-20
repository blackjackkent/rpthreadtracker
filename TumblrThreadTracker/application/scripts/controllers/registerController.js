(function() {
	"use strict";
	angular.module("rpthreadtracker")
		.controller("RegisterController",
		[
			"$scope", "$location", "sessionService", "TrackerNotification", registerController
		]);

	function registerController($scope, $location, sessionService, TrackerNotification) {
		$scope.setBodyClass("signin-page");
		$scope.register = register;

		function register() {
			$scope.loading = true;
			if (!$scope.registerForm.$valid || $scope.confirmPassword != $scope.Password) {
				showValidationError();
				$scope.loading = false;
				return;
			}
			sessionService.register($scope.username, $scope.email, $scope.Password, $scope.confirmPassword)
				.then(success, fail);
		}

		function showValidationError() {
			var notification = new TrackerNotification()
				.withType("error")
				.withMessage("");
			if ($scope.registerForm.username.$error.required) {
				notification.appendMessage("You must enter a valid username.");
			}
			if ($scope.registerForm.email.$error.email || $scope.registerForm.email.$error.required) {
				notification.appendMessage("You must enter a valid email.");
			}
			if ($scope.registerForm.Password.$error.required) {
				notification.appendMessage("You must enter a password.");
			}
			if ($scope.registerForm.confirmPassword.$error.required) {
				notification.appendMessage("You must confirm your password.");
			}
			if ($scope.confirmPassword != $scope.Password) {
				notification.appendMessage("Your passwords must match.");
			}
			notification.show();
		}

		function success() {
			sessionService.login($scope.username, $scope.Password)
				.then(function() {
					$scope.loading = false;
					$location.path("/");
				});
		}

		function fail(response) {
			$scope.loading = false;
			if (response && response.data) {
				new TrackerNotification()
					.withMessage("ERROR: " + response.data)
					.withType("error")
					.show();
			} else {
				new TrackerNotification()
					.withMessage("Error registering account. Please try again later.")
					.withType("error")
					.show();
			}
		}
	}
})();