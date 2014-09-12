'use strict';

// Declare app level module which depends on filters, and services
angular.module('rpThreadTracker', [
        'ngRoute',
        'rpThreadTracker.filters',
        'rpThreadTracker.services',
        'rpThreadTracker.directives',
        'rpThreadTracker.controllers'
    ])
    .config([
        '$routeProvider', '$locationProvider', function($routeProvider, $locationProvider) {
            $routeProvider.when('/', { templateUrl: '/application/views/dashboard.html', controller: 'MainController' });
            $routeProvider.when('/', {
                templateUrl: '/application/views/dashboard.html',
                controller: 'MainController',
                resolve: {
                    pageId: function() {
                        return "dashboard";
                    }
                }
            });
            $routeProvider.when('/threads', {
                templateUrl: '/application/views/threads.html',
                controller: 'MainController',
                resolve: {
                    pageId: function() {
                        return "allthreads";
                    }
                }
            });
            $routeProvider.when('/threads/yourturn', {
                templateUrl: '/application/views/threads.html',
                controller: 'MainController',
                resolve: {
                    pageId: function() {
                        return "yourturn";
                    }
                }
            });
            $routeProvider.when('/threads/theirturn', {
                templateUrl: '/application/views/threads.html',
                controller: 'MainController',
                resolve: {
                    pageId: function() {
                        return "theirturn";
                    }
                }
            });
            $routeProvider.when('/public/:pageId', {
                templateUrl: '/application/views/public.html',
                controller: 'PublicController'
            });
            $routeProvider.when('/login', {
                templateUrl: '/application/views/login.html',
                controller: 'LoginController',
                resolve: {
                    pageId: function () {
                        return "login";
                    }
                }
            });
            $routeProvider.when('/register', {
                templateUrl: '/application/views/register.html',
                controller: 'RegisterController',
                resolve: {
                    pageId: function () {
                        return "register";
                    }
                }
            });

            $routeProvider.when('/add-thread', {
                templateUrl: '/application/views/add-thread.html',
                controller: 'AddThreadController',
                resolve: {
                    pageId: function () {
                        return "add-thread";
                    }
                }
            });

            $routeProvider.when('/edit-thread/:userThreadId', {
                templateUrl: '/application/views/add-thread.html',
                controller: 'EditThreadController',
                resolve: {
                    pageId: function () {
                        return "edit-thread";
                    }
                }
            });

            $routeProvider.when('/manage-blogs', {
                templateUrl: '/application/views/manage-blogs.html',
                controller: 'ManageBlogsController',
                resolve: {
                    pageId: function () {
                        return "manage-blogs";
                    }
                }
            });

            $routeProvider.when('/edit-blog/:userBlogId', {
                templateUrl: '/application/views/edit-blog.html',
                controller: 'EditBlogController',
                resolve: {
                    pageId: function () {
                        return "edit-blog";
                    }
                }
            });

            $routeProvider.when('/about', {
                templateUrl: '/application/views/about.html',
                controller: 'StaticController',
                resolve: {
                    pageId: function () {
                        return "about";
                    }
                }
            });

            $routeProvider.when('/contact', {
                templateUrl: '/application/views/contact.html',
                controller: 'StaticController',
                resolve: {
                    pageId: function () {
                        return "contact";
                    }
                }
            });

            $routeProvider.when('/help', {
                templateUrl: '/application/views/help.html',
                controller: 'StaticController',
                resolve: {
                    pageId: function () {
                        return "help";
                    }
                }
            });

            $routeProvider.when('/logout', {
                templateUrl: '/application/views/login.html',
                controller: 'LogoutController',
                resolve: {
                    pageId: function () {
                        return "logout";
                    }
                }
            });
            $routeProvider.otherwise({ redirectTo: '/' });

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
                        if (response.status == '401') {
                            $location.path('/login');
                        } else {
                            return $q.reject(response);
                        }
                    }
                };
            }
        ]);
    }
]);
