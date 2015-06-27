(function() {
    'use strict';
    angular.module("app").directive("favoritesList",
        function (FavoritesService) {

            function controller($scope) {

                $scope.fetchFavorites = function() {
                    $scope.isFetching = true;
                    return FavoritesService.getFavorites().then(function (favorites) {
                        $scope.favorites = favorites;
                    }).then(function () {
                        $scope.isFetching = false;
                    });
                }


                $scope.fetchFavorites();

            }

            return {
                controller: controller,
                templateUrl: "app/profile/favoritesList.html"
            }
        });
})();