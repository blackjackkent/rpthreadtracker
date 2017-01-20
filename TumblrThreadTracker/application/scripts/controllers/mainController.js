'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('MainController', [
    '$scope', '$location', '$analytics', 'threadService', 'contextService', 'blogService', 'newsService', 'sessionService', 'pageId', 'TrackerNotification',
    function($scope, $location, $analytics, threadService, contextService, blogService, newsService, sessionService, pageId, TrackerNotification) {
        $scope.setCurrentBlog = setCurrentBlog;
        $scope.setSortDescending = setSortDescending;
        $scope.setCurrentOrderBy = setCurrentOrderBy;
        $scope.setFilteredTag = setFilteredTag;
        $scope.bulkAction = bulkAction;
        $scope.untrackThreads = untrackThreads;
        $scope.archiveThreads = archiveThreads;
        $scope.unarchiveThreads = unarchiveThreads;
        $scope.refreshThreads = refreshThreads;
        $scope.setDashboardFilter = setDashboardFilter;
        $scope.toggleAtAGlanceData = toggleAtAGlanceData;
        $scope.generateRandomOwedThread = generateRandomOwedThread;
        $scope.$on("$destroy", destroyView);
        initView();

        // ******* functions *********
        function setCurrentBlog() {
            contextService.setCurrentBlog($scope.currentBlog);
            populateTagFilter();
            $analytics.eventTrack('Change Current Blog', { category: 'Private Thread View' });
        };
        function setSortDescending() {
            contextService.setSortDescending($scope.sortDescending);
            $analytics.eventTrack('Change Sort Descending', { category: 'Private Thread View' });
        };
        function setCurrentOrderBy() {
            contextService.setCurrentOrderBy($scope.currentOrderBy);
            $analytics.eventTrack('Change Order By', { category: 'Private Thread View' });
        };
        function setFilteredTag() {
            contextService.setFilteredTag($scope.filteredTag);
            $analytics.eventTrack('Change Filtered Tag', { category: 'Private Thread View' });
        };
        function bulkAction() {
            var bulkAffected = [];
            for (var property in $scope.bulkItems) {
                if ($scope.bulkItems.hasOwnProperty(property) && $scope.bulkItems[property] == true) {
                    bulkAffected.push(property);
                }
            }
            if ($scope.bulkItemAction == "UntrackSelected") {
                $scope.untrackThreads(bulkAffected);
            } else if ($scope.bulkItemAction == "ArchiveSelected") {
                $scope.archiveThreads(bulkAffected);
            } else if ($scope.bulkItemAction == "UnarchiveSelected") {
                $scope.unarchiveThreads(bulkAffected);
            }
        }
        function untrackThreads(userThreadIds) {
            threadService.flushThreads();
            threadService.untrackThreads(userThreadIds).then(function () {
                threadService.getThreads();
                threadService.getArchive();
            });
            new TrackerNotification()
                .withMessage(userThreadIds.length + " thread(s) untracked.")
                .withType("success")
                .show();
        };
        function archiveThreads(userThreadIds) {
            threadService.flushThreads();
            var threadsToArchive = [];
            angular.forEach(userThreadIds, function (id) {
                threadsToArchive.push(getThreadById(id));
            });
            threadService.editThreads(threadsToArchive, true).then(function () {
                threadService.getThreads();
                threadService.getArchive();
            });
            new TrackerNotification()
                .withMessage(userThreadIds.length + " thread(s) archived.")
                .withType("success")
                .show();
        };
        function unarchiveThreads(userThreadIds) {
            threadService.flushThreads();
            var threadsToArchive = [];
            angular.forEach(userThreadIds, function (id) {
                threadsToArchive.push(getThreadById(id));
            });
            threadService.editThreads(threadsToArchive, false).then(function () {
                threadService.getThreads();
                threadService.getArchive();
            });
            new TrackerNotification()
                .withMessage(userThreadIds.length + " thread(s) unarchived.")
                .withType("success")
                .show();
        };
        function refreshThreads() {
            if (pageId == "archived") {
                threadService.getArchive(true);
            } else {
                threadService.getThreads(true);
            }
        };
        function setDashboardFilter(filterString) {
            $scope.dashboardFilter = filterString;
            $analytics.eventTrack('Set Recent to ' + filterString, { category: 'Dashboard' });
        };
        function toggleAtAGlanceData() {
            if (!$scope.user) {
                return;
            }
            $scope.user.ShowDashboardThreadDistribution = $scope.showAtAGlance;
            sessionService.updateUser($scope.user);
        }
        function generateRandomOwedThread() {
            if (!$scope.threads) {
                return;
            }
            $scope.loadingRandomThread = true;
            $scope.randomlyGeneratedThread = null;
            var options = _.filter($scope.threads,
                function(thread) {
                    return thread.IsMyTurn;
                });
            $scope.randomlyGeneratedThread = _.sample(options);
            $scope.loadingRandomThread = false;
        }
        function updateThreads(data) {
            $scope.threads = data;
            $scope.myTurnCount = 0;
            $scope.theirTurnCount = 0;
            angular.forEach($scope.threads, function (thread) {
                $scope.myTurnCount += thread.IsMyTurn ? 1 : 0;
                $scope.theirTurnCount += thread.IsMyTurn ? 0 : 1;
            });
        }
        function getThreadById(id) {
            var result;
            angular.forEach($scope.threads, function (thread) {
                if (thread.UserThreadId == id) {
                    result = thread;
                    return;
                }
            });
            return result;
        }
        function populateTagFilter() {
            $scope.allTags = [];
            if ($scope.currentBlog == '') {
                angular.forEach($scope.tagsByBlog, function (value, key) {
                    angular.forEach(value, function (tag) {
                        if ($scope.allTags.indexOf(tag) == -1) {
                            $scope.allTags.push(tag);
                        }
                    });
                });
            } else {
                angular.forEach($scope.tagsByBlog[$scope.currentBlog], function (tag) {
                    if ($scope.allTags.indexOf(tag) == -1) {
                        $scope.allTags.push(tag);
                    }
                });
            }
        }
        function initView() {
            $scope.setBodyClass('');
            $scope.pageId = pageId;
            $scope.displayPublicUrl = true;
            $scope.dashboardFilter = 'yourturn';
            $scope.bulkItems = {};
            $scope.bulkItemAction = "UntrackSelected";
            $scope.showAtAGlance = false;
            $scope.loadingRandomThread = false;
            if (pageId == "archived") {
                threadService.subscribeOnArchiveUpdate(updateThreads);
                threadService.getArchive();
            } else {
                threadService.subscribe(updateThreads);
                threadService.getThreads();
            }
            $scope.currentBlog = contextService.getCurrentBlog();
            $scope.sortDescending = contextService.getSortDescending();
            $scope.currentOrderBy = contextService.getCurrentOrderBy();
            $scope.filteredTag = contextService.getFilteredTag();
            sessionService.getUser()
                .then(function(user) {
                    $scope.showAtAGlance = user.ShowDashboardThreadDistribution;
                });
            blogService.getBlogs()
                .then(function(blogs) {
                    $scope.blogs = blogs;
                });
            newsService.getNews()
                .then(function(news) {
                    $scope.news = news;
                });
            threadService.getTagsByBlog()
                .then(function(tagCollections) {
                    $scope.tagsByBlog = {};
                    angular.forEach(tagCollections,
                        function(collection) {
                            if (!$scope.tagsByBlog.hasOwnProperty(collection.BlogShortname)) {
                                $scope.tagsByBlog[collection.BlogShortname] = [];
                            }
                            angular.forEach(collection.TagCollection,
                                function(tag) {
                                    if ($scope.tagsByBlog[collection.BlogShortname].indexOf(tag) == -1) {
                                        $scope.tagsByBlog[collection.BlogShortname].push(tag);
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
]);