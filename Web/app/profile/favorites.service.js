(function() {
    'use strict';
    angular.module("app").factory("FavoritesService",
        function($http) {

            function getFavorites() {
                return $http.get("/api/favorites").then(function(results) {
                    return results.data;
                });

            }

            return {
                getFavorites: getFavorites
            }
        });
})();