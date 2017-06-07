(function (app) {
    app.controller('slideListController', slideListController);

    slideListController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$filter', '$ngBootbox'];

    function slideListController($scope, apiHttpService, notificationService, $filter, $ngBootbox) {
        $scope.lstSlides = [];
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.keyword = '';
        $scope.GetSlides = GetSlides;
        $scope.search = search;
        $scope.DeleteSlide = DeleteSlide;
        $scope.DeleteMulti = DeleteMulti;

        $scope.checkAll = checkAll;

        $scope.noImage = '../../../assets/admin/img/NoImage.jpg';
        $scope.lstSlides.Image = null;
        // ham xoa nhieu san pham
        function DeleteMulti() {
            $ngBootbox.confirm('Bạn có chắc chắn muốn xóa?').then(function () {
                var lstItem = [];
                // lay ra list id cua product voi lstItem = [5,6] sau do
                // su dung JSON.stringifly de convert toi kieu string va truyen vao params

                $.each($scope.selected, function (i, item) {
                    lstItem.push(item.ID);
                });

                var config = {
                    params: {
                        //JSON.stringify chuyen params kieu int toi string boi vi backend web api co params lstProductCategoryID kieu string:
                        // vi du khi khong su dung se sinh ra 2 params giong nhau hien thi url nhu sau:
                        //localhost:64283/api/productcategory/deletemulti?lstProductCategoryID=5&lstProductCategoryID=6 400 (Bad Request)
                        // sau khi su dung localhost:64283/api/productcategory/deletemulti?lstProductCategoryID=%5B5,6%5D
                        lstSlideID: JSON.stringify(lstItem)
                    }
                }
                apiHttpService.del('api/slide/deletemulti', config, function (result) {
                    notificationService.displaySuccess(result.data + ' thể lọai sản phẩm đã được xóa.');
                    //xoa xong goi lai ham search de get lai san pham
                    search();
                    
                }, function (error) {
                    notificationService.displayError('Xóa không thành công.');
                })
            });
        }

        // ham xoa 1 san pham voi params : productCategoryID
        function DeleteSlide(slideID) {
            $ngBootbox.confirm('Bạn có chắc chắn muốn xóa?').then(function () {
                var config = {
                    params: {
                        slideID: slideID
                    }
                }
                apiHttpService.del('api/slide/delete', config, function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được xóa.');
                    //xoa xong goi lai ham search de get lai san pham
                    search();
                }, function (error) {
                    notificationService.displayError('Xóa không thành công.');
                })
            });
        }

        // ham kiem tra su kien check all checkbox.
        // mac dinh ham checkbox ban dau la false,nghia la k co check
        // neu nhu xet if la false dau tien,khi check vao toan bo se la false o k check
        // khi xet if ban dau phai xet true if nhu o^ dc check.
        function checkAll() {
            if ($scope.selectAll) {
                $scope.selectAll = true;
            }
            else {
                $scope.selectAll = false;
            }
            angular.forEach($scope.lstSlides, function (item) {
                item.checked = $scope.selectAll;
            });
        }

        // ham lang nghe thay doi? su kien checked cho checkbox trong lstSlides,
        // de show button delete or hidden button.
        $scope.$watch("lstSlides", function (n, o) {
            var trues = $filter("filter")(n, { checked: true });
            if (trues.length > 0) {
                // su dung $watch: bien $scope.selected se lay toan bo gia tri cua lstSlides khi duoc checked.
                $scope.selected = trues;
                document.getElementById("btnDelete").style.visibility = "visible";
            }
            else {
                document.getElementById("btnDelete").style.visibility = "hidden";
            }
        }, true);

        // ham tim kiem goi lai ham GetSlides();,boi vi ben trong co params keyword.
        function search() {
            GetSlides();
            $scope.selectAll = false;
        }
        // default value page = 10
        $(function () {
            $("#selectbox option[value='10']").attr("selected", "selected");
        })

        // ham lay san pham
        function GetSlides(page) {
            //mac dinh page = rong~ nen se gan = 0,bien page duoc truyen vao tu pagerdirective.html
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: $scope.pageSize
                }
            }
            $scope.loading = true;
            apiHttpService.get('api/slide/getall', config, function (result) {
                $scope.lstSlides = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.loading = false;
            }, function () {
                notificationService.displayError('Lấy danh sách sản phẩm không thành công.');
                $scope.loading = false;
            });
        }

        $scope.GetSlides();
    }
})(angular.module('shopproject.slides'));