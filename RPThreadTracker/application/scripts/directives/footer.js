'use strict';
(function() {
	angular.module('rpthreadtracker')
		.directive('footer',
		[
			'cacheBuster', footer
		]);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function footer(cacheBuster) {
		return {
			'restrict': 'E',
			'replace': 'true',
			'scope': {'user': '='},
			'templateUrl': '/application/views/directives/footer.html?cacheBuster=' + cacheBuster,
			'controller': 'FooterController',
			'controllerAs': 'vm',
			'bindToController': true
		};
	}
}());
