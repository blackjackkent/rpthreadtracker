'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('HeaderController',
		[
			'$scope', 'threadService', 'adminflareService', '$timeout',
			headerController
		]);

	/** @this headerController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function headerController($scope, threadService, adminflareService, $timeout) {
		var vm = this;
		$timeout(adminflareService.init);
		$timeout(adminflareService.initCustom);
		threadService.subscribeLoadedThreadEvent(showLoadingIcon);
		threadService.subscribeLoadedArchiveThreadEvent(showLoadingIcon);
		threadService.subscribeAllThreadsLoaded(hideLoadingIcon);
		vm.loading = false;
		$scope.$on('$destroy', destroyView);

		function showLoadingIcon() {
			vm.loading = true;
		}

		function hideLoadingIcon() {
			vm.loading = false;
		}

		function destroyView() {
			threadService.unsubscribeLoadedThreadEvent(showLoadingIcon);
			threadService.unsubscribeLoadedArchiveThreadEvent(showLoadingIcon);
			threadService.unsubscribeAllThreadsLoaded(hideLoadingIcon);
		}
	}
}());
