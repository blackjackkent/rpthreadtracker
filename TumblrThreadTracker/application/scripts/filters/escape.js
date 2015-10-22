'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.filters.filter('escape', function () {
    return window.encodeURIComponent;
});