'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.directives.directive('footer', ['cacheBuster', function(cacheBuster) {
    return {
        restrict: 'E',
        replace: 'true',
        templateUrl: '/application/views/directives/footer.html?cacheBuster=' + cacheBuster
    };
}]);