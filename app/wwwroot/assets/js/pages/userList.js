function initializeDatatable() {
    try {
        $('#tbUsers').DataTable({
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
                    $('#dvTotalUsers').html('Total users: ' + data.results.length);
                    return data.results;
                }
            },
            "columns": [
                { "data": "id" },
                { "data": "email" },
                { "data": "username" },
                { "data": "firstname" },
                { "data": "lastname" },
                { "data": "statusName", "render": function(data, type, row) {
                    return '<span class="badge rounded-pill badge-soft-' + row.statusColor + '">' + data + '</span>';
                }},
                { "data": "creationDateAsString" },
                { "data": "modificationDateAsString", "render": function(data) {
                    return data === '-' ? '<span class="text-center">' + data + '</span>' : '<span class="text-left">' + data + '</span>';
                }},
                { "data": "id", "render": function(data, type, row) {
                    return `
                        <button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/user/update?id=${data}'" title="Update"><i class="ri-pencil-fill"></i></button>
                        <button type="button" class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal(${data})" title="Delete"><i class="ri-delete-bin-2-fill"></i></button>
                        <button type="button" class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/user/detail?id=${data}'" title="View"><i class="ri-eye-fill"></i></button>
                    `;
                }}
            ],
            "order": [[6, 'desc']]
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