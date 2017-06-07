(function (app) {
    app.controller('groupEditController', groupEditController);
    groupEditController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$state', '$stateParams', 'commonService'];
    function groupEditController($scope, apiHttpService, notificationService, $state, $stateParams, commonService) {
        $scope.groupProduct = [];
        $scope.EditGroup = EditGroup;

        // Change text in Textbox Seo with data Textbox Name.
        $scope.GetSeoTitle = GetSeoTitle;
        function GetSeoTitle() {
            $scope.groupProduct.Alias = commonService.getSeoTitle($scope.groupProduct.Name);
        }
        // Event destroy submit form when enter on textbox.
        $scope.keyPressed = function (event) {
            if (event.which === 13) {
                event.preventDefault();
            }
        }
        //// Single Image
        //$scope.imageUpload = function (event) {
        //    var files = event.target.files; //FileList object

        //    for (var i = 0; i < files.length; i++) {
        //        var file = files[i];

        //        var reader = new FileReader();
        //        reader.onload = function (e) {
        //            $scope.$apply(function () {
        //                $scope.groupProduct.Image = e.target.result;
        //                $scope.FileName = e.target.Name;
        //            });
        //            $(".pip").show();
        //        };
        //        reader.readAsDataURL(file);
        //    }
        //}
        //$(document).ready(function () {
        //    $(".remove").click(function () {
        //        $(this).parent(".pip").remove();
        //        var control = $("#files");
        //        control.replaceWith(control.val('').clone(true));
        //        $scope.groupProduct.Image = null;
        //    });
        //});

        // Single Image
        var maxSize = 1024;
        $scope.imageUpload = function (event) {
            var files = event.target.files; //FileList object
            for (var i = 0; i < files.length; i++) {
                var file = files[0];
                var reader = new FileReader();
                if (file.type.indexOf("image") == 0) {
                    reader.onload = function (e) {
                        if ((e.total / 1024).toFixed(0) > maxSize) {
                            notificationService.displayWarning("Kích thước hình ảnh lớn hơn 1MB.");
                        }
                        else {
                            $scope.$apply(function () {
                                $scope.groupProduct.Image = e.target.result;
                                $scope.FileName = e.target.Name;
                            });
                            $(".pip").show();
                        }
                    };
                }
                else {
                    notificationService.displayWarning("Vui lòng cung cấp file hình ảnh.");
                }

                reader.readAsDataURL(file);
            }
        }
        $(document).ready(function () {
            $(".remove").click(function () {
                $('imageThumb').attr('src', $scope.groupProduct.Image) == null;
                var control = $("#filesImage");
                control.replaceWith(control.val('').clone(true));
                $scope.groupProduct.Image = null;
                $(".pip").hide();
            });
        });


        // Cau hinh url:
        // service $stateParams de lay id groupProductID cua url.
        // 'api/groupProduct/getbyid/' + $stateParams.groupProductID
        // cau hinh thong tin tren dung voi ben back end web api
        // [Route("getbyid/{groupProductID:int}")]
        // Ngoai ra cau hinh params o module.js o cong state url: "/edit_product_category/:groupProductID",
        // du'ng voi uirouter o view page ui-rsef =  "edit_product_category({groupProductID:item.ID})"
        function GetGroupProductbyID() {
            apiHttpService.get('api/group/getbyid/' + $stateParams.groupID, null, function (result) {
                $scope.groupProduct = result.data;
                if ($scope.groupProduct.Image === null || $scope.groupProduct.Image == "") {
                    $(".pip").hide();
                }
            }, function (error) {
                notificationService.displayError('Load data fail.');
            });
        }

        function EditGroup() {
            apiHttpService.put('api/group/update', $scope.groupProduct, function (result) {
                notificationService.displaySuccess(result.data.Name + ' cập nhập thành công');
                $state.go('groups');
            }, function (error) {
                notificationService.displayError(error);
            });
        }

        GetGroupProductbyID();
    }

})(angular.module('shopproject.groups'));