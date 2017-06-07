(function (app) {
    app.controller('orderViewController', orderViewController);
    orderViewController.$inject = ['$scope', 'apiHttpService', 'notificationService', '$state', '$stateParams'];
    function orderViewController($scope, apiHttpService, notificationService, $state, $stateParams) {
        $scope.order = [];
        $scope.orderDetail = [];
        $scope.EditOrder = EditOrder;
        $scope.GetOrderDetail = GetOrderDetail;
        $scope.message = 'trống';

        function GetOrderbyID() {
            apiHttpService.get('api/order/getbyid/' + $stateParams.orderID, null, function (result) {
                $scope.order = result.data;
            }, function (error) {
                notificationService.displayError('Load dữ liệu không thành công');
            });
        }

        function EditOrder() {
            apiHttpService.put('api/order/edit', $scope.order, function (result) {
                notificationService.displaySuccess('Đơn hàng 100'+result.data.ID + ' đã được xác nhận');
                $state.go('orders');
            }, function (error) {
                notificationService.displayError('Lỗi xảy ra !');
            });
        }
        
        function GetOrderDetail() {
            apiHttpService.get('api/order/getorderdetail/' + $stateParams.orderID, null, function (result) {
                $scope.orderDetail = result.data;
                $scope.Total = 0;
                $.each(result.data, function (i,item) {
                    $scope.Total += item.Amount;
                });

                
            }, function (error) {
                notificationService.displayError('Lấy dữ liệu thất bại.');
            });
        }
   
        GetOrderbyID();
        $scope.GetOrderDetail();
    }

})(angular.module('shopproject.orders'));