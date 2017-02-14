'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('BaseController',
		[
			'sessionService', 'SESSION_EVENTS', 'cacheBuster', 'BodyClass', '$timeout',
			'adminflareService', baseController
		]);

	/** @this baseController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len
	function baseController(sessionService, SESSION_EVENTS, cacheBuster, BodyClass, $timeout, adminflareService) {
		var vm = this;
		vm.cacheBuster = cacheBuster;
		vm.bodyClass = BodyClass;
		$timeout(adminflareService.init);
		$timeout(adminflareService.initCustom);

		sessionService.subscribe(handleSessionEvent);
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
	}
}());
