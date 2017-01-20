(function() {
    'use strict';
    angular.module('rpthreadtracker')
        .directive('footer',
        [
            'cacheBuster', footer
        ]);

    function footer(cacheBuster) {
        return {
            restrict: 'E',
            replace: 'true',
            scope: {
            	user: '='
            },
            templateUrl: '/application/views/directives/footer.html?cacheBuster=' + cacheBuster,
            controller: 'FooterController',
        	controllerAs: 'vm',
			bindToController: true
        };
    }
})();