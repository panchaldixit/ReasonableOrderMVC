var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/orders/getall/",
            "type": "GET",
            "datatype": "json"
        },
        "paging": false,
        "info": false,
        "searching": false,
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "quantity", "width": "20%" },
            { "data": "price", "width": "20%" },
            { "data": "total", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/Orders/Upsert?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Edit
                        </a>
                        &nbsp;
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
                            onclick=Delete('/orders/Delete?id='+${data})>
                            Delete
                        </a>
                        </div>`;
                }, "width": "40%"
            }
        ],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            // converting to interger to find total
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            // computing column Total of the complete result 
            var totalQuantity = api
                .column(1)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            var totalPrice = api
                .column(2)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            var subTotal = api
                .column(3)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer by showing the total with the reference of the column index 
            $(api.column(0).footer()).html('Sub Total');
            $(api.column(1).footer()).html(totalQuantity);
            $(api.column(2).footer()).html(totalPrice);
            $(api.column(3).footer()).html(parseFloat(subTotal));



            var TaxAmount = (subTotal * 15) / 100;
            var TotalAmount = subTotal + TaxAmount;
            $("#TaxValue").html(TaxAmount);
            $("#TotalValue").html(TotalAmount);
        },
        "language": {
            "emptyTable": "No Data Found !!!"
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function SubmitOrder() {
    var totalOrderValue = $("#TotalValue")["0"].innerText;
    if (totalOrderValue != "0") {
        if (totalOrderValue && (parseInt(totalOrderValue) > 100))
            $("div.primary").fadeIn(50).delay(2500).fadeOut(90);
        $.ajax({
            type: "GET",
            url: '/orders/SubmitOrder?orderValue=' + totalOrderValue,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    var message = "Your order has been received. Your order number is " + response.message + " !";
                    $('div.success').text(message);
                    $("div.success").fadeIn(50).delay(2500).fadeOut(90);
                    setTimeout(function () {location.reload();}, 700);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (response) {
                toastr.error(response.message);
            }
        });
    }
    else {
        toastr.error("Please add an item to submit order!");
    }
}

function GetPrice() {
    var itemName = $("#items").val();
    $.ajax({
        url: "/orders/getprice?itemName=" + itemName, success: function (result) {
            $("#quantity").val("1");
            $("#price").val(result["price"]);
            $("#totalPrice").val(result["price"]);
        }
    });
}

function SetTotalPriceBasedOnQuantity(value) {
    var quantity = parseInt(value);
    $("#totalPrice").val(quantity * parseFloat($("#price").val()));
}