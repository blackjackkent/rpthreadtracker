﻿'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.directives.directive('headerNav', ['cacheBuster', function(cacheBuster) {
    return {
        restrict: 'E',
        replace: 'true',
        templateUrl: '/application/views/directives/header-nav.html?cacheBuster=' + cacheBuster,
        controller: 'HeaderController'
    };
}]);