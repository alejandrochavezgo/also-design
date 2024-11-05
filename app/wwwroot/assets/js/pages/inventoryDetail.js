function initializeTransactionTable() {
    try {
        console.log("entra");
        $('#dvTableTransactions').DataTable({
            "ajax": {
                "url": "getInventoryMovementsByPurchaseOrderIdAndInventoryItemId",
                "type": "get",
                "data": function(d) {
                    d.inventoryItemId = $('#inventoryItemId').val();
                },
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
                    return data.results;
                }
            },
            "columns": [
                { "data": "inventoryMovementTypeDescription" },
                { "data": "quantity" },
                { "data": "packingUnitTypeDescription" },
                { "data": "unitValue", "render": $.fn.dataTable.render.number(',', '.', 2, '$') },
                { "data": "totalValue", "render": $.fn.dataTable.render.number(',', '.', 2, '$') },
                { "data": "creatorUsername" },
                { "data": "comments" },
                { "data": "creationDate", "render": function(data) {
                    return data ? new Date(data).toLocaleDateString() : '-';
                }}
            ],
            "order": [[7, 'desc']]
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