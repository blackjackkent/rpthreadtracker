(function() {
    'use strict';
    angular.module('rpthreadtracker')
        .directive('sideNav',
        [
            'cacheBuster', sideNav
        ]);
    function sideNav(cacheBuster) {
            return {
                restrict: 'E',
                replace: 'true',
                scope: {
                	publicView: '@',
					pageId: '@'
                },
                templateUrl: '/application/views/directives/side-nav.html?cacheBuster=' + cacheBuster,
                controller: "SideNavController",
                controllerAs: "vm",
                bindToController: true
            };
        }
})();