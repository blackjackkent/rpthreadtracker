'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.services.service('sessionService', [
    '$q', '$http', function($q, $http) {
        var user = null;

        function getUser(force) {
            var deferred = $q.defer(),
                config = {
                    url: '/api/User',
                    method: 'GET'
                },
                success = function (response) {
                    if (response) {
                        user = response.data;
                        deferred.resolve(response.data);
                    } else {
                        deferred.resolve(null);
                    }
                };
            if (user != null && !force) {
                deferred.resolve(user);
                return deferred.promise;
            }
            $http(config).then(success);
            return deferred.promise;
        }

        function isLoggedIn() {
            var deferred = $q.defer(),
                config = {
                    method: 'GET',
                    url: '/api/User'
                },
                success = function(response, status, headers, config) {
                    deferred.resolve(response);
                },
                error = function(data) {
                    deferred.reject(false);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        }

        function login(username, password) {
            var deferred = $q.defer(),
                config = {
                    method: 'POST',
                    url: '/api/Session',
                    data: { UserName: username, Password: password }
                },
                success = function(response, status, headers, config) {
                    deferred.resolve(response.data);
                },
                error = function(data) {
                    deferred.reject(data);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        }

        function logout() {
            var deferred = $q.defer(),
                config = {
                    method: 'DELETE',
                    url: '/api/Session'
                },
                success = function(response, status, headers, config) {
                    deferred.resolve(response.data);
                    user = null;
                },
                error = function(data) {
                    deferred.reject(data);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        }

        function submitForgotPassword(username) {
            var deferred = $q.defer(),
                config = {
                    method: 'POST',
                    url: '/api/ForgotPassword?username=' + username
                },
                success = function(response, status, headers, config) {
                    deferred.resolve(response.data);
                },
                error = function(data) {
                    deferred.reject(data);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        }

        function register(username, email, password, confirmPassword) {
            var deferred = $q.defer(),
                config = {
                    url: '/api/Account',
                    method: "POST",
                    data: {
                        Username: username,
                        Email: email,
                        Password: password,
                        ConfirmPassword: confirmPassword
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

        function changePassword(oldPassword, newPassword, confirmNewPassword) {
            var deferred = $q.defer(),
                config = {
                    url: '/api/ChangePassword',
                    method: "POST",
                    data: {
                        OldPassword: oldPassword,
                        NewPassword: newPassword,
                        ConfirmNewPassword: confirmNewPassword
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

        return {
            isLoggedIn: isLoggedIn,
            login: login,
            logout: logout,
            submitForgotPassword: submitForgotPassword,
            changePassword: changePassword,
            register: register,
            getUser: getUser
        };
    }
]);