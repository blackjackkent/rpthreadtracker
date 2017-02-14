'use strict';
(function() {
	angular.module('rpthreadtracker').directive('ngFocus', ngFocus);

	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function ngFocus() {
		var focusClass = 'ng-focused';
		// eslint-disable-next-line max-params,
		function applyFocus(scope, element, attrs, ctrl) {
			ctrl.$focused = false;
			element.bind('focus', function() {
				element.addClass(focusClass);
				scope.$apply(function() {
					ctrl.$focused = true;
				});
			}).bind('blur', function() {
				element.removeClass(focusClass);
				scope.$apply(function() {
					ctrl.$focused = false;
				});
			});
		}
		return {
			'restrict': 'A',
			'require': 'ngModel',
			'link': applyFocus
		};
	}
}());
