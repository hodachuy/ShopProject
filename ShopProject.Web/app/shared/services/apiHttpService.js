/// <reference path="/assets/admin/libs/angular/angular.js" />

(function (app) {
    app.factory('apiHttpService', apiHttpService);
    apiHttpService.$inject = ['$http', 'notificationService', 'authenticationService'];
    function apiHttpService($http, notificationService, authenticationService) {
        return {
            get: get,
            post: post,
            put: put,
            del: del,
            postProduct: postProduct,
            putProduct: putProduct

        }

        function postProduct(url, data, success, failure) {
            authenticationService.setHeader();
            $http.post(url, data, {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).then(function (result) {
                success(result);
            }, function (error) {
                if (error.status === 401) {
                    notificationService.displayWarning('Unauthorized !  Đăng nhập quyền admin để thực thi!.');
                }
                else if (failure != null) {
                    failure(error);
                }
            });
        }

        function putProduct(url, data, success, failure) {
            authenticationService.setHeader();
            $http.put(url, data, {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).then(function (result) {
                success(result);
            }, function (error) {
                if (error.status === 401) {
                    notificationService.displayWarning('Unauthorized !  Đăng nhập quyền admin để thực thi!.');
                }
                else if (failure != null) {
                    failure(error);
                }
            });
        }

        function get(url, params, success, failure) {
            authenticationService.setHeader();
            $http.get(url, params).then(function (result) {
                success(result);
            }, function (error) {
                //console.log(error.status)
                if (error.status === 401) {
                    notificationService.displayWarning('Unauthorized !  Đăng nhập quyền admin để thực thi!.');
                }
                else if (failure != null) {
                    failure(error);
                }
            });
        }

        function post(url, data, success, failure) {
            authenticationService.setHeader();
            $http.post(url, data).then(function (result) {
                success(result);
            }, function (error) {
                //console.log(error.status)
                if (error.status === 401) {
                    notificationService.displayWarning('Unauthorized !  Đăng nhập quyền admin để thực thi!.');
                }
                else if (failure != null) {
                    failure(error);
                }
            });
        }

        function put(url, data, success, failure) {
            authenticationService.setHeader();
            $http.put(url, data).then(function (result) {
                success(result);
            }, function (error) {
                //console.log(error.status)
                if (error.status === 401) {
                    notificationService.displayWarning('Unauthorized !  Đăng nhập quyền admin để thực thi!.');
                }

                else if (failure != null) {
                    failure(error);
                }

            });
        }

        function del(url, data, success, failure) {
            authenticationService.setHeader();
            $http.delete(url, data).then(function (result) {
                success(result);
            }, function (error) {
                //console.log(error.status)
                if (error.status === 401) {
                    notificationService.displayWarning('Unauthorized !  Đăng nhập quyền admin để thực thi!.');
                }

                else if (failure != null) {
                    failure(error);
                }

            });
        }
    }
})(angular.module('shopproject.common'));