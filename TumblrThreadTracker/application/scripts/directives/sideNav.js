'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.directives.directive('sideNav', function() {
    return {
        restrict: 'E',
        replace: 'true',
        templateUrl: '/application/views/directives/side-nav.html'
    };
});