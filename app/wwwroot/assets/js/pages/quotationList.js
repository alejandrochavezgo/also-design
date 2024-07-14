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
                        '<td>' + quotation.code + '</td>' +
                        '<td>' + quotation.client.businessName + '</td>' +
                        '<td>' + quotation.user.userName + '</td>' +
                        '<td>' + quotation.payment.description + '</td>' +
                        '<td>' + quotation.currency.description + '</td>' +
                        '<td>' + quotation.subtotal + '</td>' +
                        '<td>' + quotation.tax + '</td>' +
                        '<td>' + quotation.total + '</td>' +
                        '<td class="text-center">' +
                            '<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/quotation/update?quotationId=' + quotation.id + '\'"><i class="ri-pencil-fill"></i></button>' +
                        '</td>' +
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