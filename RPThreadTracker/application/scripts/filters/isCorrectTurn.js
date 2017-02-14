'use strict';
(function() {
	angular.module('rpthreadtracker').filter('isCorrectTurn', isCorrectTurn);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function isCorrectTurn() {
		return function(threads, pageId, filterNulls) {
			if (!threads) {
				return [];
			}
			if (pageId !== 'yourturn' && pageId !== 'theirturn') {
				return threads;
			}
			var isMyTurnValue = pageId === 'yourturn';

			return _.filter(threads, function(thread) {
				var isNullFilteredThread = filterNulls && thread.LastPostDate === null;
				return thread.IsMyTurn === isMyTurnValue && !isNullFilteredThread;
			});
		};
	}
}());
