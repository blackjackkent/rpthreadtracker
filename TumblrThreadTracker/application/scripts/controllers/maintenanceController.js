'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('StaticController', [
    '$scope', 'sessionService', 'pageId', function($scope, sessionService, pageId) {
        $scope.setBodyClass('');
        $scope.pageId = pageId;
        $scope.publicView = true;
    }
]);