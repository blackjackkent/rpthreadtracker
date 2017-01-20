(function() {
    "use strict";
    angular.module("rpthreadtracker").directive("ngFocus", ngFocus);

    function ngFocus() {
        var focusClass = "ng-focused",
            applyFocus = function(scope, element, attrs, ctrl) {
                ctrl.$focused = false;
                element.bind("focus",
                        function(evt) {
                            element.addClass(focusClass);
                            scope.$apply(function() {
                                ctrl.$focused = true;
                            });
                        })
                    .bind("blur",
                        function(evt) {
                            element.removeClass(focusClass);
                            scope.$apply(function() {
                                ctrl.$focused = false;
                            });
                        });
            };
        return {
            restrict: "A",
            require: "ngModel",
            link: applyFocus
        };
    }
})();