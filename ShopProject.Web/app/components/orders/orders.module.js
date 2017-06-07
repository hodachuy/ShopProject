/// <reference path="/assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopproject.orders', ['shopproject.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('orders', {
            url: "/orders",
            parent: 'base',
            templateUrl: "app/components/orders/orderListView.html",
            controller: "orderListController"
        }).state('view_order', {
            url: "/view_order/:orderID",
            parent: 'base',
            templateUrl: "/app/components/orders/orderView.html",
            controller: "orderViewController"
        });
    }
})();