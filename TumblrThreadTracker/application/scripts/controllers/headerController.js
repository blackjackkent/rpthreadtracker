var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('HeaderController', [
    '$scope', '$timeout', 'threadService', 'adminflareService', function ($scope, $timeout, threadService, adminflareService) {
        $timeout(adminflareService.init);
        $timeout(adminflareService.initCustom);
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