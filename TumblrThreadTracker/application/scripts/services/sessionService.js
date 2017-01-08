"use strict";
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.services.constant("SESSION_EVENTS",
    { 'LOGIN': 1, 'LOGOUT': 2 })
    .service("sessionService",
    [
        "$q", "$http", "$window", 'SESSION_EVENTS', function($q, $http, $window, SESSION_EVENTS) {
            var user = null;
            var subscribers = [];

            function getUser(force) {
                var deferred = $q.defer(),
                    config = {
                        url: "/api/User",
                        method: "GET"
                    },
                    success = function(response) {
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

            function updateUser(user) {
                if (!user) {
                    return;
                }
                var deferred = $q.defer(),
                    config = {
                        url: "/api/User",
                        method: "PUT",
                        data: user
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

            function isLoggedIn() {
                var deferred = $q.defer(),
                    config = {
                        method: "GET",
                        url: "/api/User"
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
                        method: "POST",
                        url: "/token",
                        data: $.param({ UserName: username, Password: password, grant_type: "password" }),
                        headers: {
                            'Content-Type': "application/x-www-form-urlencoded"
                        }
                    },
                    success = function(response, status, headers, config) {
                        deferred.resolve(response.data);
                        $window.localStorage["TrackerBearerToken"] = response.data.access_token;
                        broadcast(SESSION_EVENTS.LOGIN);
                    },
                    error = function(data) {
                        deferred.reject(data);
                    };
                $http(config).then(success).catch(error);
                return deferred.promise;
            }

            function logout() {
                user = null;
                $window.localStorage["TrackerBearerToken"] = null;
                broadcast(SESSION_EVENTS.LOGOUT);
            }

            function submitForgotPassword(username) {
                var deferred = $q.defer(),
                    config = {
                        method: "POST",
                        url: "/api/ForgotPassword",
                        data: {
                            UsernameOrEmail: username
                        }
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
                        url: "/api/Account",
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
                        url: "/api/ChangePassword",
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
                    function(callback, key) {
                        callback(data);
                    });
            }

            return {
                isLoggedIn: isLoggedIn,
                login: login,
                logout: logout,
                submitForgotPassword: submitForgotPassword,
                changePassword: changePassword,
                register: register,
                getUser: getUser,
                updateUser: updateUser,
                subscribe: subscribe,
                unsubscribe: unsubscribe
            };
        }
    ]);