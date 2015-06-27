(function() {
    'use strict';
    angular.module("app", ["security", 'ui.router'])
        .config(["$locationProvider", function ($locationProvider) {
            $locationProvider.html5Mode(true);
        }])
        .config(["$stateProvider", function ($stateProvider) {
            $stateProvider.state('profile', {
                url: "/",
                templateUrl: "app/profile/index.html",
                controller: "ProfileController",
                resolve: {
                    authenticated: ["AuthResolver", function (AuthResolver) {
                        return AuthResolver.resolve();
                    }]
                }
            });


        }]);
})();
(function () {
    'use strict';
    angular.module("security", ['ui.router'])
        .run(["$rootScope", "$location", "$state", "$stateParams", function($rootScope, $location, $state, $stateParams) {
            $rootScope.$on('$stateChangeError', function(err, req) {
                console.log("State change error on " + req.controller + " url " + req.url);
                $state.go('login');
            });
        }])
        .config(["$httpProvider", function($httpProvider) {
            $httpProvider.interceptors.push('SessionInterceptorService');
        }])
        .config(["$stateProvider", "$urlRouterProvider", function($stateProvider, $urlRouterProvider) {

            $stateProvider.state('login', {
                url: "/login",
                templateUrl: "app/security/login.html",
                controller: "SessionController"
            });

            $urlRouterProvider.when('', '/profile');

        }]);
})();
(function() {
    'use strict';
    angular.module("app").directive("favoritesList",
        ["FavoritesService", function (FavoritesService) {

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
            controller.$inject = ["$scope"];

            return {
                controller: controller,
                templateUrl: "app/profile/favoritesList.html"
            }
        }]);
})();
(function() {
    'use strict';
    angular.module("app").factory("FavoritesService",
        ["$http", function($http) {

            function getFavorites() {
                return $http.get("/api/favorites").then(function(results) {
                    return results.data;
                });

            }

            return {
                getFavorites: getFavorites
            }
        }]);
})();
(function() {
    'use strict';
    angular.module("app").controller("ProfileController",
        ["$scope", "SessionService", "FavoritesService", function ($scope, SessionService, FavoritesService) {

            $scope.logout = function() {
                SessionService.logOut();
            }

        }]);
})();
(function() {
    'use strict';
    angular.module("app").directive("profile",
        ["SessionService", function (SessionService) {

            function controller($scope) {
                $scope.userName = SessionService.getUserName();
            }
            controller.$inject = ["$scope"];

            return {
                controller: controller,
                templateUrl: "app/profile/profile.html"
            };
        }]);
})();
(function() {
    'use strict';
    angular.module('security').factory('AuthResolver',
        ["$q", "SessionService", function ($q, SessionService) {
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
        }]);
})();
(function() {
    'use strict';
    angular.module("security").factory("RequestBuffer",
        ["$injector", function($injector) {

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

        }]);
})();
(function() {
    'use strict';
    angular.module("security").controller('SessionController',
        ["$scope", "$state", "SessionService", function ($scope, $state, SessionService) {

            $scope.credentials = {
                userName: "",
                password: "",
                clientId: "123"
            };

            $scope.message = "";

            $scope.loginClicked = function () {
                SessionService.login($scope.credentials)
                .then(function (response) {
                    $state.go('profile');
                }, function (err) {
                    $scope.message = (err && err.error_description) ? err.error_description : "Failed to login due to unknown error.";
                });
            };

    }]);
})();

(function() {
    'use strict';
    angular.module("security").factory('SessionService',
        ["$http", "$q", "$state", "RequestBuffer", function ($http, $q, $state, RequestBuffer) {

            var sessionStorageId = "session";
            var tokenUrl = '/api/token';

            function getAuthSession() {
                return JSON.parse(localStorage.getItem(sessionStorageId));
            }

            function setAuthSession(authSession) {
                localStorage.setItem(sessionStorageId, JSON.stringify(authSession));
            }

            function clearAuthSession() {
                localStorage.removeItem(sessionStorageId);
            }

            function isLoggedIn() {
                var authData = getAuthSession();
                return authData &&
                    authData.userName &&
                    authData.token &&
                    authData.clientId;

            }

            function getToken() {
                var authData = getAuthSession();
                return authData ? authData.token : "";
            }

            function getUserName () {
                var authData = getAuthSession();
                return authData ? authData.userName : "Guest";
            }

            function logOut () {
                clearAuthSession();
                $state.go('login');
            }

            function refresh() {
                var session = getAuthSession();

                var requestBody = ["grant_type=refresh_token",
                    "refresh_token=" + session.token,
                    "client_id=" + session.clientId].join('&');

                var loginRequest = { headers: { "Content-Type": 'application/x-www-form-urlencoded' } };

                return $http.post(tokenUrl, requestBody, loginRequest).success(function (response) {
                    console.log("New access token is " + response.access_token);
                    session.token = response.access_token;
                    setAuthSession(session);
                    return session;
                }).error(function (err, status) {
                    logOut();
                    throw err;
                });
            }

            function refreshAuthToken() {
                refresh().then(function(session) {
                    RequestBuffer.retryAll(function(config) {
                        config.headers.Authorization = 'Bearer ' + session.token;
                        return config;
                    });
                });
            }

            function login(credentials) {

                var requestBody = ["grant_type=password",
                    "username=" + credentials.userName,
                    "password=" + credentials.password,
                    "client_id=" + credentials.clientId].join('&');

                var loginRequest = { headers: { "Content-Type": 'application/x-www-form-urlencoded' } };

                return $http.post(tokenUrl, requestBody, loginRequest).success(function (response) {
                    console.log("Access token is " + response.access_token);
                    setAuthSession({
                        token: response.access_token,
                        userName: credentials.userName,
                        clientId: credentials.clientId
                    });

                }).error(function (err, status) {
                    logOut();
                    throw err;
                });
            }

            return {
                login: login,
                logOut: logOut,
                getToken: getToken,
                getUserName: getUserName,
                isLoggedIn: isLoggedIn,
                refreshAuthToken: refreshAuthToken
            };

    }]);
})();

(function() {
    'use strict';
    angular.module("security").factory('SessionInterceptorService',
        ["$q", "$location", "$injector", "RequestBuffer", function ($q, $location, $injector, RequestBuffer) {

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
    }]);
})();