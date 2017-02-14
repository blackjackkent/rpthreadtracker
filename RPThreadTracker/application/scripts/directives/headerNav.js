'use strict';
(function() {
	angular.module('rpthreadtracker')
		.directive('headerNav',
		[
			'cacheBuster', headerNav
		]);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function headerNav(cacheBuster) {
		var templateUrl = '/application/views/directives/'
			+ 'header-nav.html?cacheBuster=' + cacheBuster;
		return {
			'restrict': 'E',
			'replace': 'true',
			'scope': {
				'publicView': '@',
				'pageId': '@',
				'blogs': '=',
				'user': '='
			},
			'templateUrl': templateUrl,
			'controller': 'HeaderController',
			'controllerAs': 'vm',
			'bindToController': true
		};
	}
}());
