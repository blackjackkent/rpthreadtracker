'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.filters.filter('isCurrentBlog', function() {
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
});