'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.directives.directive('headerNav', function() {
    return {
        restrict: 'E',
        replace: 'true',
        templateUrl: '/application/views/directives/header-nav.html',
        controller: 'HeaderController'
    };
});