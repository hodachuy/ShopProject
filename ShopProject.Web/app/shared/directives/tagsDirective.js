(function (app) {
    app.directive('tags', function ($http, $q) {
        return {
            restrict: 'E',//restraint pour les éléments du dom
            template: '<tags-input ng-model="product.TagsVm" display-property="Name" placeholder="Thêm Nhãn" replace-spaces-with-dashes="false"></tags-input>',
            scope: false,

            //link: function (scope, el) {
            //    scope.loadTags = function (query) {
            //        var deferred = $q.defer();
            //        var reqGetShops = $http({ url: 'api/product/getalltag', params: { 'query': query },cache:true }).then(function (result) {

            //            deferred.resolve(result);
            //        });
            //        return deferred.promise;
            //    }

            //}
        }
    });
})(angular.module('shopproject.common'));