'use strict';

/* Filters */

angular.module('rpThreadTracker.filters', [])
    .filter('isCurrentBlog', function() {
        return function(threads, blogShortname) {
            if (blogShortname === null || blogShortname === '') {
                return threads;
            }
            var out = [];
            if (!threads) {
                return out;
            }
            for (var i = 0; i < threads.length; i++) {
                if (threads[i].BlogShortname == blogShortname) {
                    out.push(threads[i]);
                }
            }
            return out;
        };
    })
    .filter('isCorrectTurn', function() {
        return function (threads, pageId, filterNulls) {
            var isMyTurnValue = null;
            if (pageId == 'yourturn')
                isMyTurnValue = true;
            if (pageId == 'theirturn')
                isMyTurnValue = false;
            if (isMyTurnValue == null)
                return threads;

            var out = [];

            if (!threads) {
                return out;
            }
            for (var i = 0; i < threads.length; i++) {
                var isNullFilteredThread = filterNulls && (threads[i].LastPostDate == null);
                if (threads[i].IsMyTurn == isMyTurnValue && !isNullFilteredThread) {
                    out.push(threads[i]);
                }
            }
            return out;
        };
    });