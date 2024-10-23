function showDeleteModal(clientId) {
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
                        id: clientId
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
        $('#tbClients tbody').html(
            '<tr>' +
                '<td colspan="9" class="text-center">Rendering results... <img width="30" src="../assets/images/infinity.gif"></td>' +
            '</tr>');

        $.ajax({
            url: 'getAll',
            method: 'get',
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
                                    '<button type="button" class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/client/detail?id=' + client.id + '\'" title="Details"><i class="ri-eye-fill"></i></button>' +
                                        '<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/client/update?id=' + client.id + '\'" title="Update"><i class="ri-pencil-fill"></i></button>' +
                                        '<button type="button" class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal(' + client.id  + ')" title="Delete"><i class="ri-delete-bin-2-fill"></i></button>' +
                                    '</td>' +
                                '</tr>';
                        tbody.append(row);
                    });
                } else {
                    $('#tbClients tbody').html(
                        '<tr>' +
                            '<td colspan="9" class="text-center">We have nothing to show.</td>' +
                        '</tr>');
                }
                $('#dvTotalClients').html('Total clients: ' + data.results.length);
            },
            error: function (xhr, status, error) {
                $('#tbClients tbody').html(
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
        $('#tbClients tbody').html(
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