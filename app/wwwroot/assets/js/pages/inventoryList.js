$(document).ready(function () {
    try {
        $('#loader').show();
        $.ajax({
            url: 'getAll',
            type: 'get',
            dataType: 'json',
            success: function (data) {
                $('#dvTotalItems').html('Total items: ' + data.results.length);
                const payload = data.results.map(item => [
                    item.itemName,
                    item.itemCode,
                    item.idQuantityLastRestockDate,
                    item.itemImagePath
                ]);
                new gridjs.Grid({
                    columns: [
                        { id: 'itemName', name: 'Item', formatter: (cell, row) => {
                            return gridjs.html(
                                `<div class="d-flex align-items-center">
                                    <div class="flex-shrink-0 me-3">
                                        <div class="avatar-sm bg-light rounded p-1">
                                            <img src="${row.cells[3].data}" height="40" alt="" class="img-fluid d-block">
                                        </div>
                                    </div>
                                    <div class="flex-grow-1">
                                        <h5 class="fs-14 mb-1">
                                            <a href="#" class="text-dark">${row.cells[0].data}</a>
                                        </h5>
                                        <p class="text-muted mb-0">Code: <span class="fw-medium">${row.cells[1].data}</span></p>
                                    </div>
                                </div>`
                            );
                        }},
                        { id: 'quantity', name: 'Stock', formatter: (cell, row) => {
                            return gridjs.html(
                                `${row.cells[2].data.split('&')[1]}`
                            );
                        }},
                        { id: 'lastRestockDate', name: 'Last Restock', formatter: (cell, row) => {
                            return gridjs.html(
                                `${row.cells[2].data.split('&')[2]}`
                            );
                        }},
                        { id: 'actions', name: 'Actions', formatter: (cell, row) => {
                            return gridjs.html(`
                                <button class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/inventory/details?id=' + ${row.cells[2].data.split('&')[0]} + '\'" title="Details"><i class="ri-eye-fill"></i></button>
                                <button class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/inventory/update?id=' + ${row.cells[2].data.split('&')[0]} + '\'" title="Update"><i class="ri-pencil-fill"></i></button>
                                <button class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal(' + ${row.cells[2].data.split('&')[0]} + ')" title="Delete"><i class="ri-delete-bin-fill"></i></button>
                            `);
                        }}
                    ],
                    data: payload,
                    pagination: {
                        enabled: true,
                        limit: 20,
                    },
                    search: true,
                    sort: true
                }).render(document.getElementById("dvTableItems"));
                $('#loader').hide();
            },
            error: function (error) {
                Swal.fire({
                    title: 'Error!!',
                    text: 'Data could not be loaded.',
                    icon: 'error',
                    confirmButtonClass: 'btn btn-danger w-xs mt-2',
                    buttonsStyling: false,
                    footer: '',
                    showCloseButton: true
                });
                $('#loader').hide();
            }
        });
    } catch (error) {
        Swal.fire({
            title: 'Error!!',
            text: 'Data could not be loaded.',
            icon: 'error',
            confirmButtonClass: 'btn btn-danger w-xs mt-2',
            buttonsStyling: false,
            footer: '',
            showCloseButton: true
        });
        $('#loader').hide();
    }
});
