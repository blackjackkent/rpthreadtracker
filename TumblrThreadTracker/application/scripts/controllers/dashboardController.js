'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('DashboardController',
		[
			'$scope', '$controller', '$location', 'threadService', 'contextService',
			'blogService', 'newsService', 'sessionService', 'pageId', 'TrackerNotification',
			'BodyClass', '$mdDialog', dashboardController
		]);

	/** @this dashboardController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function dashboardController($scope, $controller, $location, threadService, contextService, blogService, newsService, sessionService, pageId, TrackerNotification, BodyClass, $mdDialog) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		sessionService.loadUser(vm);
		BodyClass.set('');

		initScopeValues();
		initSubscriptions();
		initScopeFunctions();
		$scope.$on('$destroy', destroyView);

		function initScopeValues() {
			vm.pageId = pageId;
			vm.dashboardFilter = 'yourturn';
			vm.showAtAGlance = false;
			vm.loadingRandomThread = false;
			sessionService.getUser().then(function(user) {
				vm.showAtAGlance = user.ShowDashboardThreadDistribution;
			});
			blogService.getBlogs().then(function(blogs) {
				vm.blogs = blogs;
			});
			newsService.getNews().then(function(news) {
				vm.news = news;
			});
		}

		function initScopeFunctions() {
			vm.untrackThreads = untrackThreads;
			vm.archiveThreads = archiveThreads;
			vm.loadThreads = loadThreads;
			vm.refreshThreads = refreshThreads;
			vm.setDashboardFilter = setDashboardFilter;
			vm.toggleAtAGlanceData = toggleAtAGlanceData;
			vm.generateRandomOwedThread = generateRandomOwedThread;
		}

		function initSubscriptions() {
			threadService.subscribeLoadedThreadEvent(loadThreads);
			threadService.loadThreads();
		}

		function loadThreads(data) {
			vm.threads = data;
			vm.myTurnCount = _.filter(vm.threads, function(thread) {
				return thread.IsMyTurn;
			}).length;
			vm.theirTurnCount = _.filter(vm.threads, function(thread) {
				return !thread.IsMyTurn;
			}).length;
		}

		function refreshThreads() {
			threadService.flushThreads();
			threadService.loadThreads();
		}

		function untrackThreads(threads) {
			var message = 'This will untrack ';
			message += threads.length;
			message += ' thread(s) from your account. Continue?';
			var confirm = $mdDialog.confirm()
				.title('Untrack Thread(s)')
				.textContent(message)
				.ok('Yes')
				.cancel('Cancel');
			$mdDialog.show(confirm).then(function() {
				vm.loading = true;
				threadService.untrackThreads(threads).then(function() {
					vm.loading = false;
					refreshThreads();
					new TrackerNotification()
						.withMessage(threads.length + ' thread(s) untracked.')
						.withType('success')
						.show();
				},
				function() {
					vm.loading = false;
					new TrackerNotification()
						.withMessage('There was an error untracking your threads.')
						.withType('error')
						.show();
				});
			});
		}

		function archiveThreads(threads) {
			vm.loading = true;
			threadService.archiveThreads(threads).then(function() {
				vm.loading = false;
				refreshThreads();
				new TrackerNotification()
					.withMessage(threads.length + ' thread(s) archived.')
					.withType('success')
					.show();
			}, function() {
				vm.loading = false;
				new TrackerNotification()
					.withMessage('There was an error archiving your threads.')
					.withType('error')
					.show();
			});
		}

		function setDashboardFilter(filterString) {
			vm.dashboardFilter = filterString;
		}

		function toggleAtAGlanceData() {
			if (!vm.user) {
				return;
			}
			vm.user.ShowDashboardThreadDistribution = vm.showAtAGlance;
			sessionService.updateUser(vm.user);
		}

		function generateRandomOwedThread() {
			if (!vm.threads) {
				return;
			}
			vm.loadingRandomThread = true;
			vm.randomlyGeneratedThread = null;
			var options = _.filter(vm.threads, function(thread) {
				return thread.IsMyTurn;
			});
			vm.randomlyGeneratedThread = _.sample(options);
			vm.loadingRandomThread = false;
		}

		function destroyView() {
			threadService.unsubscribeLoadedThreadEvent(loadThreads);
		}
	}
}());
