'use strict';

/* Controllers */

angular.module('rpThreadTracker.controllers', ['rpThreadTracker.services'])
  .controller('DashboardController', ['$scope', 'threadService', function($scope, threadService) {
      function updateThreads(data) {
          $scope.threads = data;
          $scope.myTurnCount = 0;
          $scope.theirTurnCount = 0;
          angular.forEach($scope.threads, function(thread) {
              $scope.myTurnCount += thread.IsMyTurn ? 1 : 0;
              $scope.theirTurnCount += thread.IsMyTurn ? 0 : 1;
          })
      }

        threadService.subscribe(updateThreads);
        threadService.getThreads();

        $scope.$on("$destroy", function () {
            threadService.unsubscribe(updateThreads);
        });
    }])
  .controller('MyCtrl2', ['$scope', function($scope) {

  }]);
