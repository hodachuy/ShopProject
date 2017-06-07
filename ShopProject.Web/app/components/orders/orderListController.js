(function (app) {
    app.controller('orderListController', orderListController);
   

    orderListController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$filter', '$ngBootbox','$window'];

    function orderListController($scope, apiHttpService, notificationService, $filter, $ngBootbox,$window) {
        $scope.lstOrders = [];
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.keyword = '';
        $scope.GetOrders = GetOrders;
        $scope.search = search;
        $scope.DeleteOrder = DeleteOrder;
        $scope.DeleteMulti = DeleteMulti;


        $scope.checkAll = checkAll;

        function DeleteMulti() {
            $ngBootbox.confirm('Bạn có chắc chắn muốn xóa?').then(function () {
                var lstItem = [];
                $.each($scope.selected, function (i, item) {
                    lstItem.push(item.ID);
                });
                var config = {
                    params: {
                        lstOrderID: JSON.stringify(lstItem)
                    }
                }
                apiHttpService.del('api/order/deletemulti', config, function (result) {
                    notificationService.displaySuccess(result.data + ' đơn hàng đã được xóa.');
                    search();
                }, function (error) {
                    notificationService.displayError('Xóa đơn hàng không thành công.');
                })
            });
        }

        function DeleteOrder(ordertID) {
            $ngBootbox.confirm('Bạn có chắc chắn muốn xóa?').then(function () {
                var config = {
                    params: {
                        ordertID: ordertID
                    }
                }
                apiHttpService.del('api/order/delete', config, function (result) {
                    notificationService.displaySuccess('Đơn hàng ' + '100'+result.data.ID + ' đã được xóa.');
                    search();
                }, function (error) {
                    notificationService.displayError('Xóa đơn hàng không thành công.');
                })
            });
        }

        function checkAll() {
            if ($scope.selectAll) {
                $scope.selectAll = true;
            }
            else {
                $scope.selectAll = false;
            }
            angular.forEach($scope.lstOrders, function (item) {
                item.checked = $scope.selectAll;
            });
        }

        $scope.$watch("lstOrders", function (n, o) {
            var trues = $filter("filter")(n, { checked: true });
            if (trues.length > 0) {
                $scope.selected = trues;
                document.getElementById("btnDelete").style.visibility = "visible";
            }
            else {
                document.getElementById("btnDelete").style.visibility = "hidden";
            }
        }, true);

        function search() {
            GetOrders();
            $scope.selectAll = false;
        }

        $(function () {
            $("#selectbox option[value='10']").attr("selected", "selected");
        })

        function GetOrders(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: $scope.pageSize
                }
            }
            $scope.loading = true;
            apiHttpService.get('api/order/getall', config, function (result) {
                $scope.lstOrders = result.data.Items;            
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.loading = false;
            }, function () {
                notificationService.displayError('Lấy danh sách sản phẩm không thành công.');
                $scope.loading = false;
            });
        }

        $scope.GetOrders();
    }
})(angular.module('shopproject.orders'));