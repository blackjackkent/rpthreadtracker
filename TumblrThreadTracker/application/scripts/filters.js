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
    });