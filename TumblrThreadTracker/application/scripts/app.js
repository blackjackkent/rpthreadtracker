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
    ]);
