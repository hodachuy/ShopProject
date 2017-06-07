/// <reference path="/assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopproject.slides', ['shopproject.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('slides', {
                url: "/slides",
                parent: 'base',
                templateUrl: "/app/components/slides/slideListView.html",
                controller: "slideListController"
            }).state('add_slide', {
                url: "/add_slide",
                parent: 'base',
                templateUrl: "/app/components/slides/slideAddView.html",
                controller: "slideAddController"
            }).state('edit_slide', {
                url: "/edit_slide/:slideID",
                parent: 'base',
                templateUrl: "/app/components/slides/slideEditView.html",
                controller: "slideEditController"
            });
    }
})();