(function() {
    'use strict';
    angular.module("app", ["security", 'ui.router'])
        .config(function ($locationProvider) {
            $locationProvider.html5Mode(true);
        })
        .config(function ($stateProvider) {
            $stateProvider.state('profile', {
                url: "/",
                templateUrl: "app/profile/index.html",
                controller: "ProfileController",
                resolve: {
                    authenticated: function (AuthResolver) {
                        return AuthResolver.resolve();
                    }
                }
            });


        });
})();