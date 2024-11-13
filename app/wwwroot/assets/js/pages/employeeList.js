function initializeDatatable() {
    try {
        $('#tbEmployees').DataTable({
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
                    $('#dvTotalEmployees').html('Total employees: ' + data.results.length);
                    return data.results;
                }
            },
            "columns": [
                {
                    "data": "employee.code",
                    "render": function(data) {
                        return `<span class="badge bg-dark">${data}</span>`;
                    }
                },
                { "data": "email" },
                { "data": "firstname" },
                { "data": "lastname" },
                { "data": "statusName", "render": function(data, type, row) { return '<span class="badge rounded-pill badge-soft-' + row.statusColor + '">' + data + '</span>'; } },
                { "data": "creationDateAsString" },
                { "data": "modificationDateAsString", "render": function(data) { return data === '-' ? '<span class="text-center">' + data + '</span>' : '<span class="text-left">' + data + '</span>'; } },
                { "data": "id", "render": function(data) {
                    return `
                    <button type="button" class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/employee/detail?id=${data}'" title="View"><i class="ri-eye-fill"></i></button>
                        <button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/employee/update?id=${data}'" title="Update"><i class="ri-pencil-fill"></i></button>
                        <button type="button" class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal(${data})" title="Delete"><i class="ri-delete-bin-2-fill"></i></button>
                    `;
                }}
            ],
            "order": [[5, 'desc']]
        }).on('xhr', function() {
            $('#loader').hide();
        });
    } catch(exception) {
        $('#tbEmployees tbody').html(
            '<tr>' +
                '<td colspan="8" class="text-center">We have nothing to show.</td>' +
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
}

$(document).ready(function () {
    $('#loader').show();
    initializeDatatable();
});