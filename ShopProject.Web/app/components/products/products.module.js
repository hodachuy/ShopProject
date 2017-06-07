/// <reference path="/assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopproject.products', ['shopproject.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('products', {
            url: "/products",
            parent: 'base',
            templateUrl: "app/components/products/productListView.html",
            controller: "productListController"
        }).state('add_product', {
            url: "/add_product",
            parent: 'base',
            templateUrl: "app/components/products/productAddView.html",
            controller:"productAddController"
        }).state('edit_product', {
            url: "/edit_product/:productID",
            parent: 'base',
            templateUrl: "/app/components/products/productEditView.html",
            controller: "productEditController"
        });
    }
})();