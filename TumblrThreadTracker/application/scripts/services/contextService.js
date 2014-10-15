'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.services.service('contextService', [
    '$q', '$http', function($q, $http) {
        var sortDescending = false,
            currentBlog = '',
            currentOrderBy = 'LastPostDate';

        function getSortDescending() {
            return sortDescending;
        }

        function getCurrentBlog() {
            return currentBlog;
        }

        function getCurrentOrderBy() {
            return currentOrderBy;
        }

        function setSortDescending(sort) {
            sortDescending = sort;
        }

        function setCurrentBlog(blogShortname) {
            currentBlog = blogShortname;
        }

        function setCurrentOrderBy(orderBy) {
            currentOrderBy = orderBy;
        }

        function getPublicUrl(pageId, userId) {
            return "http://www.rpthreadtracker.com/public/" + pageId + "?userId=" + userId + "&currentBlog=" + currentBlog + "&sortDescending=" + sortDescending + "&currentOrderBy=" + currentOrderBy;
        }

        return {
            getSortDescending: getSortDescending,
            getCurrentBlog: getCurrentBlog,
            getCurrentOrderBy: getCurrentOrderBy,
            setSortDescending: setSortDescending,
            setCurrentBlog: setCurrentBlog,
            setCurrentOrderBy: setCurrentOrderBy,
            getPublicUrl: getPublicUrl
        };
    }
]);