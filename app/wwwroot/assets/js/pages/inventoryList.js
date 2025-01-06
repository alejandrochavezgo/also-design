function showDeleteModal(inventoryItemId, stock) {
    try {
        if (parseFloat(stock) > 0) {
            Swal.fire({
                title: 'Error!!',
                html: 'The stock must be 0.00',
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: !1,
                footer: '',
                showCloseButton: !1
            });
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
                        id: inventoryItemId,
                        quantity: stock
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

async function showInventoryReleaseModal(inventoryItemId, stock) {
    try {
        $('#loader').show()
        $('#inReceivingUser').val('');
        $('#inReceivingUser').removeAttr('userId');
        $('#inProjectName').val('');
        $('#inProjectName').removeAttr('projectId');
        $('#inQuantityToRelease').val('');
        $('#inQuantityToRelease').removeAttr('stock');
        $('#inQuantityToRelease').removeAttr('inventoryItemId');
        $('#taComments').val('');
        
        const inventoryItem = await getInventoryItem(inventoryItemId);
        if (!inventoryItem) {
            $('#loader').hide();
            return;
        }

        if (parseFloat(inventoryItem.results.quantity).toFixed(2) != parseFloat(stock).toFixed(2)) {
            Swal.fire({
                title: 'Error!!',
                html: 'The quantity in the database is different from the one currently being displayed. Please refresh the page to get an updated stock.',
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
            $('#loader').hide();
        }

        $('#inQuantityToRelease').attr('stock', stock);
        $('#inQuantityToRelease').attr('inventoryItemId', inventoryItemId);
        $('#inventoryReleaseModal').modal('show');
        $('#loader').hide();
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

async function getInventoryItem(inventoryItemId) {
    try {
        const response = await fetch(window.location.origin + '/inventory/getItemInventoryById?id=' + inventoryItemId);
        if (!response || !response.ok) {
            Swal.fire({
                title: 'Error!!',
                html: `HTTP error! Status: ${response.status}`,
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
            return;
        }

        const inventoryItem = await response.json();
        if (!inventoryItem || !inventoryItem.isSuccess || !inventoryItem.results) {
            Swal.fire({
                title: 'Error!!',
                html: inventoryItem.message,
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
            return;
        }

        return inventoryItem;
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
        return false;
    }
}

function initializeUserAutocomplete() {
    try {
        $('#inReceivingUser').autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: '/user/getUsersByTerm',
                    method: 'get',
                    dataType: 'json',
                    data: {
                        username: request.term
                    },
                    success: function(data) {
                        response($.map(data, function(item) {
                            return {
                                label: item.username,
                                value: item.username,
                                id: item.id,
                                username: item.item
                            };
                        }));
                    },
                    error: function(error) {
                        Swal.fire({
                            title: 'Error!!',
                            html: error,
                            icon: 'error',
                            confirmButtonClass: 'btn btn-danger w-xs mt-2',
                            buttonsStyling: !1,
                            footer: '',
                            showCloseButton:!1
                        });
                    }
                });
            },
            appendTo: '#inventoryReleaseModal',
            minLength: 2,
            select: function(event, ui) {
                $('#inReceivingUser').val(ui.item.id);
                $('#inReceivingUser').attr('receivingUserId', ui.item.id);
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
}

function initializeProjectAutocomplete() {
    try {
        $('#inProjectName').autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: '/project/getProjectsByTerm',
                    method: 'get',
                    dataType: 'json',
                    data: {
                        name: request.term
                    },
                    success: function(data) {
                        response($.map(data, function(item) {
                            return {
                                label: item.name,
                                value: item.name,
                                id: item.id,
                                businessName: item.item
                            };
                        }));
                    },
                    error: function(error) {
                        Swal.fire({
                            title: 'Error!!',
                            html: error,
                            icon: 'error',
                            confirmButtonClass: 'btn btn-danger w-xs mt-2',
                            buttonsStyling: !1,
                            footer: '',
                            showCloseButton:!1
                        });
                    }
                });
            },
            appendTo: '#inventoryReleaseModal',
            minLength: 2,
            select: function(event, ui) {
                $('#inProjectName').val(ui.item.id);
                $('#inProjectName').attr('projectId', ui.item.id);
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
}

function initializeInputNumericalMasks()
{
    try {
        $('.numerical-mask').on('input', function() {
            var value = $(this).val();
            var valid = /^\d*\.?\d*$/.test(value);
            if (!valid) {
                $(this).val(value.slice(0, -1));
            } else {
                var numValue = parseFloat(value);
                if (numValue < 0) {
                    $(this).val('0');
                }
            }
        });

        $('.numerical-mask').on('blur', function() {
            var value = $(this).val();
            if (value === '' || isNaN(parseFloat(value))) {
                $(this).val('0.00');
            } else {
                $(this).val(parseFloat(value).toFixed(2));
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
}

function initializeDatatable() {
    try {
        $('#loader').show();
        $('#dvTableItems').DataTable({
            "scrollX": true,
            "autoWidth": false,
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
                            <button class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/inventory/detail?id=${data}'" title="Details"><i class="ri-eye-fill"></i></button>
                            <button class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/inventory/update?id=${data}'" title="Update"><i class="ri-pencil-fill"></i></button>
                            <button class="btn btn-warning btn-icon waves-effect waves-light mx-1" onclick="showInventoryReleaseModal('${data}', '${row.stock}')" title="Release"><i class="ri-inbox-archive-fill"></i></button>
                            <button class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal('${data}', '${row.stock}')" title="Delete"><i class="ri-delete-bin-fill"></i></button>`;
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

function inventoryRelease()
{
    try {
        $('#loader').show();
        const inventoryItemToRelase = {
            id: parseInt($('#inQuantityToRelease').attr('inventoryItemId'), 10),
            stock: parseFloat($('#inQuantityToRelease').attr('stock'), 10), // O usa parseFloat si necesitas preservar decimales
            receivingUserId: parseInt($('#inReceivingUser').attr('receivingUserId'), 10),
            deliveringUserId: parseInt($('#inDeliveringUser').attr('deliveringUserId'), 10),
            projectId: parseInt($('#inProjectName').attr('projectId'), 10),
            quantityToRelease: parseFloat($('#inQuantityToRelease').val(), 10), // O usa parseFloat si necesitas decimales
            comments: $('#taComments').val() // No requiere conversión
        };

        console.log(inventoryItemToRelase);

        if(!isValidForm(inventoryItemToRelase)) {
            $('#loader').hide();
            return;
        }

        fetch('updateInventoryStockByInventoryItemId', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(inventoryItemToRelase)
        })
        .then(response => {return response.json()})
        .then(data => {
            if(!data.isSuccess) {
                Swal.fire({
                    title: 'Error!!',
                    html: data.message,
                    icon: 'error',
                    confirmButtonClass: 'btn btn-danger w-xs mt-2',
                    buttonsStyling: !1,
                    footer: '',
                    showCloseButton:!1
                });
                $('#loader').hide();
                return;
            }

            Swal.fire({
                title: 'Succcess',
                html: data.message,
                icon: 'success',
                confirmButtonClass: 'btn btn-success w-xs mt-2',
                buttonsStyling: !1,
                footer: '',
                showCloseButton:!1
            }).then(function(t) {
                window.location.href = 'list';
            });
            $('#loader').hide();
        })
        .catch(error => {
            Swal.fire({
                title: 'Error!!',
                html: error,
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: !1,
                footer: '',
                showCloseButton:!1
            });
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
    }
}

function isValidForm(inventoryItemToRelease) {
    try {
        if (!inventoryItemToRelease.id || 
            !inventoryItemToRelease.receivingUserId || 
            !inventoryItemToRelease.deliveringUserId || 
            !inventoryItemToRelease.projectId || 
            !inventoryItemToRelease.quantityToRelease || 
            inventoryItemToRelease.stock <= 0 || 
            inventoryItemToRelease.quantityToRelease <= 0 || 
            inventoryItemToRelease.quantityToRelease > inventoryItemToRelease.stock) {
            Swal.fire({
                title: 'Error!!',
                html: 'The Receiving User, Delivering User, Project Name, and Quantity to Release cannot be empty. The quantity to request must be less than or equal to the quantity in stock.',
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
            return false;
        }

        return true;
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
        return false;
    }
}

$(document).ready(function () {
    initializeDatatable();
    initializeUserAutocomplete();
    initializeProjectAutocomplete();
    initializeInputNumericalMasks();
});