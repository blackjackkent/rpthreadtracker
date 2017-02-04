'use strict';
(function() {
	angular.module('rpthreadtracker')
        .factory('contextService',
		[
			contextService
		]);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function contextService() {
		var sortDescending = false,
			currentBlog = null,
			filteredTag = '',
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

		function getFilteredTag() {
			return filteredTag;
		}

		function setSortDescending(sort) {
			sortDescending = sort;
		}

		function setCurrentBlog(blog) {
			currentBlog = blog;
		}

		function setCurrentOrderBy(orderBy) {
			currentOrderBy = orderBy;
		}

		function setFilteredTag(newFilteredTag) {
			filteredTag = newFilteredTag;
		}

		function getPublicUrl(pageId, userId) {
			return 'http://www.rpthreadtracker.com/public/' +
                pageId +
                '?userId=' +
                userId +
                '&currentBlog=' +
                currentBlog +
                '&sortDescending=' +
                sortDescending +
                '&currentOrderBy=' +
                currentOrderBy;
		}

		return {
			'getSortDescending': getSortDescending,
			'getCurrentBlog': getCurrentBlog,
			'getCurrentOrderBy': getCurrentOrderBy,
			'getFilteredTag': getFilteredTag,
			'setSortDescending': setSortDescending,
			'setCurrentBlog': setCurrentBlog,
			'setCurrentOrderBy': setCurrentOrderBy,
			'setFilteredTag': setFilteredTag,
			'getPublicUrl': getPublicUrl
		};
	}
}());
