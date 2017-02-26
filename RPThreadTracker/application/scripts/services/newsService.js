'use strict';
(function() {
	angular.module('rpthreadtracker')
        .factory('newsService',
		[
			'$q', '$http', newsService
		]);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function newsService($q, $http) {
		var news = [];

		function getNews(force) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/News',
					'method': 'GET'
				};
			function success(response) {
				deferred.resolve(response.data);
			}
			if (news.length > 0 && !force) {
				deferred.resolve(news);
				return deferred.promise;
			}
			$http(config).then(success);
			return deferred.promise;
		}

		return {'getNews': getNews};
	}
}());
