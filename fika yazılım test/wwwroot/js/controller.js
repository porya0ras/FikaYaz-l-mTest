(function () {
   'use strict';

    angular
        .module('app.controllers', [])
        .controller('HomeCtrl', [
            '$scope',
            '$http',
            '$window',
            '$timeout',
            function ($scope, $http, $window, $timeout) {
                $scope.init = function () {
                    console.log("test");
                };
            }]);
})();
