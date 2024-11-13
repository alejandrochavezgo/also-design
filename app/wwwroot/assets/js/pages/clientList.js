function showDeleteModal(clientId, status) {
    try {
        if (status != '1') {
            Swal.fire({
                title: 'Error!!',
                html: 'To delete a client, they must first be active.',
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: !1,
                footer: '',
                showCloseButton: !1
            });
            return;
        }

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
                        id: clientId,
                        status: status
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

function initializeDatatable() {
    try {
        $('#tbClients').DataTable({
            "scrollX": true,
            "autoWidth": false,
            "ajax": {
                "url": "getAll",
                "type": "get",
                "dataSrc": function(data) {
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
                        return [];
                    }
                    $('#dvTotalClients').html('Total clients: ' + data.results.length);
                    return data.results;
                }
            },
            "columns": [
                { "data": "id" },
                { "data": "businessName" },
                { "data": "rfc" },
                { "data": "city" },
                { "data": "state" },
                { 
                    "data": "contactEmails",
                    "render": function(emails) {
                        let emailHtml = '<div class="avatar-group">';
                        emails.forEach(function(email) {
                            emailHtml += `
                                <a class="avatar-group-item" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="${email}">
                                    <img src="../assets/images/account-circle-custom-717171.png" alt="" class="rounded-circle avatar-xxs">
                                </a>`;
                        });
                        emailHtml += '</div>';
                        return emailHtml;
                    }
                },
                { 
                    "data": "contactPhones",
                    "render": function(phones) {
                        let phoneHtml = '<div class="avatar-group">';
                        phones.forEach(function(phone) {
                            phoneHtml += `
                                <a class="avatar-group-item" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" title="${phone}">
                                    <img src="../assets/images/phone-custom-717171.png" alt="" class="rounded-circle avatar-xxs">
                                </a>`;
                        });
                        phoneHtml += '</div>';
                        return phoneHtml;
                    }
                },
                { 
                    "data": "statusName", 
                    "render": function(data, type, row) {
                        return `<span class="badge rounded-pill badge-soft-${row.statusColor}">${data}</span>`;
                    }
                },
                { 
                    "data": "id",
                    "render": function(data, type, row) {
                        return `
                            <button type="button" class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/client/detail?id=${data}'" title="Details"><i class="ri-eye-fill"></i></button>
                            <button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/client/update?id=${data}'" title="Update"><i class="ri-pencil-fill"></i></button>
                            <button type="button" class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal(${data}, '${row.status}')" title="Delete"><i class="ri-delete-bin-2-fill"></i></button>
                        `;
                    }
                }
            ],
            "order": [[0, 'asc']]
        }).on('xhr', function() {
            $('#loader').hide();
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
        $('#loader').hide();
    }
}

$(document).ready(function () {
    $('#loader').show();
    initializeDatatable();
});