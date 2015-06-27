(function() {
    'use strict';
    angular.module("security").factory('SessionService',
        function ($http, $q, $state, RequestBuffer) {

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

    });
})();
