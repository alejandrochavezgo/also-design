$(document).ready(function () {
    try {
        $('#tbSuppliers tbody').html(
            '<tr>' +
                '<td colspan="10" class="text-center">Rendering results... <img width="30" src="../assets/images/infinity.gif"></td>' +
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
                var tbody = $('#tbSuppliers tbody');
                tbody.empty();
                if (data.results.length > 0) {
                    data.results.forEach(function (supplier) {
                        var row =
                            '<tr>' +
                            '<td>' + supplier.id + '</td>' +
                            '<td>' + supplier.businessName + '</td>' +
                            '<td>' + supplier.rfc + '</td>' +
                            '<td>' + supplier.address + '</td>' +
                            '<td>' + supplier.city + '</td>' +
                            '<td>' + supplier.state + '</td>' +
                            '<td>' +
                                '<div class="avatar-group">';
                                supplier.contactEmails.forEach(function(email) {
                                    row += '<a class="avatar-group-item" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="' + email + '">' +
                                                '<img src="../assets/images/account-circle-custom-717171.png" alt="" class="rounded-circle avatar-xxs">' +
                                            '</a>';
                                });
                                row +=  '</div>' +
                                        '</td>' +
                                        '<td>' +
                                        '<div class="avatar-group">';
                                            supplier.contactPhones.forEach(function(phone) {
                                                row += '<a class="avatar-group-item" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="' + phone + '">' +
                                                            '<img src="../assets/images/phone-custom-717171.png" alt="" class="rounded-circle avatar-xxs">' +
                                                        '</a>';
                                            });
                                row +=  '</div>' +
                                        '</td>' +
                                        '<td><span class="badge rounded-pill badge-soft-' + supplier.statusColor + '">' + supplier.statusName + '</span></td>' +
                                        '<td class="text-center">' +
                                            '<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/supplier/update?id=' + supplier.id + '\'"><i class="ri-pencil-fill"></i></button>' +
                                        '</td>' +
                                    '</tr>';
                        tbody.append(row);
                    });
                }
                else {
                    $('#tbSuppliers tbody').html(
                        '<tr>' +
                            '<td colspan="10" class="text-center">We have nothing to show.</td>' +
                        '</tr>');
                }
                $('#dvTotalSuppliers').html('Total suppliers: ' + data.results.length);
            },
            error: function (xhr, status, error) {
                $('#tbSuppliers tbody').html(
                    '<tr>' +
                        '<td colspan="10" class="text-center">We have nothing to show.</td>' +
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
        $('#tbSuppliers tbody').html(
            '<tr>' +
                '<td colspan="10" class="text-center">We have nothing to show.</td>' +
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