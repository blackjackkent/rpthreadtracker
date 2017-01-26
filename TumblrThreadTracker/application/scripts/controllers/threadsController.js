'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('ThreadsController',
		[
			'$scope', '$controller', '$window', 'threadService', 'contextService',
			'blogService', 'newsService', 'sessionService', 'pageId', 'TrackerNotification',
			'BodyClass', 'THREAD_BULK_ACTIONS', threadsController
		]);

	/** @this dashboardController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function threadsController($scope, $controller, $window, threadService, contextService, blogService, newsService, sessionService, pageId, TrackerNotification, BodyClass, THREAD_BULK_ACTIONS) {
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
			vm.currentBlog = contextService.getCurrentBlog();
			vm.sortDescending = contextService.getSortDescending();
			vm.currentOrderBy = contextService.getCurrentOrderBy();
			vm.filteredTag = contextService.getFilteredTag();
			vm.bulkItemAction = 'UntrackSelected';
			blogService.getBlogs().then(function(blogs) {
				vm.blogs = blogs;
			});
		}

		function initScopeFunctions() {
			vm.setCurrentBlog = setCurrentBlog;
			vm.setSortDescending = setSortDescending;
			vm.setCurrentOrderBy = setCurrentOrderBy;
			vm.setFilteredTag = setFilteredTag;
			vm.untrackThreads = untrackThreads;
			vm.archiveThreads = archiveThreads;
			vm.unarchiveThreads = unarchiveThreads;
			vm.refreshThreads = refreshThreads;
			vm.bulkAction = bulkAction;
			vm.buildPublicLink = buildPublicLink;
		}

		function initSubscriptions() {
			threadService.subscribeLoadedThreadEvent(loadThreads);
			threadService.subscribeLoadedArchiveThreadEvent(loadThreads);
			if (vm.pageId === 'archived') {
				threadService.loadArchivedThreads();
			} else {
				threadService.loadThreads();
			}
		}

		function loadThreads(data) {
			vm.threads = data;
			populateTagFilter();
		}

		function refreshThreads() {
			if (vm.pageId === 'archived') {
				threadService.loadArchivedThreads(true);
			} else {
				threadService.loadThreads(true);
			}
		}

		function untrackThreads(userThreadIds) {
			threadService.untrackThreads(userThreadIds).then(function() {
				threadService.loadThreads();
				new TrackerNotification()
					.withMessage(userThreadIds.length + ' thread(s) untracked.')
					.withType('success')
					.show();
			}, function() {
				new TrackerNotification()
					.withMessage("There was an error untracking your threads.")
					.withType('error')
					.show();
			});
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

		function unarchiveThreads(userThreadIds) {
	
		}

		function setCurrentBlog() {
			contextService.setCurrentBlog(vm.currentBlog);
			populateTagFilter();
		}

		function setSortDescending() {
			contextService.setSortDescending(vm.sortDescending);
		}

		function setCurrentOrderBy() {
			contextService.setCurrentOrderBy(vm.currentOrderBy);
		}

		function setFilteredTag() {
			contextService.setFilteredTag(vm.filteredTag);
		}

		function bulkAction() {
			var bulkAffected = [];
			for (var property in vm.bulkItems) {
				if (vm.bulkItems.hasOwnProperty(property) && vm.bulkItems[property] === true) {
					bulkAffected.push(property);
				}
			}
			if (vm.bulkItemAction === THREAD_BULK_ACTIONS.UNTRACK) {
				vm.untrackThreads(bulkAffected);
			} else if (vm.bulkItemAction === THREAD_BULK_ACTIONS.ARCHIVE) {
				vm.archiveThreads(bulkAffected);
			} else if (vm.bulkItemAction === THREAD_BULK_ACTIONS.UNARCHIVE) {
				vm.unarchiveThreads(bulkAffected);
			}
		}

		function populateTagFilter() {
			var tagsByThread = _.map(vm.threads, 'ThreadTags');
			vm.allTags = _.union(_.flatten(tagsByThread));
		}

		function buildPublicLink() {
			var url = $window.location.origin;
			url += '/public/' + vm.pageId;
			url += '?userId=' + (vm.user ? vm.user.UserId : '');
			url += '&currentBlog=' + vm.currentBlog;
			url += '&sortDescending=' + vm.sortDescending;
			url += '&currentOrderBy=' + vm.currentOrderBy;
			url += '&filteredTag=' + vm.filteredTag;
			return url;
		}

		function destroyView() {
			threadService.unsubscribeLoadedThreadEvent(loadThreads);
			threadService.unsubscribeLoadedArchiveThreadEvent(loadThreads);
		}
	}
}());
