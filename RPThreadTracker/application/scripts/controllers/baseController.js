'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('BaseController',
		[
			'$scope', 'sessionService', 'SESSION_EVENTS', 'cacheBuster', 'BodyClass', '$timeout',
			'adminflareService', baseController
		]);

	/** @this baseController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function baseController($scope, sessionService, SESSION_EVENTS, cacheBuster, BodyClass, $timeout, adminflareService) {
		var vm = this;
		vm.cacheBuster = cacheBuster;
		vm.bodyClass = BodyClass;
		$timeout(adminflareService.init);
		$timeout(adminflareService.initCustom);
		$scope.$on('$destroy', destroyView);

		sessionService.subscribeLoginLogoutEvent(handleSessionEvent);
		sessionService.subscribeUpdateUserEvent(handleUpdateUserEvent);
		registerSession();

		function registerSession() {
			sessionService.isLoggedIn().then(function(isLoggedIn) {
				if (isLoggedIn) {
					sessionService.getUser().then(function(user) {
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

		function handleUpdateUserEvent() {
			sessionService.getUser().then(function(user) {
				if (!user) {
					vm.user = null;
					return;
				}
				vm.userId = user.UserId;
				vm.user = user;
			});
		}

		function destroyView() {
			sessionService.unsubscribeLoginLogoutEvent(handleSessionEvent);
			sessionService.unsubscribeUpdateUserEvent(handleUpdateUserEvent);
		}
	}
}());
