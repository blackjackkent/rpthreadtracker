(function() {
	angular.module("rpthreadtracker")
		.controller("FooterController",
		[
			"$scope", "sessionService",
			footerController
		]);

	function footerController($scope, sessionService) {
		$scope.toggleTheme = function() {
			if (!$scope.user) {
				return;
			}
			$scope.user.UseInvertedTheme = !$scope.user.UseInvertedTheme;
			sessionService.updateUser($scope.user);
		};
		$scope.year = new Date().getFullYear();
	}
})();