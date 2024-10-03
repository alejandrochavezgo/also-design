$(document).ready(function () {
    try {
        $('#tbClients tbody').html(
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
                
                var tbody = $('#tbClients tbody');
                tbody.empty();
                if (data.results.length > 0) { 
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
                                    '<td><span class="badge rounded-pill badge-soft-' + client.statusColor + '">' + client.statusName + '</span></td>' +
                                    '<td class="text-center">' +
                                        '<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/client/update?id=' + client.id + '\'"><i class="ri-pencil-fill"></i></button>' +
                                    '</td>' +
                                '</tr>';
                        tbody.append(row);
                    });
                }
                else {
                    $('#tbClients tbody').html(
                        '<tr>' +
                            '<td colspan="10" class="text-center">We have nothing to show.</td>' +
                        '</tr>');
                }
                $('#dvTotalClients').html('Total clients: ' + data.results.length);
            },
            error: function (xhr, status, error) {
                $('#tbClients tbody').html(
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
        $('#tbClients tbody').html(
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