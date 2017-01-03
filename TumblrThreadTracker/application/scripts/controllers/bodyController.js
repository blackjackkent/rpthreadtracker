'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('BodyController', [
    '$scope', function($scope) {
        $scope.setBodyClass = setBodyClass;

        function setBodyClass(_bodyClass) {
            $scope.bodyClass = _bodyClass;
        };
    }
]);