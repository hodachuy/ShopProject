(function (app) {
    app.controller('productCategoryEditController', productCategoryEditController);

    //service $state dung de dieu huong trang
    //service $stateParams de lay id cua url

    productCategoryEditController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$state', '$stateParams', 'commonService'];
    function productCategoryEditController($scope, apiHttpService, notificationService, $state, $stateParams, commonService) {
        $scope.productCategory = {
            Status: true
        }
        $scope.flatFolders = [];
        $scope.EditProductCategory = EditProductCategory;

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

        // function chuyen doi Name toi ky tu SEO
        $scope.GetSeoTitle = GetSeoTitle;
        function GetSeoTitle() {
            $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name);
        }

        // Single Image
        //$scope.imageUpload = function (event) {
        //    var files = event.target.files; //FileList object

        //    for (var i = 0; i < files.length; i++) {
        //        var file = files[i];
                
        //        var reader = new FileReader();
        //        reader.onload = function (e) {
        //            $scope.$apply(function () {
        //                $scope.productCategory.Image = e.target.result;
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
        //        $scope.productCategory.Image = null;
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
                                $scope.productCategory.Image = e.target.result;
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
                $('imageThumb').attr('src', $scope.productCategory.Image) == null;
                var control = $("#filesImage");
                control.replaceWith(control.val('').clone(true));
                $scope.productCategory.Image = null;
                $(".pip").hide();
            });
        });


        // Font weight chữ đậm cho danh mục cha.
        $(document).ready(function () {
            $("#selectCategory").click(function () {
                $.each($scope.flatFolders, function (i, item) {
                    var optMain = item.Name;
                    if (optMain.indexOf(' ') == 0) {
                        $('#selectCategory option:contains(' + optMain + ')').css({ "font-weight": "bold", "color": "#0769ad" });
                    }
                    if (optMain.indexOf('-') == 0) {
                        $('#selectCategory option:contains(' + optMain + ')').css("font-weight", "bold");
                    }
                });
            });
        });

        // Cau hinh url:
        // service $stateParams de lay id productCategoryID cua url.
        // 'api/productcategory/getbyid/' + $stateParams.productCategoryID
        // cau hinh thong tin tren dung voi ben back end web api
        // [Route("getbyid/{productCategoryID:int}")]
        // Ngoai ra cau hinh params o module.js o cong state url: "/edit_product_category/:productCategoryID",
        // du'ng voi uirouter o view page ui-rsef =  "edit_product_category({productCategoryID:item.ID})"
        function GetproductCategorybyID() {
            apiHttpService.get('api/productcategory/getbyid/' + $stateParams.productCategoryID, null, function (result) {
                $scope.productCategory = result.data;
                if ($scope.productCategory.Image === null || $scope.productCategory.Image == "") {
                    $(".pip").hide();
                }
            }, function (error) {
                notificationService.displayError('Load data fail.');
            });
        }

        function EditProductCategory() {
            apiHttpService.put('api/productcategory/edit', $scope.productCategory, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã thêm thành công.');
                $state.go('product_categories');
            }, function (error) {
                notificationService.displayError('Please input filed !');
            });
        }

        function LoadParentProductCategory() {
            apiHttpService.get('api/productcategory/getParent', null, function (result) {
                $scope.productCategoryParent = commonService.getTree(result.data, "ID", "ParentID");
                $scope.productCategoryParent.forEach(function (item) {
                    recur(item, 0, $scope.flatFolders);
                });
            });
        }

        function times(n, str) {
            var result = '';
            for (var i = 0; i < n; i++) {
                result += str;
            }
            return result;
        };
        function recur(item, level, arr) {
            arr.push({
                Name: times(level, '--') + ' ' + item.Name,
                ID: item.ID,
                Level: level,
                Indent: times(level, '-')
            });
            if (item.children) {
                item.children.forEach(function (item) {
                    recur(item, level + 1, arr);
                });
            }
        };

        LoadParentProductCategory();

        GetproductCategorybyID();
    }

})(angular.module('shopproject.productCategories'));