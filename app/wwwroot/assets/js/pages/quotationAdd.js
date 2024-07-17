document.querySelectorAll('.uppercase-input').forEach(input => {
    input.addEventListener('input', function() {
        this.value = this.value.toUpperCase();
    });
});

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
                console.log(ui);
                var row = $(this).closest('tr');
                console.log(row);
                row.find('.quotation-item').val(ui.item.value);
                row.find('.product-quantity').val(1);
                row.find('td:nth-child(3)').text(ui.item.code);
                row.find('td:nth-child(6)').text(ui.item.unit);
                row.find('td:nth-child(7)').text('$' + ui.item.unitValue.toFixed(2));
                row.find('td:nth-child(8)').text('$' + (ui.item.unitValue * row.find('.product-quantity').val()).toFixed(2));
                updateQuotationAmounts();
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

function updateAddButtons() {
    try {
        $('#tbItems tr').each(function(index, element) {
            $(element).find('#addItem').remove();
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

function updateQuotationAmounts() {
    try {
        var subTotal = 0;
        $('#tbItems tr').each(function(index, row) {
            var quantity = $(row).find('.product-quantity').val();
            var unitValue = parseFloat($(row).find('td:nth-child(7)').text().replace('$', ''));
            var total = quantity * unitValue;
            $(row).find('td:nth-child(8)').text('$' + total.toFixed(2));
            subTotal += total;
        });
        var taxRate = parseFloat($('#inQuotationTax').val()) || 0;
        var taxAmount = subTotal * (taxRate / 100);
        var totalAmount = subTotal + taxAmount;
        $('#tdQuotationSubTotal').text('$' + subTotal.toFixed(2));
        $('#tdQuotationTotalAmount').text('$' + taxAmount.toFixed(2));
        $('#thQuotationTotalAmount').text('$' + totalAmount.toFixed(2));
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
        var newRow = '' + 
            '<tr>' +
                '<td class="width-0per">' +
                    '<button id="addItem" type="button" class="btn btn-success btn-sm btn-icon waves-effect waves-light"><i class="ri-add-circle-fill"></i></button>' +
                    '<span> </span>' +
                    '<button id="removeItem" type="button" class="btn btn-danger btn-sm btn-icon waves-effect waves-light"><i class="ri-close-circle-fill"></i></button>' +
                    '</td>' +
                    '<td scope="row">1</td>' +
                    '<td scope="row">-</td>' +
                '<td class="text-start">' +
                    '<input type="text" class="form-control fw-medium quotation-item mb-1 bg-light border-0" id="inQuotationItem" placeholder="Search Item...">' +
                    '<textarea class="form-control bg-light border-0" rows="2" placeholder="Item Details"></textarea>' +
                '</td>' +
                '<td>' +
                    '<div class="input-step">' +
                        '<button type="button" class="minus">–</button>' +
                        '<input type="number" class="product-quantity" value="1" min="1" max="100" readonly>' +
                        '<button type="button" class="plus">+</button>' +
                    '</div>' +
                '</td>' +
                '<td>-</td>' +
                '<td class="text-end">$0.00</td>' +
                '<td class="text-end">$0.00</td>' +
            '</tr>';
    
        $('#tbItems').append(newRow);
        updateAddButtons();
        initializeItemAutocomplete('.quotation-item:last');
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
        updateAddButtons();
        updateQuotationAmounts();
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

function initializeClientAutocomplete() {
    try {
        $('#inClientBusinessName').autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: '/client/getClientByTerm',
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
                            html: exception,
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
                $('#inClientId').val(ui.item.id);
                $('#inClientCity').val(ui.item.city);
                $('#inClientAddress').val(ui.item.address);
                $('#inClientRfc').val(ui.item.rfc);
                $('#clientContactNames').empty();
                $.each(ui.item.contactNames, function(index, value) {
                    $('#clientContactNames').append($('<option>').text(value).attr('value', value));
                });
                $.each(ui.item.contactPhones, function(index, value) {
                    $('#clientContactPhones').append($('<option>').text(value).attr('value', value));
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

function initializeInputMasks() {
    try {
        $('#inQuotationTax').on('input', function() {
            var value = $(this).val();
            var valid = /^\d*\.?\d*$/.test(value);
            if (!valid || value.length > 5) {
                $(this).val(value.slice(0, -1));
            } else {
                var numValue = parseFloat(value);
                if (numValue < 0) {
                    $(this).val('0');
                }
            }
            updateQuotationAmounts();
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

function initializeCounter() {
    try {
        var t = document.getElementsByClassName("plus"),
            e = document.getElementsByClassName("minus"),
            n = document.getElementsByClassName("product");
        t &&
            Array.from(t).forEach(function (t) {
                t.addEventListener("click", function (e) {
                    var input = e.target.previousElementSibling;
                    if (parseInt(input.value) < input.getAttribute("max")) {
                        input.value++;
                        updateQuotationAmounts();
                        n &&
                            Array.from(n).forEach(function (t) {
                                updateQuantity(e.target);
                            });
                    }
                });
            });
        e &&
            Array.from(e).forEach(function (t) {
                t.addEventListener("click", function (e) {
                    var input = e.target.nextElementSibling;
                    if (parseInt(input.value) > input.getAttribute("min")) {
                        input.value--;
                        updateQuotationAmounts();
                        n &&
                            Array.from(n).forEach(function (t) {
                                updateQuantity(e.target);
                            });
                    }
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
    updateAddButtons();
    initializeItemAutocomplete('.quotation-item');
    initializeClientAutocomplete();
    initializeInputMasks();
    initializeCounter();
});