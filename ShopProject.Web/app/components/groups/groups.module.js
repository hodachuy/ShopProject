/// <reference path="/assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopproject.groups', ['shopproject.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('groups', {
                url: "/groups",
                parent: 'base',
                templateUrl: "/app/components/groups/groupListView.html",
                controller: "groupListController"
            }).state('add_group', {
                url: "/add_group",
                parent: 'base',
                templateUrl: "/app/components/groups/groupAddView.html",
                controller: "groupAddController"
            }).state('edit_group', {
                url: "/edit_group/:groupID",
                parent: 'base',
                templateUrl: "/app/components/groups/groupEditView.html",
                controller: "groupEditController"
            });
    }
})();