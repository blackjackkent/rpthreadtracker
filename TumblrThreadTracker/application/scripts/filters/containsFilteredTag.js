(function() {
    "use strict";
    angular.module("rpthreadtracker").filter("containsFilteredTag", containsFilteredTag);

    function containsFilteredTag() {
        return function(threads, filteredTag) {
            if (filteredTag === null || filteredTag === "") {
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
    }
})();