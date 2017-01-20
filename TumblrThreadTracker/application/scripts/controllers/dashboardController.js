'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('DashboardController',
		[
			'$scope', '$controller', '$location', 'threadService', 'contextService',
			'blogService', 'newsService', 'sessionService', 'pageId', 'TrackerNotification',
			'BodyClass', dashboardController
		]);

	/** @this dashboardController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function dashboardController($scope, $controller, $location, threadService, contextService, blogService, newsService, sessionService, pageId, TrackerNotification, BodyClass) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		sessionService.loadUser(vm);
		BodyClass.set('');

		initScopeValues();
		initScopeFunctions();
		initSubscriptions();
		$scope.$on('$destroy', destroyView);

		function untrackThreads(userThreadIds) {
			threadService.untrackThreads(userThreadIds)
				.then(function() {
					threadService.getThreads(true);
				});
			new TrackerNotification()
				.withMessage(userThreadIds.length + ' thread(s) untracked.')
				.withType('success')
				.show();
		}

		function archiveThreads(userThreadIds) {
			threadService.archiveThreads(userThreadIds)
				.then(function() {
					threadService.getThreads(true);
				});
			new TrackerNotification()
				.withMessage(userThreadIds.length + ' thread(s) archived.')
				.withType('success')
				.show();
		}

		function refreshThreads() {
			threadService.getThreads(true);
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

		function updateThreads(data) {
			vm.threads = data;
			vm.myTurnCount = 0;
			vm.theirTurnCount = 0;
			angular.forEach(vm.threads, function(thread) {
				vm.myTurnCount += thread.IsMyTurn ? 1 : 0;
				vm.theirTurnCount += thread.IsMyTurn ? 0 : 1;
			});
		}

		function initScopeValues() {
			vm.pageId = pageId;
			vm.displayPublicUrl = true;
			vm.dashboardFilter = 'yourturn';
			vm.bulkItems = {};
			vm.bulkItemAction = 'UntrackSelected';
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
			vm.refreshThreads = refreshThreads;
			vm.setDashboardFilter = setDashboardFilter;
			vm.toggleAtAGlanceData = toggleAtAGlanceData;
			vm.generateRandomOwedThread = generateRandomOwedThread;
		}

		function initSubscriptions() {
			threadService.subscribe(updateThreads);
			threadService.getThreads();
		}

		function destroyView() {
			threadService.unsubscribe(updateThreads);
			threadService.unsubscribeOnArchiveUpdate(updateThreads);
		}
	}
}());
