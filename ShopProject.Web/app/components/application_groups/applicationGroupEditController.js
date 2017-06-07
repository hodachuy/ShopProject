(function (app) {
    'use strict';

    app.controller('applicationGroupEditController', applicationGroupEditController);

    applicationGroupEditController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$location', '$stateParams'];

    function applicationGroupEditController($scope, apiHttpService, notificationService, $location, $stateParams) {
        $scope.group = {}


        $scope.updateApplicationGroup = updateApplicationGroup;

        function updateApplicationGroup() {
            apiHttpService.put('/api/applicationGroup/update', $scope.group, addSuccessed, addFailed);
        }
        function loadDetail() {
            apiHttpService.get('/api/applicationGroup/detail/' + $stateParams.id, null,
            function (result) {
                $scope.group = result.data;
            },
            function (result) {
                notificationService.displayError(result.data);
            });
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.group.Name + ' đã được cập nhật thành công.');

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
        loadDetail();
    }
})(angular.module('shopproject.application_groups'));