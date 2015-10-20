'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.filters.filter('containsFilteredTag', function () {
    return function (threads, filteredTag) {
        if (filteredTag === null || filteredTag === '') {
            return threads;
        }
        var out = [];
        if (!threads) {
            return out;
        }
        for (var i = 0; i < threads.length; i++) {
            if (threads[i].ThreadTags.indexOf(filteredTag) != -1) {
                out.push(threads[i]);
            }
        }
        return out;
    };
});