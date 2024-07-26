function showDeleteModal(quotationId) {
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

$(document).ready(function () {
    try {
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

                var tbody = $('#tbQuotations tbody');
                tbody.empty();
                data.results.forEach(function (quotation) {
                    var row =
                        '<tr>' +
                            '<td><span class="badge bg-dark">' + quotation.code + '</span></td>' +
                            '<td>' + quotation.client.businessName + '</td>' +
                            '<td>' + quotation.user.username + '</td>' +
                            '<td>' + quotation.payment.description + '</td>' +
                            '<td>$' + parseFloat(quotation.totalAmount).toFixed(2) + '</td>' +
                            '<td>' + quotation.currency.description + '</td>' +
                            '<td><span class="badge rounded-pill badge-soft-' + quotation.statusColor + '">' + quotation.statusName + '</span></td>' +
                            '<td>' + quotation.creationDateAsString + '</td>' +
                            '<td class="text-center">' +
                                '<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/quotation/update?quotationId=' + quotation.id + '\'"><i class="ri-pencil-fill"></i></button>' +
                                '<button type="button" class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal(' + quotation.id  + ')"><i class="ri-delete-bin-2-fill"></i></button>' +
                            '</td>' +
                        '<tr>'
                    tbody.append(row);
                });
                $('#dvTotalQuotations').html('Total quotations: ' + data.results.length);
            },
            error: function (xhr, status, error) {
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