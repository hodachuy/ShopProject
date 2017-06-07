(function (app) {
    'use strict';

    app.controller('applicationUserAddController', applicationUserAddController);

    applicationUserAddController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$location', 'commonService'];

    function applicationUserAddController($scope, apiHttpService, notificationService, $location, commonService) {
        $scope.account = {
            Groups: []
        }

        $scope.addAccount = addAccount;

        function addAccount() {
            apiHttpService.post('/api/applicationUser/add', $scope.account, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.account.Name + ' đã được thêm mới.');

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

    }
})(angular.module('shopproject.application_users'));