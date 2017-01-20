(function() {
    "use strict";
    angular.module("rpthreadtracker")
        .service("blogService",
        [
            "$q", "$http", blogService
        ]);

    function blogService($q, $http) {
        var blogs = [];

        function getBlogs(force, includeHiatusedBlogs) {
            var deferred = $q.defer(),
                config = {
                    url: "/api/Blog" + (includeHiatusedBlogs ? "?includeHiatusedBlogs=true" : ""),
                    method: "GET"
                },
                success = function(response) {
                    if (!response) {
                        deferred.resolve(null);
                    } else {
                        deferred.resolve(response.data);
                    }
                };
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
                    url: "/api/Blog/" + id,
                    method: "GET"
                },
                success = function(response) {
                    deferred.resolve(response.data);
                },
                error = function(response) {
                    deferred.reject(response.data);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        };

        function flushBlogs() {
            blogs = [];
        }

        function createBlog(blogShortname) {
            var deferred = $q.defer(),
                config = {
                    url: "/api/Blog",
                    method: "POST",
                    data: {
                        BlogShortname: blogShortname
                    }
                },
                success = function(response, status, headers, config) {
                    deferred.resolve(response.data);
                },
                error = function(response, status, headers, config) {
                    deferred.reject(response);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        }

        function editBlog(blog) {
            var deferred = $q.defer(),
                config = {
                    url: "/api/Blog",
                    method: "PUT",
                    data: blog
                },
                success = function(response, status, headers, config) {
                    deferred.resolve(response.data);
                },
                error = function(response, status, headers, config) {
                    deferred.reject(response);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        }

        function untrackBlog(userBlogId) {
            var deferred = $q.defer(),
                config = {
                    url: "/api/Blog?userBlogId=" + userBlogId,
                    method: "DELETE"
                },
                success = function(response, status, headers, config) {
                    deferred.resolve(response.data);
                },
                error = function(response, status, headers, config) {
                    deferred.reject(response);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        }

        return {
            getBlogs: getBlogs,
            getBlog: getBlog,
            flushBlogs: flushBlogs,
            createBlog: createBlog,
            editBlog: editBlog,
            untrackBlog: untrackBlog
        };
    }
})();