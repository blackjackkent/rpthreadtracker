'use strict';

/* Controllers */

angular.module('rpThreadTracker.controllers', ['rpThreadTracker.services'])
  .controller('DashboardController', ['$scope', 'threadService', function($scope, threadService) {
      function updateThreads(data) {
          $scope.threads = data;
          console.log(data);
      }

        threadService.subscribe(updateThreads);
        threadService.getThreads();

        $scope.$on("$destroy", function () {
            threadService.unsubscribe(updateThreads);
        });
    }])
  .controller('MyCtrl2', ['$scope', function($scope) {

  }]);
