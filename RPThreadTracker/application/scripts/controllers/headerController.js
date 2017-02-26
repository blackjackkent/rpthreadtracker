'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('HeaderController',
		[
			'$scope', 'threadService',
			headerController
		]);

	/** @this headerController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len
	function headerController($scope, threadService) {
		var vm = this;
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
