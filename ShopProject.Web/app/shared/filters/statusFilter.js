(function (app) {
    app.filter('statusFilter', function () {
        return function (input) {
            if (input == true)
                return 'Kích hoạt';
            else
                return 'Khóa';
        }
    });
    app.filter('statusOrderFilter', function () {
        return function (input) {
            if (input == true)
                return 'Xác nhận';
            else
                return 'Đơn hàng chưa được xác nhận';
        }
    });
})(angular.module('shopproject.common'));