(function (app) {
    app.factory('notificationService', notificationService);
    function notificationService() {
        toastr.options = {
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "fadeIn": 300,
            "fadeOut": 1000,
            "timeOut": 3000,
            "extendedTimeOut": 1000
        };

        return {
            displayInfo: notificationInfo,
            displaySuccess: notificationSuccess,
            displayWarning: notificationWarning,
            displayError: notificationError
        }

        function notificationInfo(message) {
            toastr.info(message);
        }

        function notificationSuccess(message) {
            toastr.success(message);
        }

        function notificationWarning(message) {
            toastr.warning(message, '', { timeOut: 2000, positionClass: "toast-top-full-width" });
        }

        function notificationError(error) {
            if (Array.isArray(error)) {
                error.each(function (err) {
                    toastr.error(err);
                });
            }
            else {
                toastr.error(error);
            }
        }
    }

})(angular.module('shopproject.common'));