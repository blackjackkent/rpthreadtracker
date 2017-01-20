(function() {
    "use strict";
    angular.module("rpthreadtracker")
        .controller("BaseController",
        [
            "sessionService", "SESSION_EVENTS", "cacheBuster", 'BodyClass',
            baseController
        ]);

    function baseController(sessionService, SESSION_EVENTS, cacheBuster, BodyClass) {
	    var vm = this;
	    vm.cacheBuster = cacheBuster;
	    vm.bodyClass = BodyClass;

        sessionService.subscribe(handleSessionEvent);
        registerSession();

        function registerSession() {
            sessionService.isLoggedIn()
                .then(function(isLoggedIn) {
                    if (isLoggedIn) {
                        sessionService.getUser()
                            .then(function(user) {
                                vm.userId = user.UserId;
                                vm.user = user;
                            });
                    }
                });
        }

        function clearSession() {
            vm.userId = null;
            vm.user = null;
        }

        function handleSessionEvent(eventType) {
            if (eventType === SESSION_EVENTS.LOGIN) {
                registerSession();
            }
            if (eventType === SESSION_EVENTS.LOGOUT) {
                clearSession();
            }
        }

        function setBodyClass(_bodyClass) {
        	vm.bodyClass = _bodyClass;
        }
    }
})();