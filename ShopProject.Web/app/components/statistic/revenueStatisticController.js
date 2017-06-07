(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', 'apiHttpService', 'notificationService','$filter'];

    function revenueStatisticController($scope, apiHttpService, notificationService,$filter) {
        $scope.tabledata = [];
        $scope.labels = [];
        $scope.series = ['Doanh số', 'Lợi nhuận'];
        $scope.chartdata = [];
        $scope.fromDate = '01/01/2016';
        $scope.toDate = '01/01/2018';
        function getStatistic() {
            var config = {
                param: {
                    //mm/dd/yyyy
                    fromDate: $scope.fromDate,
                    toDate: $scope.toDate
                }
            }
            apiHttpService.get('api/statistic/getrevenue?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                $scope.tabledata = response.data;
                var labels = [];
                var chartData = [];
                var revenues = [];
                var benefits = [];
                $.each(response.data, function (i, item) {
                    labels.push($filter('date')(item.Date,'dd/MM/yyyy'));
                    revenues.push(item.Revenues);
                    benefits.push(item.Benefit);
                });
                chartData.push(revenues);
                chartData.push(benefits);

                $scope.chartdata = chartData;
                $scope.labels = labels;
            }, function (response) {
                notificationService.displayError('Không thể tải dữ liệu');
            });
        }

        getStatistic();
    }

})(angular.module('shopproject.statistics'));