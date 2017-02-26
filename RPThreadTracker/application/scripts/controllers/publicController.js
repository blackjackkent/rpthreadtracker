'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('PublicController',
		[
			'$scope', '$controller', '$routeParams', '$filter', 'publicThreadService',
			'BodyClass', publicController
		]);

	/** @this publicController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function publicController($scope, $controller, $routeParams, $filter, publicThreadService, BodyClass) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		BodyClass.set('centered-layout error-page');

		initScopeValues();
		initView();

		function initScopeValues() {
			vm.pageId = $routeParams.pageId;
			vm.userId = $routeParams.userId;
			vm.currentBlog = $routeParams.currentBlog;
			vm.currentOrderBy = $routeParams.currentOrderBy;
			vm.sortDescending = $routeParams.sortDescending !== 'false';
			vm.filteredTag = $routeParams.filteredTag;
		}

		function initView() {
			vm.publicTitleString = buildPublicTitleString();
			publicThreadService.subscribeLoadedThreadEvent(updateThreads);
			publicThreadService.loadThreads(vm.userId, vm.currentBlog);
		}

		function updateThreads(data) {
			vm.threads = data;
			var filtered = $filter('isCurrentBlog')(data, vm.currentBlog);
			filtered = $filter('isCorrectTurn')(filtered, vm.pageId);
			filtered = $filter('containsFilteredTag')(filtered, decodeURIComponent(vm.filteredTag));
			vm.threadCount = filtered.length;
		}

		function buildPublicTitleString() {
			var result = '';
			if (vm.pageId === 'yourturn') {
				result += 'Threads I Owe';
			} else if (vm.pageId === 'theirturn') {
				result += 'Threads Awaiting Reply';
			} else {
				result += 'All Threads';
			}
			if (vm.currentBlog !== '') {
				result += ' on ' + vm.currentBlog;
			}
			if (vm.filteredTag !== '') {
				result += " tagged '" + vm.filteredTag + "'";
			}
			return result;
		}
	}
}());
