(function() {
    "use strict";
    angular.module("rpthreadtracker").filter("escape", escape);

    function escape() {
        return window.encodeURIComponent;
    }
})();