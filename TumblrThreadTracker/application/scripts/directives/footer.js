'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.directives.directive('footer', function() {
    return {
        restrict: 'E',
        replace: 'true',
        templateUrl: '/application/views/directives/footer.html'
    };
});