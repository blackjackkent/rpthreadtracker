(function() {
	"use strict";
	angular.module("rpthreadtracker")
		.controller("MainController",
		[
			"$scope", "$controller", "$location", "$analytics", "threadService", "contextService", "blogService", "newsService",
			"sessionService", "pageId", "TrackerNotification", "BodyClass",
			mainController
		]);

	function mainController($scope, $controller, $location, $analytics, threadService, contextService, blogService,
		newsService, sessionService, pageId, TrackerNotification, BodyClass) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', { $scope: $scope }));

		vm.setCurrentBlog = setCurrentBlog;
		vm.setSortDescending = setSortDescending;
		vm.setCurrentOrderBy = setCurrentOrderBy;
		vm.setFilteredTag = setFilteredTag;
		vm.bulkAction = bulkAction;
		vm.untrackThreads = untrackThreads;
		vm.archiveThreads = archiveThreads;
		vm.unarchiveThreads = unarchiveThreads;
		vm.refreshThreads = refreshThreads;
		vm.setDashboardFilter = setDashboardFilter;
		vm.toggleAtAGlanceData = toggleAtAGlanceData;
		vm.generateRandomOwedThread = generateRandomOwedThread;
		vm.$on("$destroy", destroyView);
		initView();

		// ******* functions *********
		function setCurrentBlog() {
			contextService.setCurrentBlog(vm.currentBlog);
			populateTagFilter();
			$analytics.eventTrack("Change Current Blog", { category: "Private Thread View" });
		}

		function setSortDescending() {
			contextService.setSortDescending(vm.sortDescending);
			$analytics.eventTrack("Change Sort Descending", { category: "Private Thread View" });
		}

		function setCurrentOrderBy() {
			contextService.setCurrentOrderBy(vm.currentOrderBy);
			$analytics.eventTrack("Change Order By", { category: "Private Thread View" });
		}

		function setFilteredTag() {
			contextService.setFilteredTag(vm.filteredTag);
			$analytics.eventTrack("Change Filtered Tag", { category: "Private Thread View" });
		}

		function bulkAction() {
			var bulkAffected = [];
			for (var property in vm.bulkItems) {
				if (vm.bulkItems.hasOwnProperty(property) && vm.bulkItems[property] == true) {
					bulkAffected.push(property);
				}
			}
			if (vm.bulkItemAction == "UntrackSelected") {
				vm.untrackThreads(bulkAffected);
			} else if (vm.bulkItemAction == "ArchiveSelected") {
				vm.archiveThreads(bulkAffected);
			} else if (vm.bulkItemAction == "UnarchiveSelected") {
				vm.unarchiveThreads(bulkAffected);
			}
		}

		function untrackThreads(userThreadIds) {
			threadService.flushThreads();
			threadService.untrackThreads(userThreadIds)
				.then(function() {
					threadService.getThreads();
					threadService.getArchive();
				});
			new TrackerNotification()
				.withMessage(userThreadIds.length + " thread(s) untracked.")
				.withType("success")
				.show();
		}

		function archiveThreads(userThreadIds) {
			threadService.flushThreads();
			var threadsToArchive = [];
			angular.forEach(userThreadIds,
				function(id) {
					threadsToArchive.push(getThreadById(id));
				});
			threadService.editThreads(threadsToArchive, true)
				.then(function() {
					threadService.getThreads();
					threadService.getArchive();
				});
			new TrackerNotification()
				.withMessage(userThreadIds.length + " thread(s) archived.")
				.withType("success")
				.show();
		}

		function unarchiveThreads(userThreadIds) {
			threadService.flushThreads();
			var threadsToArchive = [];
			angular.forEach(userThreadIds,
				function(id) {
					threadsToArchive.push(getThreadById(id));
				});
			threadService.editThreads(threadsToArchive, false)
				.then(function() {
					threadService.getThreads();
					threadService.getArchive();
				});
			new TrackerNotification()
				.withMessage(userThreadIds.length + " thread(s) unarchived.")
				.withType("success")
				.show();
		}

		function refreshThreads() {
			if (pageId == "archived") {
				threadService.getArchive(true);
			} else {
				threadService.getThreads(true);
			}
		}

		function setDashboardFilter(filterString) {
			vm.dashboardFilter = filterString;
			$analytics.eventTrack("Set Recent to " + filterString, { category: "Dashboard" });
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
			var options = _.filter(vm.threads,
				function(thread) {
					return thread.IsMyTurn;
				});
			vm.randomlyGeneratedThread = _.sample(options);
			vm.loadingRandomThread = false;
		}

		function updateThreads(data) {
			vm.threads = data;
			vm.myTurnCount = 0;
			vm.theirTurnCount = 0;
			angular.forEach(vm.threads,
				function(thread) {
					vm.myTurnCount += thread.IsMyTurn ? 1 : 0;
					vm.theirTurnCount += thread.IsMyTurn ? 0 : 1;
				});
		}

		function getThreadById(id) {
			var result;
			angular.forEach(vm.threads,
				function(thread) {
					if (thread.UserThreadId == id) {
						result = thread;
						return;
					}
				});
			return result;
		}

		function populateTagFilter() {
			vm.allTags = [];
			if (vm.currentBlog == "") {
				angular.forEach(vm.tagsByBlog,
					function(value) {
						angular.forEach(value,
							function(tag) {
								if (vm.allTags.indexOf(tag) == -1) {
									vm.allTags.push(tag);
								}
							});
					});
			} else {
				angular.forEach(vm.tagsByBlog[vm.currentBlog],
					function(tag) {
						if (vm.allTags.indexOf(tag) == -1) {
							vm.allTags.push(tag);
						}
					});
			}
		}

		function initView() {
			sessionService.loadUser(vm);
			BodyClass.set('');
			vm.pageId = pageId;
			vm.displayPublicUrl = true;
			vm.dashboardFilter = "yourturn";
			vm.bulkItems = {};
			vm.bulkItemAction = "UntrackSelected";
			vm.showAtAGlance = false;
			vm.loadingRandomThread = false;
			if (pageId == "archived") {
				threadService.subscribeOnArchiveUpdate(updateThreads);
				threadService.getArchive();
			} else {
				threadService.subscribe(updateThreads);
				threadService.getThreads();
			}
			vm.currentBlog = contextService.getCurrentBlog();
			vm.sortDescending = contextService.getSortDescending();
			vm.currentOrderBy = contextService.getCurrentOrderBy();
			vm.filteredTag = contextService.getFilteredTag();
			sessionService.getUser()
				.then(function(user) {
					vm.showAtAGlance = user.ShowDashboardThreadDistribution;
				});
			blogService.getBlogs()
				.then(function(blogs) {
					vm.blogs = blogs;
				});
			newsService.getNews()
				.then(function(news) {
					vm.news = news;
				});
			threadService.getTagsByBlog()
				.then(function(tagCollections) {
					vm.tagsByBlog = {};
					angular.forEach(tagCollections,
						function(collection) {
							if (!vm.tagsByBlog.hasOwnProperty(collection.BlogShortname)) {
								vm.tagsByBlog[collection.BlogShortname] = [];
							}
							angular.forEach(collection.TagCollection,
								function(tag) {
									if (vm.tagsByBlog[collection.BlogShortname].indexOf(tag) == -1) {
										vm.tagsByBlog[collection.BlogShortname].push(tag);
									}
								});
						});
					populateTagFilter();
				});
		}

		function destroyView() {
			threadService.unsubscribe(updateThreads);
			threadService.unsubscribeOnArchiveUpdate(updateThreads);
		}
	}
})();