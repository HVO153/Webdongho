var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/san-pham";
        });
        $('#btnPayment').off('click').on('click', function () {
            window.location.href = "/thanh-toan";
        });
        $('#btnUpdate').off('click').on('click', function () {
            var listProduct = $('.txtQuantity');
            var cartList = [];

            $.each(listProduct, function (i, item) {
                var quantity = $(item).val() || 0; // Nếu trống thì gán mặc định là 0
                cartList.push({
                    Quantity: parseInt(quantity), // Chuyển đổi giá trị sang số nguyên
                    Product: {
                        IdPro: $(item).data('id')
                    }
                });
            });

            $.ajax({
                url: '/Cart/Update',
                contentType: 'application/json',
                data: JSON.stringify(cartList),
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status === true) {
                        window.location.href = "/gio-hang";
                    }
                },
                error: function (xhr, status, error) {
                    console.error("AJAX Error: ", error);
                }
            });
        });

        $('#btnDeleteAll').off('click').on('click', function () {
            $.ajax({
                url: '/Cart/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });
        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            console.log("Delete button clicked");

            var id = $(this).data('id');
            console.log("Product ID to delete:", id); // Kiểm tra ID sản phẩm

            $.ajax({
                url: '/Cart/Delete',  // Đảm bảo URL đúng
                data: { id: id },  // Truyền đúng id sản phẩm
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    console.log("Response from server:", res);  // Kiểm tra phản hồi từ server
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                },
                error: function (xhr, status, error) {
                    console.error("AJAX Error:", error);
                }
            });
        });

    }
}
cart.init(); 