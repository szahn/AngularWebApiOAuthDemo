(function() {
    'use strict';
    angular.module("security").factory("RequestBuffer",
        function($injector) {

            var $http;
            var requests = [];

            function clear() {
                requests = [];
            }

            function push(requestConfig, requestDeferred) {
                requests.push({
                    config: requestConfig,
                    deferred: requestDeferred
                });
            }

            function retryRequest(config, deferred) {
                console.log("Retrying " + config.url);
                $http(config).then(deferred.resolve, deferred.reject);
            }

            function retryAll(updateConfig) {
                $http = $http || $injector.get('$http');
                _.each(requests, function (request) {
                    retryRequest(updateConfig(request.config), request.deferred);
                });

                clear();
            }

            return {
                push: push,
                retryAll: retryAll,
                clear: clear
            };

        });
})();