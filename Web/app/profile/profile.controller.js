(function() {
    'use strict';
    angular.module("app").controller("ProfileController",
        function ($scope, SessionService, FavoritesService) {

            $scope.logout = function() {
                SessionService.logOut();
            }

        });
})();