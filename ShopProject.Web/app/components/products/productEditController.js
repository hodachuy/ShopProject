/// <reference path="ProductAddController.js" />
(function (app) {
    app.controller('productEditController', productEditController);

    productEditController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$state', '$stateParams', 'commonService', '$ngBootbox'];
    function productEditController($scope, apiHttpService, notificationService, $state, $stateParams, commonService, $ngBootbox) {
        $scope.EditProduct = EditProduct;
        $scope.DeleteImage = DeleteImage;
        $scope.GetMultiImage = GetMultiImage;
        $scope.GetproductbyID = GetproductbyID;
        $scope.ProductCategory = [];
        $scope.product = {
            Status: true,
            MoreImages: []
        }
        $scope.DataImage = [];
        $scope.AllTag = [];
        $scope.product.TagsVm = [];
        $scope.LoadAllTag = LoadAllTag;

        // Event destroy submit form when enter on textbox.
        $scope.keyPressed = function (event) {
            if (event.which === 13) {
                event.preventDefault();
            }
        }

        // Not allow input to text.
        $('#group-product').keydown(function () {
            //code to not allow any changes to be made to input field
            return false;
        });

        // Setting tinymce.
        $scope.tinymceOptions = {
            onChange: function (e) {
                // put logic here for keypress and cut/paste changes
            },
            // performance
            cleanup: true,
            verify_html: false,
            entity_encoding: "raw",
            cleanup_on_startup: true,
            // ket thuc performance
            height: '200px',
            inline: false,
            plugins: 'advlist autolink link image lists print preview code media table textcolor hr searchreplace wordcount ',
            //toolbar: "forecolor backcolor table image autolink preview charmap link searchreplace fontsizeselect fontselect ",
            toolbar: "fontselect | bold italic underline | forecolor | fontsizeselect | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | image searchreplace ",
            fontsize_formats: "8pt 10pt 12pt 13pt 14pt 18pt 24pt 36pt",
            skin: 'lightgray',
            theme: 'modern',
            languague: 'vi',
            valid_elements: '+*[*]',
            file_browser_callback: RoxyFileBrowser,

            media_url_resolver: function (data, resolve/*, reject*/) {
                if (data.url.indexOf('YOUR_SPECIAL_VIDEO_URL') !== -1) {
                    var embedHtml = '<iframe src="' + data.url +
                    '" width="400" height="400" ></iframe>';
                    resolve({ html: embedHtml });
                } else {
                    resolve({ html: '' });
                }
            }
        }

        // Manage file with roxy fileman in tinymce.
        $scope.RoxyFileBrowser = RoxyFileBrowser;
        var roxyFileman = '/fileman/index.html?integration=tinymce4';
        function RoxyFileBrowser(field_name, url, type, win) {
            var cmsURL = roxyFileman;  // script URL - use an absolute path!
            if (cmsURL.indexOf("?") < 0) {
                cmsURL = cmsURL + "?type=" + type;
            }
            else {
                cmsURL = cmsURL + "&type=" + type;
            }
            cmsURL += '&input=' + field_name + '&value=' + win.document.getElementById(field_name).value;
            tinyMCE.activeEditor.windowManager.open({
                file: cmsURL,
                title: 'Insert File',
                width: 850, // Your dimensions may differ - toy around with them!
                height: 550,
                resizable: "yes",
                plugins: "media",
                inline: "yes", // This parameter only has an effect if you use the inlinepopups plugin!
                close_previous: "no"
            }, {
                window: win,
                input: field_name
            });
            return false;
        }

        // Change text in Textbox Seo with data Textbox Name.
        $scope.GetSeoTitle = GetSeoTitle;
        function GetSeoTitle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
        }

        // Single Image.
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
                                $scope.product.Image = e.target.result;
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
                $('imageThumb').attr('src', $scope.product.Image) == null;
                var control = $("#filesImage");
                control.replaceWith(control.val('').clone(true));
                $scope.product.Image = null;
                $(".pip").hide();
            });
        });

        // Multi Image.
        $scope.imgsrc = [];
        var data = new FormData();
        var maxSize = 1024;
        $scope.GetFilesInput = function ($files) {
            for (var i = 0; i < $files.length; i++) {
                var reader = new FileReader();
                reader.fileName = $files[i].name;
                if ($files[i].type.indexOf("image") == 0) {
                    reader.onload = function (event) {
                        if ((event.total / 1024).toFixed(0) > maxSize) {
                            GetMultiImage();
                            notificationService.displayWarning("Kích thước hình ảnh lớn hơn 1MB.");
                        }
                        else {
                            var image = {};
                            image.ProductImageID = $scope.product.ID;
                            image.FileName = event.target.result;
                            $scope.imgsrc.push(image);
                            $scope.$apply();
                            apiHttpService.post('api/fileimage/create', $scope.imgsrc, function (result) {
                                notificationService.displaySuccess("Thêm hình ảnh thành công.");
                                GetMultiImage();
                            }, function (error) {
                                notificationService.displayError(error);
                            });
                        }
                    }
                }
                else {
                    notificationService.displayWarning("Vui lòng cung cấp file hình ảnh.");
                }
                reader.readAsDataURL($files[i]);
            }
            //setTimeout(function () {
            //        apiHttpService.post('api/fileimage/create', $scope.imgsrc, function (result) {
            //            notificationService.displaySuccess("Thêm hình ảnh thành công.");
            //            GetMultiImage();
            //        }, function (error) {
            //            notificationService.displayError(error);
            //        });

            //}, 5000);
        }

        // Load FileImage.
        function GetMultiImage() {
            apiHttpService.get('api/fileimage/getmultiImage/' + $stateParams.productID, null, function (result) {
                $scope.imgsrc = result.data;
            }, function (error) {
                notificationService.displayError('Lấy dữ liệu thất bại.');
            });
        }

        // Delete FileImage.
        function DeleteImage(fileImageID, idx, image) {
            if (fileImageID === null || fileImageID === "") {
                $scope.imgsrc.splice(idx, 1);
                data.delete(idx, image);
                notificationService.displaySuccess('Hình ảnh đã được xóa.');
                $("#files").val('');
            }
            else {
                $ngBootbox.confirm('Bạn có chắc chắn muốn xóa?').then(function () {
                    var config = {
                        params: {
                            fileImageID: fileImageID
                        }
                    }
                    apiHttpService.del('api/fileimage/delete', config, function (result) {
                        notificationService.displaySuccess('Hình ảnh đã đượcc xóa.');
                        // reload image.
                        GetMultiImage();

                    }, function (error) {
                        notificationService.displayError('Xóa sản phẩm không thành công.');
                    })
                });
            }
        }

        // Load product detail.
        function GetproductbyID() {
            apiHttpService.get('api/product/getbyid/' + $stateParams.productID, null, function (result) {
                $scope.product = result.data;
                $scope.product.TagsVm = $scope.product.Tags;

                if ($scope.product.Image === null || $scope.product.Image == "") {
                    $(".pip").hide();
                }
            }, function (error) {
                notificationService.displayError('Lấy dữ liệu thất bại.');
            });
        }

        // Update product.
        function EditProduct() {
            //$.each($scope.imgsrc, function (i, item) {
            //    $scope.DataImage.push(item.FileName);
            //});
            //$scope.product.MoreImages = $scope.DataImage;

            data.append("product", angular.toJson($scope.product));
            apiHttpService.putProduct('api/product/edit', data, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã cập nhập thành công!');
                $state.go('products');
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        // Load product category.
        function LoadParentProduct() {
            apiHttpService.get('api/productcategory/getParent', null, function (result) {
                $scope.ProductCategory = result.data;
            });
        }

        // Load all tag
        function LoadAllTag(query) {
            apiHttpService.get('api/product/getalltag', null, function (result) {
                $scope.AllTag = result.data;
            });
        }

        // click tag
        $scope.GetTagItem = GetTagItem;
        var Name = [];
        var ID = [];
        var Type = [];
        var TagValue = [];
        var checkUnique = [];
        function GetTagItem(index, tagvalue) {
            Name = tagvalue;
            ID = tagvalue;
            Type = 'product';
            $scope.product.TagsVm.push({ ID: ID, Name: Name, Type: Type });
            TagValue = $scope.product.TagsVm;
            $scope.product.TagsVm = uniqBy(TagValue, JSON.stringify);
        }
        // validate tag duplicate
        function uniqBy(a, key) {
            var seen = {};
            return a.filter(function (item) {
                var k = key(item);
                return seen.hasOwnProperty(k) ? false : (seen[k] = true);
            })
        }


        LoadParentProduct();
        $scope.GetproductbyID();
        $scope.GetMultiImage();
        LoadAllTag();

    }
})(angular.module('shopproject.products'));
