'use strict';

/* Filters */

angular.module('rpThreadTracker.filters', [])
    .filter('isCurrentBlog', function() {
        return function(threads, blogShortname) {
            if (blogShortname === null || blogShortname === '') {
                return threads;
            }
            var out = [];
            for (var i = 0; i < threads.length; i++) {
                if (threads[i].BlogShortname == blogShortname) {
                    out.push(threads[i]);
                }
            }
            return out;
        };
    })
    .filter('isCorrectTurn', function() {
        return function (threads, pageId) {
            var isMyTurnValue = null;
            if (pageId == 'yourturn')
                isMyTurnValue = true;
            if (pageId == 'theirturn')
                isMyTurnValue = false;
            if (isMyTurnValue == null)
                return threads;

            var out = [];
            for (var i = 0; i < threads.length; i++) {
                if (threads[i].IsMyTurn == isMyTurnValue) {
                    out.push(threads[i]);
                }
            }
            return out;
        };
    });