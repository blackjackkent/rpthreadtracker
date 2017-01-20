(function() {
    'use strict';
    angular.module('rpthreadtracker')
        .service('newsService',
        [
            '$q', '$http', newsService
        ]);
    function newsService($q, $http) {
            var news = [];

            function getNews(force) {
                var deferred = $q.defer(),
                    config = {
                        url: '/api/News',
                        method: 'GET'
                    },
                    success = function(response) {
                        deferred.resolve(response.data);
                    };
                if (news.length > 0 && !force) {
                    deferred.resolve(news);
                    return deferred.promise;
                }
                $http(config).then(success);
                return deferred.promise;
            }

            return {
                getNews: getNews
            };
        }
})();