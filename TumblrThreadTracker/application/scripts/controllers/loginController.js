'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('LoginController', [
    '$scope', '$location', 'sessionService', 'TrackerNotification', function ($scope, $location, sessionService, TrackerNotification) {
        $scope.login = login;
        redirectIfLoggedIn();
        $scope.setBodyClass('signin-page');

        function redirectIfLoggedIn() {
            sessionService.isLoggedIn().then(function (isLoggedIn) {
                if (isLoggedIn) {
                    $location.path('/');
                }
            });
        };

        function login() {
            $scope.loading = true;
            if ($scope.loginForm != undefined) {
                $scope.username = loginForm.username.value;
                $scope.password = loginForm.password.value;
            }
            sessionService.login($scope.username, $scope.password).then(success, fail);
        };

        function success() {
            $scope.loading = false;
            $location.path('/');
        }

        function fail() {
            $scope.loading = false;
            new TrackerNotification()
                .withMessage("Incorrect username or password.")
                .withType("error")
                .show();
        }
    }
]);