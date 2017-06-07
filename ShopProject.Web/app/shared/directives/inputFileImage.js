(function (app) {
    'use strict';
    app.directive('inputFiles', ['$parse', function ($parse) {
        function fn_link(scope, element, attrs) {

            var onchange = $parse(attrs.inputFiles);
            element.on('change', function (event) {
                onchange(scope, { $files: event.target.files });
            })
        }
        return {
            link: fn_link

        }

    }]);
})(angular.module('shopproject.common'));
