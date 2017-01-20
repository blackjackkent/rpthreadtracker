(function() {
	"use strict";
	angular.module("rpthreadtracker")
		.controller("PublicController",
		[
			"$scope", "$routeParams", "$filter", "publicThreadService", publicController
		]);

	function publicController($scope, $routeParams, $filter, publicThreadService) {
		$scope.setBodyClass("centered-layout error-page");
		$scope.pageId = $routeParams.pageId;
		$scope.userId = $routeParams.userId;
		$scope.currentBlog = $routeParams.currentBlog;
		$scope.currentOrderBy = $routeParams.currentOrderBy;
		$scope.sortDescending = $routeParams.sortDescending == "false" ? false : true;
		$scope.filteredTag = $routeParams.filteredTag;
		initView();

		function initView() {
			$scope.publicTitleString = buildPublicTitleString();
			publicThreadService.subscribe(updateThreads);
			publicThreadService.getThreads($scope.userId, $scope.currentBlog);
		}

		function updateThreads(data) {
			$scope.threads = data;
			var filtered = $filter("isCurrentBlog")(data, $scope.currentBlog);
			filtered = $filter("isCorrectTurn")(filtered, $scope.pageId);
			filtered = $filter("containsFilteredTag")(filtered, decodeURIComponent($scope.filteredTag));
			$scope.threadCount = filtered.length;
		}

		function buildPublicTitleString() {
			var result = "";
			if ($scope.pageId == "yourturn") {
				result += "Threads I Owe";
			} else if ($scope.pageId == "theirturn") {
				result += "Threads Awaiting Reply";
			} else {
				result += "All Threads";
			}
			if ($scope.currentBlog != "") {
				result += " on " + $scope.currentBlog;
			}
			if ($scope.filteredTag != "") {
				result += " tagged '" + $scope.filteredTag + "'";
			}
			return result;
		}

	}
})();