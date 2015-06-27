(function () {
    'use strict';
    angular.module("security", ['ui.router'])
        .run(function($rootScope, $location, $state, $stateParams) {
            $rootScope.$on('$stateChangeError', function(err, req) {
                console.log("State change error on " + req.controller + " url " + req.url);
                $state.go('login');
            });
        })
        .config(function($httpProvider) {
            $httpProvider.interceptors.push('SessionInterceptorService');
        })
        .config(function($stateProvider, $urlRouterProvider) {

            $stateProvider.state('login', {
                url: "/login",
                templateUrl: "app/security/login.html",
                controller: "SessionController"
            });

            $urlRouterProvider.when('', '/profile');

        });
})();