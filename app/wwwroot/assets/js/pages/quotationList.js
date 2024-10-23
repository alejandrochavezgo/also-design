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

function downloadQuotation(quotationId)
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
        $('#tbQuotations tbody').html(
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

                var tbody = $('#tbQuotations tbody');
                tbody.empty();
                if (data.results.length > 0) {
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
                                    '<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/quotation/update?id=' + quotation.id + '\'" title="Update"><i class="ri-pencil-fill"></i></button>' +
                                    '<button type="button" class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal(' + quotation.id  + ')" title="Delete"><i class="ri-delete-bin-2-fill"></i></button>' +
                                    '<button type="button" class="btn btn-info btn-icon waves-effect waves-light mx-1" onclick="downloadQuotation(' + quotation.id  + ')" title="Download"><i class="ri-file-download-fill"></i></button>' +
                                    '<button type="button" class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/quotation/detail?id=' + quotation.id + '\'" title="View"><i class="ri-eye-fill"></i></button>' +
                                '</td>' +
                            '<tr>'
                        tbody.append(row);
                    });
                } else {
                    $('#tbQuotations tbody').html(
                        '<tr>' +
                            '<td colspan="9" class="text-center">We have nothing to show.</td>' +
                        '</tr>');
                }
                $('#dvTotalQuotations').html('Total quotations: ' + data.results.length);
            },
            error: function (xhr, status, error) {
                $('#tbQuotations tbody').html(
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
        $('#tbQuotations tbody').html(
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