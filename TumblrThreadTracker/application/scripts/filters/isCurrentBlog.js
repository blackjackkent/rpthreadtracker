(function() {
    "use strict";
    angular.module("rpthreadtracker").filter("isCurrentBlog", isCorrectBlog);

    function isCorrectBlog() {
        return function(threads, blog) {
            if (!blog || !blog.UserBlogId) {
                return threads;
            }
            var out = [];
            if (!threads) {
                return out;
            }
            for (var i = 0; i < threads.length; i++) {
                if (threads[i].UserBlogId === blog.UserBlogId) {
                    out.push(threads[i]);
                }
            }
            return out;
        };
    };
})();