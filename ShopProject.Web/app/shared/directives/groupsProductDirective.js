(function (app) {
    app.directive('groups', function ($http, $q) {
        return {
            restrict: 'E',//restraint pour les éléments du dom
            template: '<tags-input ng-model="product.Groups" display-property="Name" id="group-product" placeholder="               ---Chọn Nhóm Sản Phẩm---" replace-spaces-with-dashes="false"><auto-complete source="loadGroup($query)"  min-length="0" load-on-focus="true" load-on-empty="true"></auto-complete></tags-input>',
            scope: false,

            link: function (scope, el) {
                scope.loadGroup = function (query) {
                    var deferred = $q.defer();
                    var reqGetShops = $http({ url: 'api/group/getall', params: { 'query': query } }).then(function (result) {
                        deferred.resolve(result);
                    });
                    return deferred.promise;
                }
            }
        }
    });
})(angular.module('shopproject.common'));