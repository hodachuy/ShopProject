(function (app) {
    'use strict';

    app.controller('applicationUserEditController', applicationUserEditController);

    applicationUserEditController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$location', '$stateParams','$filter'];

    function applicationUserEditController($scope, apiHttpService, notificationService, $location, $stateParams, $filter) {
        
        $scope.account = {
        }

        $scope.updateAccount = updateAccount;

        function updateAccount() {
            apiHttpService.put('/api/applicationUser/update', $scope.account, addSuccessed, addFailed);
        }
        function loadDetail() {
            apiHttpService.get('/api/applicationUser/detail/' + $stateParams.id, null,
            function (result) {
                $scope.account = result.data;
                // Format string datetime to date.
                $scope.account.BirthDay = new Date($scope.account.BirthDay);
            },
            function (result) {
                notificationService.displayError(result.data);
            });
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.account.FullName + ' đã được cập nhật thành công.');

            $location.url('application_users');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }
        function loadGroups() {
            apiHttpService.get('/api/applicationGroup/getlistall',
                null,
                function (response) {
                    $scope.groups = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách nhóm.');
                });

        }

        loadGroups();
        loadDetail();
    }
})(angular.module('shopproject.application_users'));