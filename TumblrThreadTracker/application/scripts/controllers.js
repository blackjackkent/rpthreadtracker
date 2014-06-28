'use strict';

/* Controllers */

angular.module('rpThreadTracker.controllers', ['rpThreadTracker.services'])
  .controller('DashboardController', ['$scope', 'threadService', function($scope, threadService) {
        threadService.getThreadIds().then(function(data) {
            $scope.threadIds = data;
            return data;
        })
    }])
  .controller('MyCtrl2', ['$scope', function($scope) {

  }]);
