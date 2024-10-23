function showDeleteModal(purchaseOrderId) {
    try {
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this.",
            icon: "warning",
            showCancelButton: !0,
            confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
            cancelButtonClass: "btn btn-danger w-xs mt-2",
            confirmButtonText: "Delete",
            buttonsStyling: !1,
            showCloseButton: !0,
        }).then(function (t) {
            if (t.value) {
                fetch('delete', {
                    method: 'post',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        id: quotationId
                    })
                })
                .then(response => {
                    return response.json();
                })
                .then(data => {
                    if (!data.isSuccess) {
                        Swal.fire({
                            title: 'Error!!',
                            html: data.message,
                            icon: 'error',
                            confirmButtonClass: 'btn btn-danger w-xs mt-2',
                            buttonsStyling: !1,
                            footer: '',
                            showCloseButton: !1
                        });
                        return;
                    }
        
                    Swal.fire({
                        title: 'Success',
                        html: data.message,
                        icon: 'success',
                        confirmButtonClass: 'btn btn-success w-xs mt-2',
                        buttonsStyling: !1,
                        footer: '',
                        showCloseButton: !1
                    }).then(function (t) {
                        window.location.href = 'list';
                    });
                })
                .catch(error => {
                    Swal.fire({
                        title: 'Error!!',
                        html: error,
                        icon: 'error',
                        confirmButtonClass: 'btn btn-danger w-xs mt-2',
                        buttonsStyling: !1,
                        footer: '',
                        showCloseButton: !1
                    });
                });
            }
        });
    } catch(exception) {
        Swal.fire({
            title: 'Error!!',
            html: exception,
            icon: 'error',
            confirmButtonClass: 'btn btn-danger w-xs mt-2',
            buttonsStyling: false,
            footer: '',
            showCloseButton: true
        });
    }
}

function downloadQuotation(purchaseOrderId)
{
    try {
        alert('This feature is not available yet.');
    } catch(exception) {
        Swal.fire({
            title: 'Error!!',
            html: exception,
            icon: 'error',
            confirmButtonClass: 'btn btn-danger w-xs mt-2',
            buttonsStyling: false,
            footer: '',
            showCloseButton: true
        });
    }
}

$(document).ready(function () {
    try {
        $('#tbPurchaseOrders tbody').html(
            '<tr>' +
                '<td colspan="9" class="text-center">Rendering results... <img width="30" src="../assets/images/infinity.gif"></td>' +
            '</tr>');

        $.ajax({
            url: 'getAll',
            method: 'GET',
            success: function (data) {
                if (!data.isSuccess) {
                    Swal.fire({
                        title: 'Error!!',
                        html: data.message,
                        icon: 'error',
                        confirmButtonClass: 'btn btn-danger w-xs mt-2',
                        buttonsStyling: false,
                        footer: '',
                        showCloseButton: true
                    });
                }

                var tbody = $('#tbPurchaseOrders tbody');
                tbody.empty();
                if (data.results.length > 0) {
                    data.results.forEach(function (purchaseOrder) {
                        var row =
                            '<tr>' +
                                '<td><span class="badge bg-dark">' + purchaseOrder.code + '</span></td>' +
                                '<td>' + purchaseOrder.supplier.businessName + '</td>' +
                                '<td>' + purchaseOrder.user.username + '</td>' +
                                '<td>' + purchaseOrder.payment.description + '</td>' +
                                '<td>$' + parseFloat(purchaseOrder.totalAmount).toFixed(2) + '</td>' +
                                '<td>' + purchaseOrder.currency.description + '</td>' +
                                '<td><span class="badge rounded-pill badge-soft-' + purchaseOrder.statusColor + '">' + purchaseOrder.statusName + '</span></td>' +
                                '<td>' + purchaseOrder.creationDateAsString + '</td>' +
                                '<td class="text-center">' +
                                    '<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/purchaseOrder/update?id=' + purchaseOrder.id + '\'" title="Update"><i class="ri-pencil-fill"></i></button>' +
                                    '<button type="button" class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal(' + purchaseOrder.id  + ')" title="Delete"><i class="ri-delete-bin-2-fill"></i></button>' +
                                    '<button type="button" class="btn btn-info btn-icon waves-effect waves-light mx-1" onclick="downloadPurchaseOrder(' + purchaseOrder.id  + ')" title="Download"><i class="ri-file-download-fill"></i></button>' +
                                    '<button type="button" class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/purchaseOrder/detail?id=' + purchaseOrder.id + '\'" title="View"><i class="ri-eye-fill"></i></button>' +
                                '</td>' +
                            '<tr>'
                        tbody.append(row);
                    });
                } else {
                    $('#tbPurchaseOrders tbody').html(
                        '<tr>' +
                            '<td colspan="9" class="text-center">We have nothing to show.</td>' +
                        '</tr>');
                }
                $('#dvTotalPurchaseOrders').html('Total purchase orders: ' + data.results.length);
            },
            error: function (xhr, status, error) {
                $('#tbPurchaseOrders tbody').html(
                    '<tr>' +
                        '<td colspan="9" class="text-center">We have nothing to show.</td>' +
                    '</tr>');
                Swal.fire({
                    title: 'Error!!',
                    html: error,
                    icon: 'error',
                    confirmButtonClass: 'btn btn-danger w-xs mt-2',
                    buttonsStyling: false,
                    footer: '',
                    showCloseButton: true
                });
            }
        });
    } catch (exception) {
        $('#tbPurchaseOrders tbody').html(
            '<tr>' +
                '<td colspan="9" class="text-center">We have nothing to show.</td>' +
            '</tr>');
        Swal.fire({
            title: 'Error!!',
            html: exception,
            icon: 'error',
            confirmButtonClass: 'btn btn-danger w-xs mt-2',
            buttonsStyling: false,
            footer: '',
            showCloseButton: true
        });
    }
});