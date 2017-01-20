(function() {
    "use strict";
    angular.module("rpthreadtracker").filter("isCorrectTurn", isCorrectTurn);

    function isCorrectTurn() {
        return function(threads, pageId, filterNulls) {
            var isMyTurnValue = null;
            if (pageId == "yourturn")
                isMyTurnValue = true;
            if (pageId == "theirturn")
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
    };
})();