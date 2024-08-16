document.querySelectorAll('.uppercase-input').forEach(input => {
    input.addEventListener('input', function() {
        this.value = this.value.toUpperCase();
    });
});

function updateAddAndRemoveButtons() {
    try {
        var count = 1;
        $('#tbItems tr').each(function(index, element) {
            $(element).find('#addItem').remove();
            $(element).find('#rowCount').text(count++);
        });
        var lastRow = $('#tbItems tr:last');
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

function updatePurchaseOrderAmounts() {
    try {
        var subTotal = 0;
        $('#tbItems tr').each(function(index, row) {
            var quantity = $(row).find('.product-quantity').val();
            var unitValueStr = $(row).find('.numerical-mask').val().replace('$', '').trim();
            var unitValue = unitValueStr ? parseFloat(unitValueStr) : 0;
            var total = quantity * unitValue;
            $(row).find('td:last').text('$' + total.toFixed(2));
            subTotal += total;
        });
        var taxRate = parseFloat($('#inPurchaseOrderTax').val()) || 0;
        var taxAmount = subTotal * (taxRate / 100) || 0;
        var totalAmount = subTotal + taxAmount || 0;

        $('#tdPurchaseOrderSubTotal').text('$' + subTotal.toFixed(2));
        $('#tdPurchaseOrderTaxAmount').text('$' + taxAmount.toFixed(2));
        $('#thPurchaseOrderTotalAmount').text('$' + totalAmount.toFixed(2));
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

$(document).on('click', '#addItem', function() {
    try {
        var rowCounter = ($('#tbItems tr').length) + 1;
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
                '<td class="text-start">' +
                    '<textarea class="form-control fw-medium mb-1 bg-light border-0" rows="3" placeholder="Description"></textarea>' +
                    '<textarea class="form-control fw-medium mb-1 bg-light border-0 quotation-item" rows="2" placeholder="Search material..."></textarea>' +
                    '<textarea class="form-control fw-medium mb-1 bg-light border-0" rows="3" placeholder="Details"></textarea>' +
                    '<input class="form-control mb-1" type="file">' +
                    '<textarea class="form-control fw-medium bg-light border-0" rows="4" placeholder="Notes"></textarea>' +
                '</td>' +
                '<td>' +
                    '<div class="input-step">' +
                        '<button type="button" class="minus_' + rowCounter + '">–</button>' +
                        '<input type="number" class="product-quantity" value="1" min="1" max="100" readonly>' +
                        '<button type="button" class="plus_' + rowCounter + '">+</button>' +
                    '</div>' +
                '</td>' +
                '<td>' +
                    '<select class="form-select">' +
                        '<option value="PZA">PZA</option>' +
                    '</select>' +
                '</td>' +
                '<td class="text-end">' +
                    '<input class="form-control text-end numerical-mask subtotal-value" value="0.00" />' +
                '</td>' +
                '<td class="text-end total-value">' +
                    '0.00' +
                '</td>' +
            '</tr>';
        $('#tbItems').append(newRow);
        updateAddAndRemoveButtons();
        initializeInputNumericalMasks();
        initializeCounter(rowCounter);
        initializeItemAutocomplete('.quotation-item:last');
        updatePurchaseOrderAmounts();
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
        updatePurchaseOrderAmounts();
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

function initializeSupplierAutocomplete() {
    try {
        $('#inSupplierBusinessName').autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: '/supplier/getSupplierByTerm',
                    method: 'GET',
                    dataType: 'json',
                    data: {
                        businessName: request.term
                    },
                    success: function(data) {
                        response($.map(data, function(item) {
                            return {
                                label: item.businessName,
                                value: item.businessName,
                                id: item.id,
                                businessName: item.businessName,
                                contactNames: item.contactNames,
                                city: item.city,
                                contactPhones: item.contactPhones,
                                address: item.address,
                                rfc: item.rfc
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
                $('#inSupplierId').val(ui.item.id);
                $('#inSupplierCity').val(ui.item.city);
                $('#inSupplierAddress').val(ui.item.address);
                $('#inSupplierRfc').val(ui.item.rfc);
                $('#seSupplierContactNames').empty();
                $('#seSupplierContactPhones').empty();
                $.each(ui.item.contactNames, function(index, value) {
                    $('#seSupplierContactNames').append($('<option>').text(value).attr('value', value));
                });
                $.each(ui.item.contactPhones, function(index, value) {
                    $('#seSupplierContactPhones').append($('<option>').text(value).attr('value', value));
                });
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

function initializeItemAutocomplete(element) {
    try {
        $(element).autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: '/inventory/getItemByTerm',
                    method: 'GET',
                    dataType: 'json',
                    data: {
                        description: request.term
                    },
                    success: function(data) {
                        response($.map(data, function(item) {
                            return {
                                label: item.description,
                                value: item.description,
                                id: item.id,
                                description: item.description,
                                code: item.code,
                                unit: item.unit,
                                unitValue: item.unitValue
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
                row.find('.quotation-item').val(ui.item.code + ' - ' + ui.item.value);
                row.find('td:nth-child(5) select').val(ui.item.unit);
                row.find('td:nth-child(6) input').val('$' + ui.item.unitValue.toFixed(2));
                row.find('td:nth-child(7)').text('$' + (ui.item.unitValue * row.find('.product-quantity').val()).toFixed(2));
                updatePurchaseOrderAmounts();
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

            updatePurchaseOrderAmounts();
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

function initializeInputTaxMasks() {
    try {
        $('#inPurchaseOrderTax').on('input', function() {
            var value = $(this).val();
            var valid = /^\d*\.?\d*$/.test(value);
            if (!valid || value.length > 10) {
                $(this).val(value.slice(0, -1));
            } else {
                var numValue = parseFloat(value);
                if (numValue < 0) {
                    $(this).val('0');
                }
            }
            updatePurchaseOrderAmounts();
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
                updatePurchaseOrderAmounts();
            }
        }

        function decrementValue(event) {
            var input = event.target.nextElementSibling;
            var minValue = input.getAttribute("min");
            var currentValue = parseInt(input.value);
            if (currentValue > minValue) {
                input.value = currentValue - 1;
                updatePurchaseOrderAmounts();
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

function addPurchaseOrder() {
    try {
        let formData = new FormData();
        let items = [];
        $('#tbItems tr').each(function(index, row) {
            let item = {
                description: $(row).find('textarea').eq(0).val(),
                material: $(row).find('textarea').eq(1).val(),
                details: $(row).find('textarea').eq(2).val(),
                notes: $(row).find('textarea').eq(3).val(),
                quantity: parseInt($(row).find('.product-quantity').val(), 10),
                unit: $(row).find('select').val(),
                unitValue: parseFloat($(row).find('.subtotal-value').val().replace('$', '').trim()),
                totalValue: parseFloat($(row).find('.total-value').text().replace('$', '').trim())
            };

            let imageFile = $(row).find('input[type="file"]')[0].files[0];
            if (imageFile) {
                formData.append('image_' + index, imageFile);
            }
            items.push(item);
        });

        let purchaseOrder = {
            supplier: {
                id: $('#inSupplierId').val(),
                mainContactName: $('#seSupplierContactNames').val(),
                mainContactPhone: $('#seSupplierContactPhones').val()
            },
            payment: {
                id: $('#sePurchaseOrderPaymentType').val()
            },
            user: {
                id: $('#userContactId').val(),
                employee: {
                    mainContactPhone: $('#seEmployeeContactPhone').val()
                }
            },
            status: '1',
            currency: {
                id: $('#purchaseOrderCurrencyType').val(),
            },
            subTotal: parseFloat($('#tdPurchaseOrderSubTotal').text().replace('$', '').trim()),
            taxRate: parseFloat($('#inPurchaseOrderTax').val()),
            taxAmount: parseFloat($('#tdPurchaseOrderTaxAmount').text().replace('$', '').trim()),
            totalAmount: parseFloat($('#thPurchaseOrderTotalAmount').text().replace('$', '').trim()),
            generalNotes: $('textarea[placeholder="Notes"]').eq(1).val(),
            items: items
        };
        formData.append('purchaseOrder', JSON.stringify(purchaseOrder));

        fetch('add', {
            method: 'post',
            headers: {
                'Accept': 'application/json'
            },
            body: formData
        })
        .then(response => response.json())
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
                title: 'Error!',
                text: error,
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
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
    updateAddAndRemoveButtons();
    initializeSupplierAutocomplete();
    initializeInputTaxMasks();
    initializeInputNumericalMasks();
    initializeCounter(1);
    initializeItemAutocomplete('.quotation-item');
});