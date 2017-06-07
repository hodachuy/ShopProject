/// <reference path="groupAddController.js" />
(function (app) {
    app.controller('groupAddController', groupAddController);

    //$state cua uirouter
    groupAddController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$state', 'commonService'];
    function groupAddController($scope, apiHttpService, notificationService, $state, commonService) {
        $scope.AddGroup = AddGroup;
        $scope.groupProduct = {
            Status: true
        }

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
        // Single Image
        //$scope.imageUpload = function (event) {
        //    var files = event.target.files; //FileList object
        //    for (var i = 0; i < files.length; i++) {
        //        var file = files[i];
        //        var reader = new FileReader();
        //        reader.onload = function (e) {
        //            $scope.$apply(function () {
        //                $scope.groupProduct.Image = e.target.result;
        //                var filename = e.target;

        //                $("<span class=\"pip\">" +
        //           "<img class=\"imageThumb\" src=\"" + $scope.groupProduct.Image + "\" title=\"" + filename.Name + "\"/>" +
        //           "<br/><span class=\"remove\">Remove</span>" +
        //           "</span>").insertAfter("#files");

        //                $(".remove").click(function () {

        //                    $(this).parent(".pip").remove();

        //                    //$("#files").val(''); co the su dung cach nay`.

        //                    var control = $("#files");
        //                    control.replaceWith(control.val('').clone(true));

        //                    $scope.groupProduct.Image = null;
        //                });
        //            });
        //        };
        //        reader.readAsDataURL(file);
        //    }
        //}       

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

        function AddGroup() {
            apiHttpService.post('api/group/create', $scope.groupProduct, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã thêm thành công.');
                $state.go('groups');
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }       
    }
})(angular.module('shopproject.groups'));