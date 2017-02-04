'use strict';
(function() {
	angular.module('rpthreadtracker')
        .factory('blogService',
		[
			'$q', '$http', blogService
		]);

	function blogService($q, $http) {
		var blogs = [];

		function getBlogs(force, includeHiatusedBlogs) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/Blog' + (includeHiatusedBlogs ? '?includeHiatusedBlogs=true' : ''),
					'method': 'GET'
				};
			function success(response) {
				if (response) {
					deferred.resolve(response.data);
				} else {
					deferred.resolve(null);
				}
			}
			if (blogs.length > 0 && !force) {
				deferred.resolve(blogs);
				return deferred.promise;
			}
			$http(config).then(success);
			return deferred.promise;
		}

		function getBlog(id) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/Blog/' + id,
					'method': 'GET'
				};
			function success(response) {
				deferred.resolve(response.data);
			}
			function error(response) {
				deferred.reject(response.data);
			}
			$http(config).then(success).catch(error);
			return deferred.promise;
		}

		function flushBlogs() {
			blogs = [];
		}

		function createBlog(blogShortname) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/Blog',
					'method': 'POST',
					'data': {'BlogShortname': blogShortname}
				};
			function success(response) {
				deferred.resolve(response.data);
			}
			function error(response) {
				deferred.reject(response);
			}
			$http(config).then(success).catch(error);
			return deferred.promise;
		}

		function editBlog(blog) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/Blog',
					'method': 'PUT',
					'data': blog
				};
			function success(response) {
				deferred.resolve(response.data);
			}
			function error(response) {
				deferred.reject(response);
			}
			$http(config).then(success).catch(error);
			return deferred.promise;
		}

		function untrackBlog(userBlogId) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/Blog?userBlogId=' + userBlogId,
					'method': 'DELETE'
				};
			function success(response) {
				deferred.resolve(response.data);
			}
			function error(response) {
				deferred.reject(response);
			}
			$http(config).then(success).catch(error);
			return deferred.promise;
		}

		return {
			'getBlogs': getBlogs,
			'getBlog': getBlog,
			'flushBlogs': flushBlogs,
			'createBlog': createBlog,
			'editBlog': editBlog,
			'untrackBlog': untrackBlog
		};
	}
}());
