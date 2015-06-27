(function() {
    'use strict';
    angular.module("security").factory('SessionInterceptorService',
        function ($q, $location, $injector, RequestBuffer) {

            var SessionService;

            function onRequest(config) {
                SessionService = SessionService || $injector.get('SessionService');
                var token = SessionService.getToken();
                if (token !== "") {
                    config.headers = config.headers || {};
                    config.headers.Authorization = 'Bearer ' + token;
                }

                return config;
            }

            function authenticationRequired() {
                SessionService = SessionService || $injector.get('SessionService');
                SessionService.refreshAuthToken();
            }

            var throttledAuthRequired = _.debounce(authenticationRequired, 900);

            function onUnauthorized(rejection) {
                var deferred = $q.defer();
                RequestBuffer.push(rejection.config, deferred);
                throttledAuthRequired();
                return deferred.promise;
            }

            function onResponseError(rejection) {
                if (rejection.status === 401) {
                    return onUnauthorized(rejection);
                } else {
                    return $q.reject(rejection);
                }
            }

            return {
                request: onRequest,
                responseError: onResponseError
            };
    });
})();