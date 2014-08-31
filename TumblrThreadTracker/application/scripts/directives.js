'use strict';

/* Directives */


angular.module('rpThreadTracker.directives', [])
    .directive('headerNav', function() {
        return {
            restrict: 'E',
            replace: 'true',
            templateUrl: '/application/views/directives/header-nav.html'
        };
    })
    .directive('sideNav', function() {
        return {
            restrict: 'E',
            replace: 'true',
            templateUrl: '/application/views/directives/side-nav.html'
        };
    })
    .directive('footer', function () {
        return {
            restrict: 'E',
            replace: 'true',
            templateUrl: '/application/views/directives/footer.html'
        };
    });
