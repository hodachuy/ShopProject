(function (app) {
    'use strict';

    app.controller('applicationGroupAddController', applicationGroupAddController);

    applicationGroupAddController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$location', 'commonService'];

    function applicationGroupAddController($scope, apiHttpService, notificationService, $location, commonService) {
        $scope.group = {
            ID: 0,
            Roles: []
        }

        $scope.addAppGroup = addApplicationGroup;

        function addApplicationGroup() {
            apiHttpService.post('/api/applicationGroup/add', $scope.group, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.group.Name + ' đã được thêm mới.');

            $location.url('application_groups');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }
        function loadRoles() {
            apiHttpService.get('/api/applicationRole/getlistall',
                null,
                function (response) {
                    $scope.roles = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách quyền.');
                });

        }

        loadRoles();

    }
})(angular.module('shopproject.application_groups'));