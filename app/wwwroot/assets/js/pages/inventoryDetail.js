function initializeTransactionTable() {
    try {
        $('#dvTableTransactions').DataTable({
            "scrollX": true,
            "autoWidth": false,
            "ajax": {
                "url": "getInventoryMovementsByInventoryItemId",
                "type": "get",
                "data": function(d) {
                    d.inventoryItemId = $('#inventoryItemId').val();
                },
                "dataSrc": function(data) {
                    console.log(data);
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
                    return data.results;
                }
            },
            "columns": [
                { "data": "inventoryMovementTypeDescription" },
                { "data": "quantity" },
                { "data": "packingUnitTypeDescription" },
                { "data": "unitValue", "render": $.fn.dataTable.render.number(',', '.', 2, '$') },
                { "data": "totalValue", "render": $.fn.dataTable.render.number(',', '.', 2, '$') },
                { "data": "approvedDeliveredUsername" },
                { "data": "receivedUsername" },
                { "data": "projectName" },
                { "data": "comments" },
                { "data": "creationDate", "render": function(data) {
                    if (data) {
                        const date = new Date(data);
                        const year = date.getFullYear();
                        const month = String(date.getMonth() + 1).padStart(2, '0');
                        const day = String(date.getDate()).padStart(2, '0');
                        const hours = String(date.getHours()).padStart(2, '0');
                        const minutes = String(date.getMinutes()).padStart(2, '0');
                        const seconds = String(date.getSeconds()).padStart(2, '0');
                        return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
                    }
                    return '-';
                }}
            ],
            "order": [[9, 'desc']]
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
    initializeTransactionTable();
});