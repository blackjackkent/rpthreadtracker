var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('FooterController', [
    '$scope', 'sessionService', function ($scope, sessionService) {
        $scope.toggleTheme = function() {
            if (!$scope.user) {
                return;
            }
            $scope.user.UseInvertedTheme = !$scope.user.UseInvertedTheme;
            sessionService.updateUser($scope.user);
        }
    }
]);