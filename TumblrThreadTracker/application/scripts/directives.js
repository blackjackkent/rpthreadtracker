'use strict';

/* Directives */


angular.module('rpThreadTracker.directives', [])
    .directive('headerNav', function() {
        return {
            restrict: 'E',
            replace: 'true',
            templateUrl: '/application/views/directives/header-nav.html',
            controller: 'HeaderController'
        };
    })
    .directive('sideNav', function() {
        return {
            restrict: 'E',
            replace: 'true',
            templateUrl: '/application/views/directives/side-nav.html'
        };
    })
    .directive('footer', function() {
        return {
            restrict: 'E',
            replace: 'true',
            templateUrl: '/application/views/directives/footer.html'
        };
    })
    .directive('ngFocus', [
        function() {
            var focusClass = "ng-focused",
                applyFocus = function(scope, element, attrs, ctrl) {
                    ctrl.$focused = false;
                    element.bind('focus', function(evt) {
                        element.addClass(focusClass);
                        scope.$apply(function() {
                            ctrl.$focused = true;
                        });
                    }).bind('blur', function(evt) {
                        element.removeClass(focusClass);
                        scope.$apply(function() {
                            ctrl.$focused = false;
                        });
                    });
                };
            return {
                restrict: 'A',
                require: 'ngModel',
                link: applyFocus
            };
        }
    ]);
