var cart = {
    init: function () {
        cart.loadData();
        cart.registerEven();
    },
    registerEven: function () {
        $('#frmPayment').validate({
            rules: {
                name: "required",
                address: "required",
                email: {
                    required: true,
                    email: true
                },
                phone: {
                    required: true,
                    number: true
                }
            },
            messages: {
                name: "Yêu cầu nhập tên",
                address: "Yêu cầu nhập địa chỉ",
                email: {
                    required: "Bạn cần nhập email",
                    email: "Định dạng email chưa đúng"
                },
                phone: {
                    required: "Số điện thoại được yêu cầu",
                    number: "Số điện thoại phải là số."
                }
            }
        });
        $('.txtQuantity').off('keyup').on('keyup', function () {
            var quantity = parseInt($(this).val());
            var productid = parseInt($(this).data('id'));
            var quantityDb = parseInt($(this).data('quantity'));
            var price = parseFloat($(this).data('price'));
            if (isNaN(quantity) == false) {

                // Thành tiền
                var amount = quantity * price;

                $('#amount_' + productid).text(numeral(amount).format('0,0'));
            }
            else {
                $('#amount_' + productid).text(0);
            }

            // Tổng tiền
            // gọi phương thức tính tổng.
            // sử dụng .text giống như <span id="lblTotalOrder"></span> sẽ = giá trị trả về của phương thức cart.getTotalOrder()
            $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));

        }).on('keypress', function (e) {
            // Kiểm tra không cho phép nhập giá trị số lượng âm.
            var theEvent = e || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
            var regex = /[0-9]|\./;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault)
                    theEvent.preventDefault();
            }
        }).on('input', function () {
            // Kiểm tra so sanh số lượng nhập với số lượng trong kho hàng.
            // Kiểm tra nếu để giá trị rỗng -> 0
            var productid = parseInt($(this).data('id'));
            var quantity = parseInt($(this).val());
            var quantityDb = parseInt($(this).data('quantity'));
            if (isNaN(quantity))
                $('#dfQuantity_' + productid).val(0);
            else {
                $('#dfQuantity_' + productid).val(quantity);
            }
            if (quantity > quantityDb) {
                $('#dfQuantity_' + productid).val(quantityDb);
                alert('Xin lỗi quý khách chỉ còn ' + quantityDb + ' sản phẩm trong kho hàng.');
            }
            cart.updateAll();
        });
        $('#chkUserLoginInfo').off('click').on('click', function () {
            if ($(this).prop('checked'))
                cart.getLoginUser();
            else {
                $('#txtName').val('');
                $('#txtAddress').val('');
                $('#txtEmail').val('');
                $('#txtPhone').val('');
            }
        });
        $('#btnCreateOrder').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#frmPayment').valid();
            if (isValid) {
                cart.createOrder();
            }

        });
        $('input[name="paymentMethod"]').off('click').on('click', function () {
            if ($(this).val() == 'ATM_ONLINE') {
                $('#bankContent').show();
            }
            else {
                $('.boxContent').hide();
            }
        });
    },
    getLoginUser: function () {
        $.ajax({
            url: '/ShoppingCart/GetUser',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;
                    $('#txtName').val(user.FullName);
                    $('#txtAddress').val(user.Address);
                    $('#txtEmail').val(user.Email);
                    $('#txtPhone').val(user.PhoneNumber);
                }
            }
        });
    },
    createOrder: function () {
        var order = {
            CustomerName: $('#txtName').val(),
            CustomerAddress: $('#txtAddress').val(),
            CustomerEmail: $('#txtEmail').val(),
            CustomerMobile: $('#txtPhone').val(),
            CustomerMessage: $('#txtMessage').val(),
            PaymentMethod: $('input[name="paymentMethod"]:checked').val(),
            BankCode: $('input[groupname="bankcode"]:checked').prop('id'),
            Status: false
        }
        $.ajax({
            url: '/ShoppingCart/CreateOrder',
            type: 'POST',
            dataType: 'json',
            data: {
                orderViewModel: JSON.stringify(order)
            },
            success: function (response) {
                if (response.status) {
                    if (response.urlCheckout != undefined && response.urlCheckout != '') {

                        window.location.href = response.urlCheckout;
                    }
                    else {
                        console.log('create order ok');
                        $('#divCheckout').hide();
                        cart.deleteAll();
                        setTimeout(function () {
                            $('#cartContent').html('<span style="margin-left:30%">Cảm ơn bạn đã đặt hàng thành công. Chúng tôi sẽ liên hệ sớm nhất.</span>');
                        }, 1000);
                    }

                }
                else {
                    $('#divMessage').show();
                    $('#divMessage').text(response.message);
                }
            }
        });
    },
    getTotalOrder: function () {
        var listTextBox = $('.txtQuantity');
        var total = 0;
        // Total price
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },
    deleteAll: function () {
        $.ajax({
            url: '/ShoppingCart/DeleteAll',
            type: 'Post',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                    $('#divCheckout').hide();
                }
            }
        });
    },
    loadData: function () {
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'Get',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var template = $('#tplCheckOut').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {

                            ProductId: item.ProductId,
                            ProductAlias: item.Product.Alias,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            QuantityDb: item.Product.Quantity,
                            Quantity: item.Quantity,
                            PromotionPrice: item.Product.PromotionPrice,
                            Price: item.Product.Price,
                            PriceFormat: numeral(item.Product.Price).format('0,0'),
                            Amount: numeral(item.Quantity * item.Product.Price).format('0,0'),
                        });
                    });
                    $('#itemCheckout').html(html);

                    // gọi sự kiện này khi load, để tính ra lun giá tiền đàu tiên khi chưa thay đổi số lương.
                    $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));
                    // phương thức này chức các sự kiện ở các thẻ như input,button vv...
                    // khi load phải gọi tất cả chúng.
                    cart.registerEven();

                }
            }
        })
    }
}
cart.init();