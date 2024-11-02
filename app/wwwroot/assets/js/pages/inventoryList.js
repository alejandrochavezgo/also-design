function initializeDatatable() {
    try {
        $('#loader').show();
        $('#dvTableItems').DataTable({
            "ajax": {
                "url": "getAll",
                "type": "get",
                "dataSrc": function (data) {
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
                    $('#dvTotalItems').html('Total items: ' + data.results.length);
                    return data.results.map(item => ({
                        itemName: item.itemName,
                        itemCode: item.itemCode,
                        stock: item.idQuantityLastRestockDate.split('&')[1],
                        lastRestockDate: item.idQuantityLastRestockDate.split('&')[2],
                        itemImagePath: item.itemImagePath,
                        itemId: item.idQuantityLastRestockDate.split('&')[0]
                    }));
                }
            },
            "columns": [
                {
                    "data": "itemName",
                    "render": function (data, type, row) {
                        return `<div class="d-flex align-items-center">
                            <div class="flex-shrink-0 me-3">
                                <div class="avatar-sm bg-light rounded p-1">
                                    <img src="${row.itemImagePath}" height="40" alt="" class="img-fluid d-block">
                                </div>
                            </div>
                            <div class="flex-grow-1">
                                <h5 class="fs-14 mb-1">
                                    <a href="#" class="text-dark">${data}</a>
                                </h5>
                                <p class="text-muted mb-0">Code: <span class="fw-medium">${row.itemCode}</span></p>
                            </div>
                        </div>`;
                    }
                },
                { "data": "stock", "title": "Stock" },
                { "data": "lastRestockDate", "title": "Last Restock" },
                {
                    "data": "itemId",
                    "render": function (data, type, row) {
                        return `
                            <button class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/inventory/details?id=${data}'" title="Details"><i class="ri-eye-fill"></i></button>
                            <button class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/inventory/update?id=${data}'" title="Update"><i class="ri-pencil-fill"></i></button>
                            <button class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal('${data}')" title="Delete"><i class="ri-delete-bin-fill"></i></button>`;
                    },
                    "orderable": false,
                    "title": "Actions"
                }
            ],
            "order": [[2, 'desc']],
        }).on('xhr', function () {
            $('#loader').hide();
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
        $('#loader').hide();
    }
}

$(document).ready(function () {
    initializeDatatable();
});
