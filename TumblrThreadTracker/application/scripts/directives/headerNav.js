(function() {
    "use strict";
    angular.module("rpthreadtracker")
        .directive("headerNav",
        [
            "cacheBuster", headerNav
        ]);

    function headerNav(cacheBuster) {
        return {
            restrict: "E",
            replace: "true",
			scope: {
				publicView: '@',
				pageId: '@',
				blogs: '=',
				user: '='
			},
            templateUrl: "/application/views/directives/header-nav.html?cacheBuster=" + cacheBuster,
            controller: "HeaderController",
            controllerAs: "vm",
			bindToController: true
        };
    }
})();