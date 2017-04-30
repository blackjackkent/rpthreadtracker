'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('ThreadsController',
		[
			'$scope', '$controller', '$window', 'threadService', 'contextService',
			'blogService', 'newsService', 'sessionService', 'pageId', 'notificationService',
            'NOTIFICATION_TYPES', 'BodyClass', 'THREAD_BULK_ACTIONS', '$mdDialog', 'THREAD_PAGE_IDS', threadsController
		]);

	/** @this dashboardController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
    function threadsController($scope, $controller, $window, threadService, contextService, blogService, newsService, sessionService, pageId, notificationService, NOTIFICATION_TYPES, BodyClass, THREAD_BULK_ACTIONS, $mdDialog, THREAD_PAGE_IDS) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		sessionService.loadUser(vm);
		BodyClass.set('');

		initScopeContextValues();
		initScopeDataValues();
		initSubscriptions();
		initScopeFunctions();
		$scope.$on('$destroy', destroyView);

		function initScopeContextValues() {
			vm.pageId = pageId;
			vm.currentBlog = contextService.getCurrentBlog();
			vm.sortDescending = contextService.getSortDescending();
			vm.currentOrderBy = contextService.getCurrentOrderBy();
			vm.filteredTag = contextService.getFilteredTag();
			vm.bulkItemAction = THREAD_BULK_ACTIONS.UNTRACK;
		}

		function initScopeDataValues() {
			vm.threads = [];
			vm.blogs = [];
			vm.noBlogs = false;
			vm.noThreads = false;
			blogService.getBlogs().then(function(blogs) {
				if (blogs.length === 0) {
					vm.noBlogs = true;
				}
				vm.blogs = blogs;
				if (!_.find(vm.blogs, function(blog) {
					return vm.currentBlog && blog.UserBlogId === vm.currentBlog.UserBlogId;
				})) {
					vm.currentBlog = null;
				}
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
            vm.markQueued = markQueued;
			vm.unmarkQueued = unmarkQueued;
		}

		function initSubscriptions() {
			threadService.subscribeLoadedThreadEvent(onThreadLoaded);
            threadService.subscribeLoadedArchiveThreadEvent(onThreadLoaded);
            threadService.subscribeAllThreadsLoaded(onAllThreadsLoaded);
			if (vm.pageId === THREAD_PAGE_IDS.ARCHIVED) {
				threadService.loadArchivedThreads(true);
			} else {
				threadService.loadThreads(true);
			}
		}

		function onThreadLoaded(data) {
			vm.threads = data;
			populateTagFilter();
		}

		function onAllThreadsLoaded() {
			if (vm.threads.length === 0) {
				vm.noThreads = true;
			}
		}

		function refreshThreads() {
			vm.noThreads = false;
			threadService.flushThreads();
			if (vm.pageId === THREAD_PAGE_IDS.ARCHIVED) {
				threadService.loadArchivedThreads(true);
            } else {
                threadService.loadThreads(true);
			}
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
					var type = NOTIFICATION_TYPES.UNTRACK_THREAD_SUCCESS;
					notificationService.show(type, {'threads': threads});
				},
				function() {
					vm.loading = false;
					var type = NOTIFICATION_TYPES.UNTRACK_THREAD_FAILURE;
					notificationService.show(type);
				});
			});
		}

		function archiveThreads(threads) {
			vm.loading = true;
			threadService.archiveThreads(threads).then(function() {
				vm.loading = false;
				refreshThreads();
				var type = NOTIFICATION_TYPES.ARCHIVE_THREAD_SUCCESS;
				notificationService.show(type, {'threads': threads});
			}, function() {
				vm.loading = false;
				var type = NOTIFICATION_TYPES.ARCHIVE_THREAD_FAILURE;
				notificationService.show(type);
			});
		}

		function unarchiveThreads(threads) {
			vm.loading = true;
			threadService.unarchiveThreads(threads).then(function() {
				vm.loading = false;
				refreshThreads();
				var type = NOTIFICATION_TYPES.UNARCHIVE_THREAD_SUCCESS;
				notificationService.show(type, {'threads': threads});
			}, function() {
				vm.loading = false;
				var type = NOTIFICATION_TYPES.UNARCHIVE_THREAD_FAILURE;
				notificationService.show(type);
			});
		}

        function markQueued(threads) {
            vm.loading = true;
            threadService.markThreadsQueued(threads).then(function () {
                vm.loading = false;
                refreshThreads();
                var type = NOTIFICATION_TYPES.QUEUE_THREAD_SUCCESS;
                notificationService.show(type, { 'threads': threads });
            }, function () {
                vm.loading = false;
                var type = NOTIFICATION_TYPES.QUEUE_THREAD_FAILURE;
                notificationService.show(type);
            });
        }

        function unmarkQueued(threads) {
	        vm.loading = true;
	        threadService.unmarkThreadsQueued(threads).then(function () {
		        vm.loading = false;
		        refreshThreads();
		        var type = NOTIFICATION_TYPES.UNQUEUE_THREAD_SUCCESS;
		        notificationService.show(type, { 'threads': threads });
	        }, function () {
		        vm.loading = false;
		        var type = NOTIFICATION_TYPES.UNQUEUE_THREAD_FAILURE;
		        notificationService.show(type);
	        });
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
			var bulkAffected = _.filter(vm.threads, function(thread) {
				return thread.SelectedForBulk;
			});
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
			if (!_.find(vm.allTags, function(tag) {
				return tag === vm.filteredTag;
			})) {
				vm.filteredTag = '';
			}
		}

		function buildPublicLink() {
			var url = $window.location.origin;
			url += '/public/' + vm.pageId;
			url += '?userId=' + (vm.user ? vm.user.UserId : '');
			url += '&currentBlog=' + (vm.currentBlog ? vm.currentBlog.BlogShortname : '');
			url += '&sortDescending=' + vm.sortDescending;
			url += '&currentOrderBy=' + vm.currentOrderBy;
			url += '&filteredTag=' + vm.filteredTag;
			return url;
		}

		function destroyView() {
			threadService.unsubscribeLoadedThreadEvent(onThreadLoaded);
            threadService.unsubscribeLoadedArchiveThreadEvent(onThreadLoaded);
			threadService.unsubscribeAllThreadsLoaded(onAllThreadsLoaded);
		}
	}
}());
