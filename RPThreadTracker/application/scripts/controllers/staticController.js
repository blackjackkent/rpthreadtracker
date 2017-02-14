'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('StaticController',
		[
			'sessionService', 'pageId', '$controller', '$scope', 'BodyClass', staticController
		]);

	/** @this staticController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len
	function staticController(sessionService, pageId, $controller, $scope, BodyClass) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		init();

		function init() {
			sessionService.loadUser(vm);
			BodyClass.set('');
			vm.pageId = pageId;
			vm.publicView = true;
		}
	}
}());
