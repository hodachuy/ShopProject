(function (app) {
    app.controller('homeController', homeController);
    homeController.$inject = ['apiHttpService'];
    function homeController(apiHttpService) {
        apiHttpService.get('api/Home/TestMethod', null, function (response) {
        });
    }
})(angular.module('shopproject'));