'use strict';

// Declare app level module which depends on filters, and services
angular.module('rpThreadTracker', [
  'ngRoute',
  'rpThreadTracker.filters',
  'rpThreadTracker.services',
  'rpThreadTracker.directives',
  'rpThreadTracker.controllers'
]).
config(['$routeProvider', function($routeProvider) {
  $routeProvider.when('/', {templateUrl: '/application/views/dashboard.html', controller: 'DashboardController'});
  $routeProvider.otherwise({ redirectTo: '/' });

    // use the HTML5 History API
}]);
