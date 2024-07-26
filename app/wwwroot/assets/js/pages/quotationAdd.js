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

function updateQuotationAmounts() {
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
        var taxRate = parseFloat($('#inQuotationTax').val()) || 0;
        var taxAmount = subTotal * (taxRate / 100) || 0;
        var totalAmount = subTotal + taxAmount || 0;

        $('#tdQuotationSubTotal').text('$' + subTotal.toFixed(2));
        $('#tdQuotationTaxAmount').text('$' + taxAmount.toFixed(2));
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
                    '<td scope="row">' +
                        '<span id="rowCount">' +
                            (($('#tbItems tr').length) + 1) +
                        '</span>' +
                    '</td>' +
                '<td class="text-start">' +
                    '<textarea class="form-control fw-medium mb-1 bg-light border-0" rows="3" placeholder="Description"></textarea>' +
                    '<textarea class="form-control fw-medium mb-1 bg-light border-0" rows="2" placeholder="Material"></textarea>' +
                    '<textarea class="form-control fw-medium mb-1 bg-light border-0" rows="3" placeholder="Details"></textarea>' +
                    '<input class="form-control" type="file">' +
                '</td>' +
                '<td>' +
                    '<div class="input-step">' +
                        '<button type="button" class="minus">–</button>' +
                        '<input type="number" class="product-quantity" value="1" min="1" max="100" readonly>' +
                        '<button type="button" class="plus">+</button>' +
                    '</div>' +
                '</td>' +
                '<td>' +
                    '<select class="form-select">' +
                        '<option value="1">EA</option>' +
                    '</select>' +
                '</td>' +
                '<td class="text-end">' +
                    '<input class="form-control text-end numerical-mask" value="0.00" />' +
                '</td>' +
                '<td class="text-end">' +
                    '0.00' +
                '</td>' +
            '</tr>';
        $('#tbItems').append(newRow);
        updateAddAndRemoveButtons();
        initializeInputNumericalMasks();
        initializeCounter();
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

$(document).on('click', '#removeItem', function() {
    try {
        $(this).closest('tr').remove();
        updateAddAndRemoveButtons();
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

            updateQuotationAmounts();
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
        $('#inQuotationTax').on('input', function() {
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

function addQuotation() {
    try {
        let formData = new FormData();
        let items = [];
        $('#tbItems tr').each(function(index, row) {
            let item = {
                description: $(row).find('textarea').eq(0).val(),
                material: $(row).find('textarea').eq(1).val(),
                details: $(row).find('textarea').eq(2).val(),
                quantity: parseInt($(row).find('.product-quantity').val(), 10),
                unit: $(row).find('select').val(),
                unitValue: parseFloat($(row).find('.subtotal-value').val()),
                totalValue: parseFloat($(row).find('.total-value').text().replace('$', '').trim())
            };

            let imageFile = $(row).find('input[type="file"]')[0].files[0];
            if (imageFile) {
                formData.append('image_' + index, imageFile);
            }
            items.push(item);
        });

        let quotation = {
            client: {
                id: $('#inClientId').val(),
                mainContactName: $('#clientContactNames').val(),
                mainContactPhone: $('#clientContactPhones').val(),
            },
            payment: {
                id: $('#quotationPaymentType').val(),
            },
            status: "1",
            currency: {
                id: $('#quotationCurrencyType').val(),
            },
            subTotal: parseFloat($('#tdQuotationSubTotal').text().replace('$', '').trim()),
            taxRate: parseFloat($('#inQuotationTax').val()),
            taxAmount: parseFloat($('#tdQuotationTaxAmount').text().replace('$', '').trim()),
            totalAmount: parseFloat($('#thQuotationTotalAmount').text().replace('$', '').trim()),
            toolNotes: $('textarea[placeholder="Notes"]').eq(0).val(),
            generalNotes: $('textarea[placeholder="Notes"]').eq(1).val(),
            items: items
        };
        formData.append('quotation', JSON.stringify(quotation));

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
    initializeClientAutocomplete();
    initializeInputTaxMasks();
    initializeInputNumericalMasks();
    initializeCounter();
});