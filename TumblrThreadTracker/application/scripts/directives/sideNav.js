'use strict';
(function() {
	angular.module('rpthreadtracker')
		.directive('sideNav',
		[
			'cacheBuster', sideNav
		]);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function sideNav(cacheBuster) {
		return {
			'restrict': 'E',
			'replace': 'true',
			'scope': {
				'publicView': '@',
				'pageId': '@'
			},
			'templateUrl': '/application/views/directives/side-nav.html?cacheBuster=' + cacheBuster,
			'controller': 'SideNavController',
			'controllerAs': 'vm',
			'bindToController': true
		};
	}
}());
