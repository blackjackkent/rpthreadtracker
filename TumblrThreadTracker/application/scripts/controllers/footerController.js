'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('FooterController',
		[
			'$scope', 'sessionService', footerController
		]);

	/** @this footerController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len
	function footerController($scope, sessionService) {
		var vm = this;
		vm.toggleTheme = function() {
			if (!vm.user) {
				return;
			}
			vm.user.UseInvertedTheme = !vm.user.UseInvertedTheme;
			sessionService.updateUser(vm.user);
		};
		vm.year = new Date().getFullYear();
	}
}());
