'use strict';
(function() {
    angular.module('rpthreadtracker').filter('isCorrectTurn', ['THREAD_PAGE_IDS', isCorrectTurn]);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
    function isCorrectTurn(THREAD_PAGE_IDS) {
		return function(threads, pageId, filterNulls) {
			if (!threads) {
				return [];
            }
			switch (pageId) {
			    case THREAD_PAGE_IDS.YOUR_TURN:
				    return _.filter(threads, function (thread) {
					    var isNullFilteredThread = filterNulls && thread.LastPostDate === null;
					    return thread.IsMyTurn && !thread.MarkedQueued && !isNullFilteredThread;
                        });
                case THREAD_PAGE_IDS.THEIR_TURN:
	                return _.filter(threads, function (thread) {
		                var isNullFilteredThread = filterNulls && thread.LastPostDate === null;
		                return !thread.IsMyTurn && !thread.MarkedQueued && !isNullFilteredThread;
	                });
			    case THREAD_PAGE_IDS.QUEUED:
				    return _.filter(threads, function (thread) {
					    var isNullFilteredThread = filterNulls && thread.LastPostDate === null;
					    return thread.MarkedQueued && !isNullFilteredThread;
				    });
                default:
	                return threads;
			}
		};
	}
}());
