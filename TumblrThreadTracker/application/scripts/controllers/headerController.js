'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('HeaderController', [
    '$scope', 'threadService', function($scope, threadService) {
        threadService.subscribe(showLoadingIcon);
        threadService.subscribeOnComplete(hideLoadingIcon);
        $scope.loading = false;

        function showLoadingIcon() {
            $scope.loading = true;
        }

        function hideLoadingIcon() {
            $scope.loading = false;
        }
    }
]);