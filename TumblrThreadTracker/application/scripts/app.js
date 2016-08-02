'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.app = angular.module('rpThreadTracker', ['ngRoute', 'rpThreadTracker.filters', 'rpThreadTracker.services', 'rpThreadTracker.directives', 'rpThreadTracker.controllers', 'angulartics', 'angulartics.google.analytics']);
rpThreadTracker.controllers = angular.module('rpThreadTracker.controllers', ['rpThreadTracker.services']);
rpThreadTracker.directives = angular.module('rpThreadTracker.directives', []);
rpThreadTracker.filters = angular.module('rpThreadTracker.filters', []);
rpThreadTracker.services = angular.module('rpThreadTracker.services', []);

// Declare app level module which depends on filters, and services
var cacheBuster = Date.now();
rpThreadTracker.app.constant("cacheBuster", cacheBuster);
rpThreadTracker.app.config([
        '$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
            
            $routeProvider.when('/maintenance', {
                templateUrl: '/application/views/maintenance.html?cacheBuster=' + cacheBuster,
                controller: 'StaticController',
                resolve: {
                    pageId: function () {
                        return "maintenance";
                    }
                }
            });
            $routeProvider.when('/', { templateUrl: '/application/views/dashboard.html', controller: 'MainController' });
            $routeProvider.when('/', {
                templateUrl: '/application/views/dashboard.html?cacheBuster=' + cacheBuster,
                controller: 'MainController',
                resolve: {
                    pageId: function() {
                        return "dashboard";
                    }
                }
            });
            $routeProvider.when('/threads', {
                templateUrl: '/application/views/threads.html?cacheBuster=' + cacheBuster,
                controller: 'MainController',
                resolve: {
                    pageId: function() {
                        return "allthreads";
                    }
                }
            });
            $routeProvider.when('/threads/allthreads', {
                templateUrl: '/application/views/threads.html?cacheBuster=' + cacheBuster,
                controller: 'MainController',
                resolve: {
                    pageId: function () {
                        return "allthreads";
                    }
                }
            });
            $routeProvider.when('/threads/yourturn', {
                templateUrl: '/application/views/threads.html?cacheBuster=' + cacheBuster,
                controller: 'MainController',
                resolve: {
                    pageId: function() {
                        return "yourturn";
                    }
                }
            });
            $routeProvider.when('/threads/theirturn', {
                templateUrl: '/application/views/threads.html?cacheBuster=' + cacheBuster,
                controller: 'MainController',
                resolve: {
                    pageId: function() {
                        return "theirturn";
                    }
                }
            });
            $routeProvider.when('/threads/archived', {
                templateUrl: '/application/views/threads.html',
                controller: 'MainController',
                resolve: {
                    pageId: function () {
                        return "archived";
                    }
                }
            });
            $routeProvider.when('/public/:pageId', {
                templateUrl: '/application/views/public.html?cacheBuster=' + cacheBuster,
                controller: 'PublicController'
            });
            $routeProvider.when('/login', {
                templateUrl: '/application/views/login.html?v=10614',
                controller: 'LoginController',
                resolve: {
                    pageId: function () {
                        return "login";
                    }
                }
            });
            $routeProvider.when('/register', {
                templateUrl: '/application/views/register.html?cacheBuster=' + cacheBuster,
                controller: 'RegisterController',
                resolve: {
                    pageId: function () {
                        return "register";
                    }
                }
            });

            $routeProvider.when('/add-thread', {
                templateUrl: '/application/views/add-thread.html?cacheBuster=' + cacheBuster,
                controller: 'ManageThreadController',
                resolve: {
                    pageId: function () {
                        return "add-thread";
                    }
                }
            });

            $routeProvider.when('/edit-thread/:userThreadId', {
                templateUrl: '/application/views/add-thread.html?cacheBuster=' + cacheBuster,
                controller: 'ManageThreadController',
                resolve: {
                    pageId: function () {
                        return "edit-thread";
                    }
                }
            });

            $routeProvider.when('/manage-blogs', {
                templateUrl: '/application/views/manage-blogs.html?cacheBuster=' + cacheBuster,
                controller: 'ManageBlogsController',
                resolve: {
                    pageId: function () {
                        return "manage-blogs";
                    }
                }
            });

            $routeProvider.when('/edit-blog/:userBlogId', {
                templateUrl: '/application/views/edit-blog.html?cacheBuster=' + cacheBuster,
                controller: 'EditBlogController',
                resolve: {
                    pageId: function () {
                        return "edit-blog";
                    }
                }
            });

            $routeProvider.when('/manage-account', {
                templateUrl: '/application/views/manage-account.html?cacheBuster=' + cacheBuster,
                controller: 'ManageAccountController',
                resolve: {
                    pageId: function () {
                        return "manage-account";
                    }
                }
            });

            $routeProvider.when('/about', {
                templateUrl: '/application/views/about.html?cacheBuster=' + cacheBuster,
                controller: 'StaticController',
                resolve: {
                    pageId: function () {
                        return "about";
                    }
                }
            });

            $routeProvider.when('/contact', {
                templateUrl: '/application/views/contact.html?cacheBuster=' + cacheBuster,
                controller: 'StaticController',
                resolve: {
                    pageId: function () {
                        return "contact";
                    }
                }
            });

            $routeProvider.when('/help', {
                templateUrl: '/application/views/help.html?cacheBuster=' + cacheBuster,
                controller: 'StaticController',
                resolve: {
                    pageId: function () {
                        return "help";
                    }
                }
            });

            $routeProvider.when('/logout', {
                templateUrl: '/application/views/login.html?cacheBuster=' + cacheBuster,
                controller: 'LogoutController',
                resolve: {
                    pageId: function () {
                        return "logout";
                    }
                }
            });

            $routeProvider.when('/forgot-password', {
                templateUrl: '/application/views/forgot-password.html?cacheBuster=' + cacheBuster,
                controller: 'ForgotPasswordController',
                resolve: {
                    pageId: function () {
                        return "forgotpassword";
                    }
                }
            });
            $routeProvider.otherwise({ redirectTo: '/about' });

            // use the HTML5 History API

            $locationProvider.html5Mode(true);
            $locationProvider.hashPrefix('!');
        }
    ])
    .config([
    '$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push([
            '$q', '$location', function ($q, $location) {
                return {
                    responseError: function (response) {
                        var whitelist = [
                            "/about",
                            "/help",
                            "/contact",
                            "/login",
                            "/register",
                            "/forgot-password"
                        ];
                        if (response.status == '401' && (whitelist.indexOf($location.path()) == -1)) {
                            $location.path('/about');
                        } else if (response.status == '503') {
                            $location.path('/maintenance');
                        } else {
                            return $q.reject(response);
                        }
                    }
                };
            }
        ]);
    }

    ]);
