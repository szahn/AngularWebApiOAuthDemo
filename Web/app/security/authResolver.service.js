(function() {
    'use strict';
    angular.module('security').factory('AuthResolver',
        function ($q, SessionService) {
            function resolve() {
                var deferred = $q.defer();
                if (SessionService.isLoggedIn()) {
                    deferred.resolve();
                } else {
                    console.warn("User is not logged in");
                    deferred.reject('Not logged in');
                }

                return deferred.promise;
            }

            return {
                resolve: resolve
            };
        });
})();