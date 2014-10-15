'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('BodyController', [
    '$scope', function($scope) {
        $scope.setBodyClass = function(_bodyClass) {
            $scope.bodyClass = _bodyClass;
        };
    }
]);