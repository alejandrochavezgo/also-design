document.querySelectorAll('.uppercase-input').forEach(input => {
    input.addEventListener('input', function() {
        this.value = this.value.toUpperCase();
    });
});

function showDeleteModal(purchaseOrderId, status) {
    try {
        if (status != '1') {
            Swal.fire({
                title: 'Error!!',
                html: 'To delete a purchase order, it must first be active.',
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
                        id: purchaseOrderId,
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

function downloadPurchaseOrderByPurchaseOrderId(purchaseOrderId) {
    fetch(window.location.origin + '/purchaseOrder/downloadPurchaseOrderByPurchaseOrderId?id=' + purchaseOrderId, {
        method: 'get'
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`HTTP status ${response.status}`);
        }
        return response.blob();
    })
    .then(blob => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `PurchaseOrder_${purchaseOrderId}.pdf`;
        document.body.appendChild(a);
        a.click();
        a.remove();
        window.URL.revokeObjectURL(url);
    })
    .catch(error => {
        Swal.fire({
            title: 'Error!!',
            html: error.message,
            icon: 'error',
            confirmButtonClass: 'btn btn-danger w-xs mt-2',
            buttonsStyling: false,
            showCloseButton: true
        });
    });
}

async function showUpdateStatusModal(purchaseOrderId, status, statusName, statusColor) {
    try {
        $('#loader').show()
        $('#dvPurchaseOrderItemsTitle').hide();
        $('#dvPurchaseOrderItems').hide();
        $('#inCurrentStatus').val(statusName.toUpperCase());
        $('#inCurrentStatus').addClass(`bg-${statusColor}`);
        $('#inCurrentStatus').attr('purchaseOrderId', purchaseOrderId);
        $('#inCurrentStatus').attr('status', status);
        $('#taComments').val('');
        $('#seStatus').val('');

        const statusCatalogLoaded = await loadStatusCatalog(statusName);
        if (!statusCatalogLoaded) {
            $('#loader').hide();
            return;
        }

        if (status == '6' || status == '9') {
            const packingUnitTypeCatalog = await getPackingUnitTypeCatalog();
            if (!packingUnitTypeCatalog) {
                $('#loader').hide();
                return;
            }

            const purchaseOrderItemsLoaded = await getPurchaseOrderItems(purchaseOrderId, packingUnitTypeCatalog);
            if (!purchaseOrderItemsLoaded) {
                $('#loader').hide();
                return;
            }
        }

        $('#updateStatusModal').modal('show');
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

function initializeComponentListeners() {
    $('#seStatus').change(function() {
        if ($(this).val() == '9') {
            $('#dvPurchaseOrderItemsTitle').show();
            $('#dvPurchaseOrderItems').show();
        } else {
            $('#dvPurchaseOrderItemsTitle').hide();
            $('#dvPurchaseOrderItems').hide();
        }
    });
}

async function getPackingUnitTypeCatalog() {
    try {
        const response = await fetch(window.location.origin + '/inventory/getcCatalogByName?name=PACKINGUNITTYPES');
        if (!response ||!response.ok) {
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

        const catalogPackingTypes = await response.json();
        if (!catalogPackingTypes || !catalogPackingTypes.isSuccess || catalogPackingTypes.results.length == 0) {
            Swal.fire({
                title: 'Error!!',
                html: 'Packing types catalog not downloaded. Please reload the page.',
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
            return;
        }

        return catalogPackingTypes;
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

async function getPurchaseOrderItems(purchaseOrderId, packingUnitTypeCatalog) {
    try {
        const response = await fetch('getPurchaseOrderItemsByPurchaseOrderId?id=' + purchaseOrderId);
        if (!response ||!response.ok) {
            Swal.fire({
                title: 'Error!!',
                html: `HTTP error! Status: ${response.status}`,
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
            return false;
        }

        const purchaseOrderItems = await response.json();
        if (!purchaseOrderItems || !purchaseOrderItems.isSuccess || purchaseOrderItems.results.length == 0) {
            Swal.fire({
                title: 'Error!!',
                html: 'Purchase order items not downloaded. Please reload the page.',
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
            return false;
        }

        const container = document.getElementById('dvPurchaseOrderItems');
        container.innerHTML = '';
        if (purchaseOrderItems.results <= 0)
            return false;

        const packingUnitTypesOptions = packingUnitTypeCatalog.results.map(unit => {
            return `<option value="${unit.id}">${unit.description}</option>`;
        }).join('');

        let purchaseOrderItemsHtml = '';
        purchaseOrderItems.results.forEach(purchaseOrderItem => {
            purchaseOrderItemsHtml += `
                <div class="row pl-1x05r pr-1x05r" inventoryitemid="${purchaseOrderItem.inventoryItemId}">
                    <div class="col-12 pb-05r">
                        <span class="badge badge-soft-dark badge-border purchaseorder-item-title">${purchaseOrderItem.material}</span>
                    </div>
                    <div class="col-3">
                        <label class="form-label">Unit</label>
                        <select class="form-select purchaseorder-item-unittype">
                            <option value="">Select option</option>
                            ${packingUnitTypesOptions}
                        </select>
                    </div>
                    <div class="col-3">
                        <label class="form-label">Qty</label>
                        <input type="text" class="form-control numerical-mask purchaseorder-item-qty" value="${purchaseOrderItem.quantity}">
                    </div>
                    <div class="col-3">
                        <label class="form-label">$ U. Value</label>
                        <input type="text" class="form-control numerical-mask purchaseorder-item-unitprice" value="${purchaseOrderItem.unitValue}">
                    </div>
                    <div class="col-3">
                        <label class="form-label">$ Total</label>
                        <input type="text" class="form-control numerical-mask purchaseorder-item-total" value="${purchaseOrderItem.totalValue}">
                    </div>
                </div>
                <div class="row pl-1x05r pr-1x05r pt-1-5r"></div>
            `;
        });
        container.innerHTML += purchaseOrderItemsHtml;
        const selectElements = container.querySelectorAll('.purchaseorder-item-unittype');
        if (selectElements.length > 0) {
            selectElements.forEach((selectElement, index) => {
                selectElement.value = purchaseOrderItems.results[index].unit;
            });
        }
        initializeInputNumericalMasks();
        return true;
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

async function loadStatusCatalog(statusName) {
    try {
        let optionsToShow = [];
        switch (statusName.toUpperCase()) {
            case 'ACTIVE':
                optionsToShow = ['PENDING'];
                break;
            case 'PENDING':
                optionsToShow = ['APPROVED', 'REJECTED'];
                break;
            case 'APPROVED':
                optionsToShow = ['CANCELLED', 'PARTIALLY FULFILLED', 'FULFILLED'];
                break;
            case 'PARTIALLY FULFILLED':
                optionsToShow = ['PARTIALLY FULFILLED', 'FULFILLED', 'CLOSED'];
                break;
            case 'CANCELLED':
            case 'FULFILLED':
            case 'REJECTED':
                optionsToShow = ['CLOSED'];
                break;
            case 'CLOSED':
            default:
                optionsToShow = [];
        }

        $('#seStatus').empty().append('<option value="">Select option</option>');
        optionsToShow.forEach(status => {
            const value = getValueForStatus(status);
            $('#seStatus').append(`<option value="${value}">${status}</option>`);
        });
        return true;
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

function updateStatus()
{
    try {
        $('#loader').show();
        const purchaseOrderItems = [];
        $('#dvPurchaseOrderItems .row[inventoryitemid]').each(function() {
            const inventoryItemId = $(this).attr('inventoryitemid');
            const unit = $(this).find('.purchaseorder-item-unittype').val();
            const quantity = $(this).find('.purchaseorder-item-qty').val();
            const unitValue = $(this).find('.purchaseorder-item-unitprice').val();
            const totalValue = $(this).find('.purchaseorder-item-total').val();
            purchaseOrderItems.push({
                inventoryItemId,
                unit,
                quantity,
                unitValue,
                totalValue
            });
        });

        if(!isValidForm(purchaseOrderItems)) {
            $('#loader').hide();
            return;
        }

        fetch('updateStatusByPurchaseOrderId', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                purchaseOrderId: $('#inCurrentStatus').attr('purchaseOrderId'),
                currentStatusId: $('#inCurrentStatus').attr('status'),
                newStatusId: $('#seStatus').val(),
                comments: $('#taComments').val(),
                purchaseOrderItems: purchaseOrderItems
            })
        })
        .then(response => { return response.json(); })
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

function isValidForm(purchaseOrderItems) {
    try {
        var currentStatusId = $('#inCurrentStatus').attr('status')
        var purchaseOrderId = $('#inCurrentStatus').attr('purchaseOrderId');
        var currentStatusName = $('#inCurrentStatus').val();
        var newStatusId = $('#seStatus').val();
        var newStatusName = $('#seStatus option:selected').text();
        var comments = $('#taComments').val();
        if (!currentStatusId || !purchaseOrderId || !currentStatusName || !newStatusId || !comments || (currentStatusName == newStatusName && (currentStatusName != 'PARTIALLY FULFILLED' && newStatusName != 'PARTIALLY FULFILLED')) || (purchaseOrderItems.length == 0 && currentStatusId == '9')) {
            Swal.fire({
                title: 'Error!!',
                html: 'The Status and Comments fields cannot be empty. And the new status must be different from the current except when is PARTIALLY FULFILLED.',
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
            buttonsStyling: !1,
            footer: '',
            showCloseButton: !1
        });
    }
}

function getValueForStatus(status) {
    switch (status) {
        case 'ACTIVE':
            return '1';
        case 'APPROVED':
            return '6';
        case 'PENDING':
            return '7';
        case 'REJECTED':
            return '8';
        case 'PARTIALLY FULFILLED':
            return '9';
        case 'FULFILLED':
            return '10';
        case 'CLOSED':
            return '11';
        case 'CANCELLED':
            return '12';
        default:
            return '';
    }
}

function initializeDatatable() {
    try {
        $('#tbPurchaseOrders').DataTable({
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
                    $('#dvTotalPurchaseOrders').html('Total purchase orders: ' + data.results.length);
                    return data.results;
                }
            },
            "columns": [
                { 
                    "data": "code",
                    "render": function(data) {
                        return `<span class="badge bg-dark">${data}</span>`;
                    }
                },
                { "data": "supplier.businessName" },
                { "data": "user.username" },
                { "data": "payment.description" },
                { 
                    "data": "totalAmount",
                    "render": function(data) {
                        return `$${parseFloat(data).toFixed(2)}`;
                    }
                },
                { "data": "currency.description" },
                { 
                    "data": "statusName",
                    "render": function(data, type, row) {
                        return `<span class="badge rounded-pill badge-soft-${row.statusColor}">${data}</span>`;
                    }
                },
                { "data": "creationDateAsString" },
                { "data": "id", "render": function(data, type, row) {
                    return `
                        <button type="button" class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/purchaseOrder/detail?id=${data}'" title="View"><i class="ri-eye-fill"></i></button>
                        ${row.status == 1 ? `<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/purchaseOrder/update?id=${data}'" title="Update"><i class="ri-pencil-fill"></i></button>` : ''}
                        ${row.status != 11 ? `<button type="button" class="btn btn-primary btn-icon secondary waves-effect waves-light mx-1" onclick="showUpdateStatusModal(${data}, '${row.status}', '${row.statusName}', '${row.statusColor}')" title="Update Status"><i class="ri-exchange-fill"></i></button>` : ''}
                        ${row.status == 1 ? `<button type="button" class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal(${data}, ${row.status})" title="Delete"><i class="ri-delete-bin-2-fill"></i></button>` : ''}
                    `;
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

$(document).ready(function() {
    $('#loader').show();
    initializeDatatable();
    initializeInputNumericalMasks();
    initializeComponentListeners();
});