﻿'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('HeaderController',
		[
			'threadService',
			headerController
		]);

	/** @this headerController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len
	function headerController(threadService) {
		var vm = this;
		threadService.subscribeLoadedThreadEvent(showLoadingIcon);
		threadService.subscribeLoadedArchiveThreadEvent(showLoadingIcon);
		threadService.subscribeAllThreadsLoaded(hideLoadingIcon);
		vm.loading = false;

		function showLoadingIcon() {
			vm.loading = true;
		}

		function hideLoadingIcon() {
			vm.loading = false;
		}
	}
}());