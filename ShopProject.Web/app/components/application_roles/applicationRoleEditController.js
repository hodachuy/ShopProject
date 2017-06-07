(function (app) {
    'use strict';

    app.controller('applicationRoleEditController', applicationRoleEditController);

    applicationRoleEditController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$location', '$stateParams'];

    function applicationRoleEditController($scope, apiHttpService, notificationService, $location, $stateParams) {
        $scope.role = {}


        $scope.updateApplicationRole = updateApplicationRole;

        function updateApplicationRole() {
            apiHttpService.put('/api/applicationRole/update', $scope.role, addSuccessed, addFailed);
        }
        function loadDetail() {
            apiHttpService.get('/api/applicationRole/detail/' + $stateParams.id, null,
            function (result) {
                $scope.role = result.data;
            },
            function (result) {
                notificationService.displayError(result.data);
            });
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.role.Name + ' đã được cập nhật thành công.');

            $location.url('application_roles');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }
        loadDetail();
    }
})(angular.module('shopproject.application_roles'));