'use strict';
(function() {
	angular.module('rpthreadtracker',
		[
			'ngRoute', 'angulartics', 'angulartics.google.analytics',
			'frapontillo.bootstrap-switch', 'ui-notification', 'ngMaterial'
		]);

	var cacheBuster = Date.now();
	angular.module('rpthreadtracker')
		.constant('cacheBuster', cacheBuster)
		.constant('SESSION_EVENTS', {'LOGIN': 1, 'LOGOUT': 2});
	angular.module('rpthreadtracker')
		.config(['$routeProvider', '$locationProvider', routeConfig])
		.config(['NotificationProvider', notificationConfig])
		.config(['$httpProvider', interceptorConfig]);
	configureConstants(angular.module('rpthreadtracker'));

	// eslint-disable-next-line max-statements
	function routeConfig($routeProvider, $locationProvider) {
		$routeProvider.when('/maintenance',
			{
				'templateUrl': '/application/views/maintenance.html',
				'controller': 'StaticController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'maintenance';
					}
				}
			});

		$routeProvider.when('/',
			{
				'templateUrl': '/application/views/dashboard.html',
				'controller': 'DashboardController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'dashboard';
					}
				}
			});
		$routeProvider.when('/threads',
			{
				'templateUrl': '/application/views/threads.html?cacheBuster=' + cacheBuster,
				'controller': 'ThreadsController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'allthreads';
					}
				}
			});
		$routeProvider.when('/threads/allthreads',
			{
				'templateUrl': '/application/views/threads.html?cacheBuster=' + cacheBuster,
				'controller': 'ThreadsController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'allthreads';
					}
				}
			});
		$routeProvider.when('/threads/yourturn',
			{
				'templateUrl': '/application/views/threads.html?cacheBuster=' + cacheBuster,
				'controller': 'ThreadsController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'yourturn';
					}
				}
			});
		$routeProvider.when('/threads/theirturn',
			{
				'templateUrl': '/application/views/threads.html?cacheBuster=' + cacheBuster,
				'controller': 'ThreadsController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'theirturn';
					}
				}
			});
		$routeProvider.when('/threads/archived',
			{
				'templateUrl': '/application/views/threads.html?cacheBuster=' + cacheBuster,
				'controller': 'ThreadsController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'archived';
					}
				}
            });
        $routeProvider.when('/threads/queued',
            {
                'templateUrl': '/application/views/threads.html?cacheBuster=' + cacheBuster,
                'controller': 'ThreadsController',
                'controllerAs': 'vm',
                'resolve': {
                    'pageId': function () {
                        return 'queued';
                    }
                }
            });
		$routeProvider.when('/public/:pageId',
			{
				'templateUrl': '/application/views/public.html?cacheBuster=' + cacheBuster,
				'controller': 'PublicController',
				'controllerAs': 'vm'
			});
		$routeProvider.when('/login',
			{
				'templateUrl': '/application/views/login.html?v=10614',
				'controller': 'LoginController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'login';
					}
				}
			});
		$routeProvider.when('/register',
			{
				'templateUrl': '/application/views/register.html?cacheBuster=' + cacheBuster,
				'controller': 'RegisterController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'register';
					}
				}
			});
		$routeProvider.when('/add-thread',
			{
				'templateUrl': '/application/views/add-thread.html?cacheBuster=' + cacheBuster,
				'controller': 'ManageThreadController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'add-thread';
					}
				}
			});
		$routeProvider.when('/edit-thread/:userThreadId',
			{
				'templateUrl': '/application/views/add-thread.html?cacheBuster=' + cacheBuster,
				'controller': 'ManageThreadController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'edit-thread';
					}
				}
			});
		$routeProvider.when('/manage-blogs',
			{
				'templateUrl': '/application/views/manage-blogs.html?cacheBuster=' + cacheBuster,
				'controller': 'ManageBlogsController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'manage-blogs';
					}
				}
			});
		$routeProvider.when('/edit-blog/:userBlogId',
			{
				'templateUrl': '/application/views/edit-blog.html?cacheBuster=' + cacheBuster,
				'controller': 'EditBlogController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'edit-blog';
					}
				}
			});
		$routeProvider.when('/manage-account',
			{
				'templateUrl': '/application/views/manage-account.html?cacheBuster=' + cacheBuster,
				'controller': 'ManageAccountController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'manage-account';
					}
				}
			});
		$routeProvider.when('/about',
			{
				'templateUrl': '/application/views/about.html?cacheBuster=' + cacheBuster,
				'controller': 'StaticController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'about';
					}
				}
			});
		$routeProvider.when('/contact',
			{
				'templateUrl': '/application/views/contact.html?cacheBuster=' + cacheBuster,
				'controller': 'StaticController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'contact';
					}
				}
			});
		$routeProvider.when('/help',
			{
				'templateUrl': '/application/views/help.html?cacheBuster=' + cacheBuster,
				'controller': 'StaticController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'help';
					}
				}
			});
		$routeProvider.when('/logout',
			{
				'templateUrl': '/application/views/login.html?cacheBuster=' + cacheBuster,
				'controller': 'LogoutController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'logout';
					}
				}
			});
		$routeProvider.when('/forgot-password',
			{
				'templateUrl': '/application/views/forgot-password.html?cacheBuster=' + cacheBuster,
				'controller': 'ForgotPasswordController',
				'controllerAs': 'vm',
				'resolve': {
					'pageId': function() {
						return 'forgotpassword';
					}
				}
			});
		$routeProvider.otherwise({'redirectTo': '/about'});

		// Use the HTML5 History API

		$locationProvider.html5Mode(true);
		$locationProvider.hashPrefix('!');
	}

	function notificationConfig(NotificationProvider) {
		NotificationProvider.setOptions({
			'delay': 5000,
			'positionX': 'center',
			'positionY': 'top',
			'verticalSpacing': 20,
			'horizontalSpacing': 20,
			'startTop': 50
		});
	}

	function interceptorConfig($httpProvider) {
		$httpProvider.interceptors.push(['$q', '$location', responseInterceptor]);
		$httpProvider.interceptors.push(['$q', '$location', '$window', requestInterceptor]);
	}

	function requestInterceptor($q, $location, $window) {
		return {
			'request': function(httpConfig) {
				var token = $window.localStorage.TrackerBearerToken;
				if (token) {
					httpConfig.headers.Authorization = 'Bearer ' + token;
				}
				return httpConfig;
			}
		};
	}

	function responseInterceptor($q, $location) {
		return {
			'responseError': function(response) {
				var whitelist = [
					'/about',
					'/help',
					'/contact',
					'/login',
					'/register',
					'/forgot-password',
					'/public/yourturn',
					'/public/theirturn',
					'/public/allthreads'
				];
				var isNotInWhitelist = whitelist.indexOf($location.path()) === -1;
				if (response.status === 401 && isNotInWhitelist) {
					$location.path('/about');
				} else if (response.status === 503) {
					$location.path('/maintenance');
				} else {
					return $q.reject(response);
				}
			}
		};
	}

	function configureConstants(app) {
		app.constant('THREAD_BULK_ACTIONS',
			{
				'UNTRACK': 1,
				'ARCHIVE': 2,
                'UNARCHIVE': 3,
                'QUEUE': 4,
                'UNQUEUE': 5
			});
		app.constant('NOTIFICATION_TYPES',
			{
				'UNTRACK_THREAD_SUCCESS': 1,
				'UNTRACK_THREAD_FAILURE': 2,
				'ARCHIVE_THREAD_SUCCESS': 3,
				'ARCHIVE_THREAD_FAILURE': 4,
				'FORGOT_PASSWORD_SUCCESS': 5,
				'FORGOT_PASSWORD_FAILURE': 6,
				'LOGIN_FAILURE': 7,
				'UPDATE_ACCOUNT_SUCCESS': 8,
				'UPDATE_ACCOUNT_FAILURE': 9,
				'CHANGE_PASSWORD_VALIDATION_ERROR': 10,
				'EXPORT_FAILURE': 11,
				'UPDATE_BLOGS_SUCCESS': 12,
				'UPDATE_BLOGS_FAILURE': 13,
				'CREATE_BLOG_VALIDATION_ERROR': 14,
				'CREATE_THREAD_EXT_BLOG_DNE': 15,
				'CREATE_THREAD_SUCCESS': 16,
				'CREATE_THREAD_FAILURE': 17,
				'CREATE_THREAD_VALIDATION_ERROR': 18,
				'REGISTER_VALIDATION_ERROR': 19,
				'REGISTER_FAILURE': 20,
				'UNARCHIVE_THREAD_SUCCESS': 21,
                'UNARCHIVE_THREAD_FAILURE': 22,
                'QUEUE_THREAD_SUCCESS': 23,
                'QUEUE_THREAD_FAILURE': 24,
                'UNQUEUE_THREAD_SUCCESS': 25,
                'UNQUEUE_THREAD_FAILURE': 26,
				'POST_ID_ALREADY_TRACKED': 27
            });
        app.constant('THREAD_PAGE_IDS',
	        {
                'YOUR_TURN': 'yourturn',
                'THEIR_TURN': 'theirturn',
                'ARCHIVED': 'archived',
                'QUEUED': 'queued'
	        })
	}
}());
