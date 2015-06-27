(function() {
    'use strict';
    angular.module("app").directive("profile",
        function (SessionService) {

            function controller($scope) {
                $scope.userName = SessionService.getUserName();
            }

            return {
                controller: controller,
                templateUrl: "app/profile/profile.html"
            };
        });
})();