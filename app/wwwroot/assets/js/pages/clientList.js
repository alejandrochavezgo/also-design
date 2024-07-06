$(document).ready(function () {
    try {
        $.ajax({
            url: 'getClients',
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

                var tbody = $('#tbClients tbody');
                tbody.empty();
                data.results.forEach(function (client) {
                    var row =
                        '<tr>' +
                        '<td>' + client.id + '</td>' +
                        '<td>' + client.businessName + '</td>' +
                        '<td>' + client.rfc + '</td>' +
                        '<td>' + client.address + '</td>' +
                        '<td>' + client.city + '</td>' +
                        '<td>' + client.state + '</td>' +
                        '<td>' +
                            '<div class="avatar-group">';

                    client.contactEmails.forEach(function(email) {
                        row += '<a class="avatar-group-item" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="' + email + '">' +
                                    '<img src="../assets/images/account-circle-custom-717171.png" alt="" class="rounded-circle avatar-xxs">' +
                                '</a>';
                    });
                    row +=      '</div>' +
                            '</td>' + 
                            '<td>' +
                                '<div class="avatar-group">';
                    client.contactPhones.forEach(function(phone) {
                        row += '<a class="avatar-group-item" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="' + phone + '">' +
                                    '<img src="../assets/images/phone-custom-717171.png" alt="" class="rounded-circle avatar-xxs">' +
                                '</a>';
                    });
                        row +=      '</div>' +
                                '</td>' +
                                '<td><span class="badge rounded-pill badge-soft-' + (client.isActive ? 'success' : 'danger') + '">' + (client.isActive ? 'Active' : 'Inactive') + '</span></td>' +
                                '<td class="text-center">' +
                                    '<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/client/update?clientId=' + client.id + '\'"><i class="ri-pencil-fill"></i></button>' +
                                '</td>' +
                            '</tr>';
                    tbody.append(row);
                });
                $('#dvTotalClients').html('Total clients: ' + data.results.length);
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