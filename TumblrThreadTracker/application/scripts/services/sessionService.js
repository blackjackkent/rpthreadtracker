'use strict';
(function() {
	angular.module('rpthreadtracker')
        .factory('sessionService',
		[
			'$q', '$http', '$window', 'SESSION_EVENTS',
			'$httpParamSerializerJQLike', sessionService
		]);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function sessionService($q, $http, $window, SESSION_EVENTS, $httpParamSerializerJQLike) {
		var user = null;
		var subscribers = [];

		function loadUser(viewModel) {
			isLoggedIn().then(function(isLoggedIn) {
				if (isLoggedIn) {
					getUser().then(function(user) {
						viewModel.userId = user.UserId;
						viewModel.user = user;
					});
				} else {
					viewModel.userId = null;
					viewModel.user = null;
				}
			});
		}

		function getUser(force) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/User',
					'method': 'GET'
				};
			function success(response) {
				if (response) {
					user = response.data;
					deferred.resolve(response.data);
				} else {
					deferred.resolve(null);
				}
			}
			if (user !== null && !force) {
				deferred.resolve(user);
				return deferred.promise;
			}
			$http(config).then(success);
			return deferred.promise;
		}

		function updateUser(user) {
			if (!user) {
				return;
			}
			var deferred = $q.defer(),
				config = {
					'url': '/api/User',
					'method': 'PUT',
					'data': user
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

		function isLoggedIn() {
			var deferred = $q.defer(),
				config = {
					'method': 'GET',
					'url': '/api/User'
				};
			function success(response) {
				deferred.resolve(response);
			}
			function error() {
				deferred.reject(false);
			}
			$http(config).then(success).catch(error);
			return deferred.promise;
		}

		function login(username, password) {
			var deferred = $q.defer(),
				config = {
					'method': 'POST',
					'url': '/token',
					'data': $httpParamSerializerJQLike({
						'UserName': username,
						'Password': password,
						'grant_type': 'password'
					}),
					'headers': {'Content-Type': 'application/x-www-form-urlencoded'}
				};
			function success(response) {
				deferred.resolve(response.data);
				$window.localStorage.TrackerBearerToken = response.data.access_token;
				broadcast(SESSION_EVENTS.LOGIN);
			}
			function error(data) {
				deferred.reject(data);
			}
			$http(config).then(success).catch(error);
			return deferred.promise;
		}

		function logout() {
			user = null;
			$window.localStorage.TrackerBearerToken = null;
			broadcast(SESSION_EVENTS.LOGOUT);
		}

		function submitForgotPassword(username) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/ForgotPassword',
					'method': 'POST',
					'data': '"' + username + '"'
				};
			function success(response) {
				deferred.resolve(response.data);
			}
			function error(data) {
				deferred.reject(data);
			}
			$http(config).then(success).catch(error);
			return deferred.promise;
		}

		function register(registerData) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/Account',
					'method': 'POST',
					'data': registerData
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

		function changePassword(oldPassword, newPassword, confirmNewPassword) {
			var deferred = $q.defer(),
				config = {
					'url': '/api/ChangePassword',
					'method': 'POST',
					'data': {
						'OldPassword': oldPassword,
						'NewPassword': newPassword,
						'ConfirmNewPassword': confirmNewPassword
					}
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

		function subscribe(callback) {
			subscribers.push(callback);
		}

		function unsubscribe(callback) {
			var index = subscribers.indexOf(callback);
			if (index > -1) {
				subscribers.splice(index, 1);
			}
		}

		function broadcast(data) {
			angular.forEach(subscribers,
                function(callback) {
	callback(data);
});
		}

		return {
			'isLoggedIn': isLoggedIn,
			'login': login,
			'logout': logout,
			'submitForgotPassword': submitForgotPassword,
			'changePassword': changePassword,
			'register': register,
			'getUser': getUser,
			'loadUser': loadUser,
			'updateUser': updateUser,
			'subscribe': subscribe,
			'unsubscribe': unsubscribe
		};
	}
}());
