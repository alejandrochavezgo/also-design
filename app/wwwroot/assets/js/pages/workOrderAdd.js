function initializeUpperCaseInput() {
    document.querySelectorAll('.uppercase-input').forEach(input => {
        input.addEventListener('input', function() {
            this.value = this.value.toUpperCase();
        });
    });
}

function initializeQuotationAutocomplete() {
    try {
        $('#inQuotationCode').autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: '/quotation/getQuotationsByTerm',
                    method: 'get',
                    dataType: 'json',
                    data: {
                        code: request.term
                    },
                    success: function(data) {
                        response($.map(data, function(item) {
                            return {
                                label: item.code,
                                value: item.code,
                                id: item.id,
                                code: item.code,
                                projectName: item.projectName,
                                creationDateAsString: item.creationDateAsString,
                                clientBusinessName: item.client.businessName,
                                paymentType: item.payment.description,
                                currencyType: item.currency.description,
                                items: item.items,
                                subtotal: parseFloat(item.subtotal).toFixed(2),
                                tax: parseFloat(item.taxAmount).toFixed(2),
                                total: parseFloat(item.totalAmount).toFixed(2)
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
            minLength: 2,
            select: function(event, ui) {
                $('#inQuotationId').val(ui.item.id);
                $('#pProjectName').text(ui.item.projectName);
                $('#pCreationDate').text(ui.item.creationDateAsString);
                $('#pCode').text(ui.item.code);
                $('#pCode').text(ui.item.code);
                $('#pClient').text(ui.item.clientBusinessName);
                $('#pPaymentType').text(ui.item.paymentType);
                $('#pCurrencyType').text(ui.item.currencyType);
                $('#pSubtotal').text('$' + ui.item.subtotal);
                $('#pTax').text('$' + ui.item.tax);
                $('#pTotal').text('$' + ui.item.total);
                loadQuotationItems(ui.item.items);
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

function loadQuotationItems(items)
{
    try {
        $('#tbQuotationItems').empty();
        items.forEach((item, index) => {
            const row = `
                <tr>
                    <td class="width-0per"></td>
                    <td>${index + 1}</td>
                    <td class="text-start">
                        <p class="form-control fw-medium mb-1 bg-light border-0">${item.description}</p>
                        <p class="form-control fw-medium mb-1 bg-light border-0">${item.material}</p>
                        <p class="form-control fw-medium mb-1 bg-light border-0">${item.details}</p>
                        <p class="form-control fw-medium bg-light border-0">${item.notes}</p>
                    </td>
                    <td>${item.quantity}</td>
                    <td>${item.unitDescription}</td>
                    <td class="text-end">$${item.unitValue.toFixed(2)}</td>
                    <td class="text-end">$${item.totalValue.toFixed(2)}</td>
                </tr>
            `;
            $('#tbQuotationItems').append(row);
        });
    } catch (error) {
        Swal.fire({
            title: 'Error!!',
            html: error,
            icon: 'error',
            confirmButtonClass: 'btn btn-danger w-xs mt-2',
            buttonsStyling: false,
            footer: '',
            showCloseButton: true
        });
    }
}

$(document).on('click', '#addItem', function() {
    try {
        var rowCounter = $('#tbWorkOrderItems tr').length + 1;
        var newRow = '' +
            '<tr>' +
                '<td class="width-0per">' +
                    '<button id="addItem" type="button" class="btn btn-success btn-sm btn-icon waves-effect waves-light"><i class="ri-add-circle-fill"></i></button>' +
                    '<span> </span>' +
                    '<button id="removeItem" type="button" class="btn btn-danger btn-sm btn-icon waves-effect waves-light"><i class="ri-close-circle-fill"></i></button>' +
                '</td>' +
                '<td scope="row">' +
                    '<span id="rowCount">' +
                        rowCounter +
                    '</span>' +
                '</td>' +
                '<td>' +
                    '<input type="text" class="form-control uppercase-input toolNumber" maxlength="200" placeholder="Enter tool number">' +
                '</td>' +
                '<td>' +
                    '<input type="text" class="form-control uppercase-input workorder-item" maxlength="200" placeholder="Search material...">' +
                '</td>' +
                '<td>' +
                    '<div class="input-step">' +
                        '<button type="button" class="minus_' + rowCounter + '">–</button>' +
                        '<input type="number" class="product-quantity" value="1" min="1" max="100">' +
                        '<button type="button" class="plus_' + rowCounter + '">+</button>' +
                    '</div>' +
                '</td>' +
                '<td>' +
                    '<button id="btAddRoute_' + rowCounter + '" type="button" class="btn btn-secondary waves-effect waves-light route-modal route">' +
                        '<i class="ri-list-ordered"></i>' +
                    '</button>' +
                '</td>' +
                '<td>' +
                    '<button id="btAddComment_' + rowCounter + '" type="button" class="btn btn-secondary waves-effect waves-light comment-modal comment">' +
                        '<i class="ri-sticky-note-fill"></i>' +
                    '</button>' +
                '</td>' +
            '</tr>';
        $('#tbWorkOrderItems').append(newRow);
        updateAddAndRemoveButtons();
        initializeInputNumericalMasks();
        initializeCounter(rowCounter);
        initializeItemAutocomplete('.workorder-item:last');
        initializeUpperCaseInput();
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
});

$(document).on('click', '#removeItem', function() {
    try {
        $(this).closest('tr').remove();
        updateAddAndRemoveButtons();
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
});

$(document).on('click', '.route-modal', function () {
    $('#mdRoute').removeAttr('idButtonOrigin');
    $('#mdRoute').attr('idButtonOrigin', $(this).attr('id'));
    var dataRoutes = $(this).attr('data-routes');
    if (dataRoutes !== undefined && dataRoutes !== '') {
        var routes = JSON.parse(dataRoutes);
        $('#mdRoute .form-check-input').each(function () {
            var checkboxValue = parseInt($(this).val());
            if (routes.includes(checkboxValue)) {
                $(this).prop('checked', true);
            } else {
                $(this).prop('checked', false);
            }
        });
    } else {
        $('#mdRoute .form-check-input').each(function () {
            $(this).prop('checked', false);
        });
    }
    $('#mdRoute').modal('show');
});

$(document).on('click', '.comment-modal', function () {
    $('#mdComment').removeAttr('idButtonOrigin');
    $('#mdComment').attr('idButtonOrigin', $(this).attr('id'));
    var dataComment = $(this).attr('data-comment');
    if (dataComment !== undefined && dataComment !== '') {
        $('#taComment').val(dataComment);
    } else {
        $('#taComment').val('');
    }
    $('#mdComment').modal('show');
});

async function initializeCatalogs()
{
    try {
        const response = await fetch('getCatalogs');
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

        const catalogs = await response.json();
        if (!catalogs || !catalogs.isSuccess || catalogs.results.length !== 1) {
            Swal.fire({
                title: 'Error!!',
                html: 'Catalogs not downloaded. Please reload the page.',
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
            return;
        }

        var selectMapping = {
            sePriority: 0
        };

        for (var selectId in selectMapping) {
            var index = selectMapping[selectId];
            var $select = $('#' + selectId);
            $select.empty().append('<option value="">Select option</option>');
            catalogs.results[index].forEach(function(item) {
                $select.append(
                    $('<option>', {
                        value: item.id,
                        text: item.description
                    })
                );
            });
        }
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

function addRoute() {
    try {
        var routeButton = $('#' + $('#mdRoute').attr('idButtonOrigin'));
        let selectedRoutes = [];
        $('#mdRoute .form-check-input:checked').each(function() {
            selectedRoutes.push(parseInt($(this).val()));
        });
        routeButton.attr('data-routes', JSON.stringify(selectedRoutes));
        $('#mdRoute').modal('hide');
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

function addComment() {
    try {
        var commentButton = $('#' + $('#mdComment').attr('idButtonOrigin'));
        commentButton.attr('data-comment', $('#taComment').val().trim());
        $('#mdComment').modal('hide');
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

function initializeItemAutocomplete(element) {
    try {
        $(element).autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: '/inventory/getItemByTerm',
                    method: 'get',
                    dataType: 'json',
                    data: {
                        description: request.term
                    },
                    success: function(data) {
                        response($.map(data, function(item) {
                            return {
                                label: item.itemCode + " - " + item.itemName,
                                value: item.itemCode + " - " + item.itemName,
                                id: item.id,
                                description: item.itemDescription,
                                code: item.itemCode,
                                quantity: item.quantity
                            };
                        }));
                    },
                    error: function(error) {
                        Swal.fire({
                            title: 'Error!!',
                            html: error.responseText,
                            icon: 'error',
                            confirmButtonClass: 'btn btn-danger w-xs mt-2',
                            buttonsStyling: !1,
                            footer: '',
                            showCloseButton:!1
                        });
                    }
                });
            },
            minLength: 2,
            select: function(event, ui) {
                var row = $(this).closest('tr');
                row.find('.workorder-item').val(ui.item.description);
                row.find('.workorder-item').attr('inventorytemid', ui.item.id);
                row.find('.workorder-item').attr('inventorytemquantity', ui.item.quantity);
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

function updateAddAndRemoveButtons() {
    try {
        var count = 1;
        $('#tbWorkOrderItems tr').each(function(index, element) {
            $(element).find('#addItem').remove();
            $(element).find('#rowCount').text(count++);
        });
        var lastRow = $('#tbWorkOrderItems tr:last');
        lastRow.find('td:first').prepend('<button id="addItem" type="button" class="btn btn-success btn-sm btn-icon waves-effect waves-light"><i class="ri-add-circle-fill"></i></button>');
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

function initializeCounter(counter) {
    try {
        var plusButton = document.querySelector(".plus_" + counter);
        var minusButton = document.querySelector(".minus_" + counter);
        if (plusButton) {
            plusButton.removeEventListener("click", incrementValue);
            plusButton.addEventListener("click", incrementValue);
        }
        if (minusButton) {
            minusButton.removeEventListener("click", decrementValue);
            minusButton.addEventListener("click", decrementValue);
        }

        function incrementValue(event) {
            var input = event.target.previousElementSibling;
            var maxValue = input.getAttribute("max");
            var currentValue = parseInt(input.value);
            if (currentValue < maxValue) {
                input.value = currentValue + 1;
            }
        }

        function decrementValue(event) {
            var input = event.target.nextElementSibling;
            var minValue = input.getAttribute("min");
            var currentValue = parseInt(input.value);
            if (currentValue > minValue) {
                input.value = currentValue - 1;
                updateQuotationAmounts();
            }
        }
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

function getWorkOrderItems() {
    try {
        var items = [];
        $('#tbWorkOrderItems tr').each(function () {
            var rawRoutes = $(this).find('.route').attr('data-routes');
            var routesAsList = JSON.parse(rawRoutes).map(route => route.toString());
            var item = {
                toolNumber: $(this).find('.toolNumber').val() || '',
                inventoryItemId: $(this).find('.workorder-item').attr('inventorytemid') || '',
                quantityInStock: $(this).find('.workorder-item').attr('inventorytemquantity') || '',
                quantity: parseFloat($(this).find('.product-quantity').val()) || 0,
                routes: routesAsList || [],
                comments: $(this).find('.comment').attr('data-comment') || ''
            };
            items.push(item);
        });
        return items;
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

function add() {
    try {
        var workOrder = {
            quotationId: $('#inQuotationId').val(),
            items: getWorkOrderItems(),
            priorityId: $('#sePriority').val(),
            rfq: parseInt($('#inRfq').val()),
            deliveryDate: $('#inDeliveryDate').val()
        };

        if(!isValidForm(workOrder))
            return;

        $('#loader').show();
        fetch('add', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(workOrder)
        })
        .then(response => {return response.json()})
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
                $('#loader').hide();
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
            $('#loader').hide();
        })
        .catch(error => {
            Swal.fire({
                title: 'Error!',
                text: error,
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
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
    }
}

function isValidForm(workOrder) {
    try {
        if (!workOrder.quotationId || !workOrder.priorityId || !workOrder.deliveryDate || !workOrder.items || workOrder.items.length === 0) {
            Swal.fire({
                title: 'Error!!',
                html: 'The fields Quotation, Priority, Delivery Date, and Items cannot be empty.',
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: false
            });
            return false;
        }

        const today = new Date();
        const todayUTC = new Date(Date.UTC(today.getUTCFullYear(), today.getUTCMonth(), today.getUTCDate()));
        const deliveryDate = new Date(workOrder.deliveryDate);
        const deliveryDateUTC = new Date(Date.UTC(deliveryDate.getUTCFullYear(), deliveryDate.getUTCMonth(), deliveryDate.getUTCDate()));

        if (deliveryDateUTC < deliveryDate) {
            Swal.fire({
                title: 'Error!!',
                html: 'Delivery Date must be today or a future date.',
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: false
            });
            return false;
        }

        for (const item of workOrder.items) {
            if (!item.toolNumber || item.toolNumber.trim() === '') {
                Swal.fire({
                    title: 'Error!!',
                    html: 'Tool Number cannot be empty.',
                    icon: 'error',
                    confirmButtonClass: 'btn btn-danger w-xs mt-2',
                    buttonsStyling: false,
                    footer: '',
                    showCloseButton: false
                });
                return false;
            }

            if (!item.inventoryItemId || item.inventoryItemId <= 0) {
                Swal.fire({
                    title: 'Error!!',
                    html: 'Inventory Item cannot be empty.',
                    icon: 'error',
                    confirmButtonClass: 'btn btn-danger w-xs mt-2',
                    buttonsStyling: false,
                    footer: '',
                    showCloseButton: false
                });
                return false;
            }

            if (!item.quantity || item.quantity <= 0) {
                Swal.fire({
                    title: 'Error!!',
                    html: 'Quantity must be greater than 0 for: ' + item.toolNumber,
                    icon: 'error',
                    confirmButtonClass: 'btn btn-danger w-xs mt-2',
                    buttonsStyling: false,
                    footer: '',
                    showCloseButton: false
                });
                return false;
            }

            if (!item.quantityInStock || item.quantityInStock <= 0) {
                Swal.fire({
                    title: 'Error!!',
                    html: 'There are no items in stock for this material:' + item.toolNumber,
                    icon: 'error',
                    confirmButtonClass: 'btn btn-danger w-xs mt-2',
                    buttonsStyling: false,
                    footer: '',
                    showCloseButton: false
                });
                return false;
            }

            if (item.quantity > item.quantityInStock) {
                Swal.fire({
                    title: 'Error!!',
                    html: 'The quantity must be less than the 24 items available in stock for this material: ' + item.toolNumber,
                    icon: 'error',
                    confirmButtonClass: 'btn btn-danger w-xs mt-2',
                    buttonsStyling: false,
                    footer: '',
                    showCloseButton: false
                });
                return false;
            }

            if (!item.routes || item.routes === '[]') {
                Swal.fire({
                    title: 'Error!!',
                    html: 'Routes cannot be empty.',
                    icon: 'error',
                    confirmButtonClass: 'btn btn-danger w-xs mt-2',
                    buttonsStyling: false,
                    footer: '',
                    showCloseButton: false
                });
                return false;
            }
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

$(document).ready(function() {
    $('#loader').show();
    updateAddAndRemoveButtons();
    initializeQuotationAutocomplete();
    initializeInputNumericalMasks();
    initializeCounter(1);
    initializeItemAutocomplete('.workorder-item');
    initializeUpperCaseInput();
    initializeCatalogs().then(() => {
        $('#loader').hide();
    });
});