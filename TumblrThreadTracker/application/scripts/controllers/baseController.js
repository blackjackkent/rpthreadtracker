'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('BaseController', [
    '$scope', 'sessionService', 'SESSION_EVENTS', 'cacheBuster', function ($scope, sessionService, SESSION_EVENTS, cacheBuster) {
        $scope.cacheBuster = cacheBuster;
        sessionService.subscribe(handleSessionEvent);
        registerSession();

        function registerSession() {
            sessionService.isLoggedIn().then(function(isLoggedIn) {
                if (isLoggedIn) {
                    sessionService.getUser().then(function(user) {
                        $scope.userId = user.UserId;
                        $scope.user = user;
                    });
                }
            });
        }

        function clearSession() {
            $scope.userId = null;
            $scope.user = null;
        }

        function handleSessionEvent(eventType) {
            if (eventType === SESSION_EVENTS.LOGIN) {
                registerSession();
            }
            if (eventType === SESSION_EVENTS.LOGOUT) {
                clearSession();
            }
        }
    }
]);