'use strict';
(function() {
	angular.module('rpthreadtracker').filter('escape', ['$window', escape]);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function escape($window) {
		return $window.encodeURIComponent;
	}
}());
