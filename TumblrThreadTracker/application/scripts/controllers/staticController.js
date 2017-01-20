(function() {
	"use strict";
	angular.module("rpthreadtracker")
		.controller("StaticController",
		[
			"sessionService", "pageId", '$controller', '$scope', 'BodyClass', staticController
		]);

	function staticController(sessionService, pageId, $controller, $scope, BodyClass) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', { $scope: $scope }));
		sessionService.loadUser(vm);
		BodyClass.set('');
		vm.pageId = pageId;
		vm.publicView = true;
	}
})();