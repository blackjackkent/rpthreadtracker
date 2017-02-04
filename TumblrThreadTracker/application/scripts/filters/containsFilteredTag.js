'use strict';
(function() {
	angular.module('rpthreadtracker').filter('containsFilteredTag', containsFilteredTag);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function containsFilteredTag() {
		return function(threads, filteredTag) {
			if (!threads) {
				return [];
			}
			if (!filteredTag) {
				return threads;
			}
			return _.filter(threads, function(thread) {
				return thread.ThreadTags.indexOf(filteredTag) !== -1;
			});
		};
	}
}());
