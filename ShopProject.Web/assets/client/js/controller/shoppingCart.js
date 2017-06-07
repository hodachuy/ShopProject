var cart = {
    init: function () {
        cart.loadData();
        cart.registerEven();
    },
    registerEven: function () {

        $('.btnAddToCart').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            cart.addItem(productId);
        });

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
        // Xóa từng sản phẩm
        $('.btnDeleteItem').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            cart.deleteItem(productId);
        });

        // cập nhật cho nhiều dòng sản phẩm phải sử dụng class.
        // <td><input type="number" min="0" data-id="{{ProductId}}" data-price="{{Price}}" value="{{Quantity}}"
        // data-quantity="{{QuantityDb}}" class="input txtQuantity" id="dfQuantity_{{ProductId}}"/></td>
        // --value="{{Quantity}}"       : gọi val() để lấy giá trị.       
        // --data-id="{{ProductId}}"    : goi .data('id') để lấy giá trị.
        // --data-quantity="{{QuantityDb}}": goik .data(quantity) để lấy giá trị.
        // -- class="input txtQuantity" .txtQuantity xử lý cho input, sự kiện input cho nhiều sản phẩm phải sử dụng class,
        // có thể sử dụng id với id truyền vào id="dfQuantity_{{ProductId}}"
        // -- id="dfQuantity_{{ProductId}}" hiển thị dùng cho xử lý if else cho oninput,
        // -- không dùng class="input txtQuantity" vì nó thay đổi toàn bộ.
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
                toastr.warning('Xin lỗi quý khách chỉ còn ' + quantityDb + ' sản phẩm trong kho hàng.', '', { timeOut: 1500, positionClass: "toast-top-left" });
            }
            cart.updateAll();
        });
        $('#btnContinue').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/";
        });
        $('#btnDeleteAll').off('click').on('click', function (e) {
            e.preventDefault();
            cart.deleteAll();
            //window.location.href = "/";   
        });
        $('#btnCheckout').off('click').on('click', function (e) {
            e.preventDefault();
            //$('#divCheckout').show();
            window.location.href = "/thanh-toan.html";
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
            PaymentMethod: "Thanh Toán Tiền Mặt",
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
                    console.log('create order ok');
                    $('#divCheckout').hide();
                    cart.deleteAll();
                    setTimeout(function () {
                        $('#cartContent').html('Cảm ơn bạn đã đặt hàng thành công. Chúng tôi sẽ liên hệ sớm nhất.');
                    }, 1000);


                }
            }
        });
    },
    // phương thức tính tổng tiền khi thay đổi số lượng.
    getTotalOrder: function () {
        var listTextBox = $('.txtQuantity');
        var total = 0;
        // Total price
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },
    getTotalCart: function () {
        var listTextBox = $('.txtQuantity');
        var total = 0;
        // Total price
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val());
        });
        return total;
    },
    updateAll: function () {
        var cartList = [];
        $.each($('.txtQuantity'), function (i, item) {
            cartList.push({
                ProductId: $(item).data('id'),
                Quantity: $(item).val()
            });
        });
        $.ajax({
            url: '/ShoppingCart/Update',
            type: 'POST',
            data: {
                cartData: JSON.stringify(cartList)
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                    console.log('Update ok');
                }
            }
        });
    },
    addItem: function (productId) {
        $.ajax({
            url: '/ShoppingCart/Add',
            data: {
                productId: productId
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    if (response.data !== null) {
                        // Load session first time when click button.
                        $('.drawer-nav').load("/view-cart.html", function () {
                            // show panel left.    
                            $(window).scrollTop(0);
                            $('.drawer').drawer('open');
                            cart.loadData();
                        });
                    }
                }
                else {
                    toastr.warning(response.message, '', { timeOut: 1500, positionClass: "toast-top-left" });
                }
            }
        });
    },
    deleteItem: function (productId) {
        $.ajax({
            url: '/ShoppingCart/DeleteItem',
            data: {
                productId: productId
            },
            type: 'Post',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                    if (response.data === undefined) {
                        $('#divCheckout').hide();
                    }
                }
            }
        });
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
                    var template = $('#tplCart').html();
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
                    $('#cartInfoBody').html(html);

                    // load count cart.
                    $('.cart').load("/count-cart.html", function () {
                        $('#cart-count').text(cart.getTotalCart());
                        $('#openPanel').on('click', function (e) {
                            $('.drawer').drawer('open');
                        });
                    });

                    if (html == '') {
                        $('#cartContent').html('Không có sản phẩm nào trong giỏ hàng');
                    }
                    else {
                        $('#cartInfoBody').html(html);
                    }
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