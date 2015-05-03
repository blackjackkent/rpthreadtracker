'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.directives.directive('sideNav', ['cacheBuster', function(cacheBuster) {
    return {
        restrict: 'E',
        replace: 'true',
        templateUrl: '/application/views/directives/side-nav.html?cacheBuster=' + cacheBuster
    };
}]);