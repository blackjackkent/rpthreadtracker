(function() {
	angular.module("rpthreadtracker")
		.controller("HeaderController",
		[
			"$timeout", "threadService", "adminflareService",
			headerController
		]);

	function headerController($timeout, threadService, adminflareService) {
		var vm = this;
		$timeout(adminflareService.init);
		$timeout(adminflareService.initCustom);
		threadService.subscribe(showLoadingIcon);
		threadService.subscribeOnComplete(hideLoadingIcon);
		vm.loading = false;

		function showLoadingIcon() {
			vm.loading = true;
		}

		function hideLoadingIcon() {
			vm.loading = false;
		}
	}
})();