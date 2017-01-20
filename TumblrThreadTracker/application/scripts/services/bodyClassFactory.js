(function() {
	'use strict';
	angular.module('rpthreadtracker').factory('BodyClass', bodyClassFactory);
	function bodyClassFactory() {
		var bodyClass = '';
		return {
			value: function () { return bodyClass; },
			set: function (newBodyClass) { bodyClass = newBodyClass; }
		};
	};
})();