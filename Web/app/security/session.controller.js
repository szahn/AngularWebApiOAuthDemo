(function() {
    'use strict';
    angular.module("security").controller('SessionController',
        function ($scope, $state, SessionService) {

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

    });
})();
