function showDeleteModal(quotationId, status) {
    try {
        if (status != '1') {
            Swal.fire({
                title: 'Error!!',
                html: 'To delete a quotations, it must first be active.',
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
                        id: quotationId,
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

function downloadQuotation(quotationId)
{
    try {
        alert('This feature is not available yet.');
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

async function showUpdateStatusModal(quotationId, status, statusName, statusColor) {
    try {
        $('#loader').show()
        $('#inCurrentStatus').val(statusName.toUpperCase());
        $('#inCurrentStatus').addClass(`bg-${statusColor}`);
        $('#inCurrentStatus').attr('quotationId', quotationId);
        $('#inCurrentStatus').attr('status', status);
        $('#taComments').val('');
        $('#seStatus').val('');

        const statusCatalogLoaded = await loadStatusCatalog(statusName);
        if (!statusCatalogLoaded) {
            $('#loader').hide();
            return;
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

async function loadStatusCatalog(statusName) {
    try {
        let optionsToShow = [];
        switch (statusName.toUpperCase()) {
            case 'ACTIVE':
                optionsToShow = ['PENDING'];
                break;
            case 'PENDING':
                optionsToShow = ['APPROVED', 'REJECTED', 'EXPIRED', 'CANCELLED'];
                break;
            case 'APPROVED':
                optionsToShow = ['LINKED', 'CANCELLED', 'CLOSED'];
                break;
            case 'REJECTED':
            case 'EXPIRED':
            case 'CANCELLED':
                optionsToShow = ['CLOSED'];
                break;
            case 'CLOSED':
            case 'LINKED':
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

function getValueForStatus(status) {
    switch (status) {
        case 'ACTIVE':
            return '1';
        case 'LINKED':
            return '4';
        case 'APPROVED':
            return '6';
        case 'PENDING':
            return '7';
        case 'REJECTED':
            return '8';
        case 'CLOSED':
            return '11';
        case 'CANCELLED':
            return '12';
        case 'EXPIRED':
            return '14';
        default:
            return '';
    }
}

function updateStatus()
{
    try {
        $('#loader').show();

        if(!isValidForm()) {
            $('#loader').hide();
            return;
        }

        fetch('updateStatusByQuotationId', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                quotationId: $('#inCurrentStatus').attr('quotationId'),
                currentStatusId: $('#inCurrentStatus').attr('status'),
                newStatusId: $('#seStatus').val(),
                comments: $('#taComments').val()
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

function isValidForm() {
    try {
        var currentStatusId = $('#inCurrentStatus').attr('status')
        var quotationId = $('#inCurrentStatus').attr('quotationId');
        var currentStatusName = $('#inCurrentStatus').val();
        var newStatusId = $('#seStatus').val();
        var newStatusName = $('#seStatus option:selected').text();
        var comments = $('#taComments').val();
        if (!currentStatusId || !quotationId || !currentStatusName || !newStatusId || !comments || (currentStatusName == newStatusName)) {
            Swal.fire({
                title: 'Error!!',
                html: 'The Status and Comments fields cannot be empty.',
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

function initializeQuotationsDatatable() {
    try {
        $('#tbQuotations').DataTable({
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
                    $('#dvTotalQuotations').html('Total quotations: ' + data.results.length);
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
                { "data": "client.businessName" },
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
                { 
                    "data": "id",
                    "render": function(data, type, row) {
                        return `
                            <button type="button" class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/quotation/detail?id=${data}'" title="View"><i class="ri-eye-fill"></i></button>
                            ${row.status == 1 ? `<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href='/quotation/update?id=${data}'" title="Update"><i class="ri-pencil-fill"></i></button>` : ''}
                            ${row.status != 11 && row.status != 4 ? `<button type="button" class="btn btn-primary btn-icon secondary waves-effect waves-light mx-1" onclick="showUpdateStatusModal(${data}, '${row.status}', '${row.statusName}', '${row.statusColor}')" title="Update Status"><i class="ri-exchange-fill"></i></button>` : ''}
                            <button type="button" class="btn btn-info btn-icon waves-effect waves-light mx-1" onclick="downloadQuotation(${data})" title="Download"><i class="ri-file-download-fill"></i></button>
                            ${row.status == 1 ? `<button type="button" class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal(${data}, ${row.status})" title="Delete"><i class="ri-delete-bin-2-fill"></i></button>` : ''}
                        `;
                    }
                }
            ],
            "order": [[0, 'asc']]
        }).on('xhr', function() {
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
    $('#loader').show();
    initializeQuotationsDatatable();
});
