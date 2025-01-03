function initializeUpperCaseInput() {
    document.querySelectorAll('.uppercase-input').forEach(input => {
        input.addEventListener('input', function() {
            this.value = this.value.toUpperCase();
        });
    });
}

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
                    '<textarea class="form-control fw-medium mb-1 bg-light border-0" rows="2" placeholder="Material"></textarea>' +
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
                    '<option value="">Select option</option>' +
                    '<option value="1">EACH (EA)</option>' +
                    '<option value="2">BOX (BOX)</option>' +
                    '<option value="3">PACKAGE (PKG)</option>' +
                    '<option value="4">SET (SET)</option>' +
                    '<option value="5">PALLET (PAL)</option>' +
                    '<option value="6">POUND (LB)</option>' +
                    '<option value="7">KILOGRAM (KG)</option>' +
                    '<option value="8">METER (M)</option>' +
                    '<option value="9">LITER (L)</option>' +
                    '<option value="10">FOOT (FT)</option>' +
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
                    method: 'get',
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
                $('#inClientId').val(ui.item.id);
                $('#inClientCity').val(ui.item.city);
                $('#inClientAddress').val(ui.item.address);
                $('#inClientRfc').val(ui.item.rfc);
                $('#seClientContactNames').empty();
                $('#seClientContactPhones').empty();
                $.each(ui.item.contactNames, function(index, value) {
                    $('#seClientContactNames').append($('<option>').text(value).attr('value', value));
                });
                $.each(ui.item.contactPhones, function(index, value) {
                    $('#seClientContactPhones').append($('<option>').text(value).attr('value', value));
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

function initializeProjectAutocomplete() {
    try {
        $('#inProjectName').autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: '/project/getProjectByTerm',
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
                updateQuotationAmounts();
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
        if (!catalogs || !catalogs.isSuccess || catalogs.results.length !== 2) {
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
            seQuotationPaymentType: 0,
            quotationCurrencyType: 1
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

function add() {
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
                unitValue: parseFloat($(row).find('.subtotal-value').val()),
                totalValue: parseFloat($(row).find('.total-value').text().replace('$', '').trim())
            };

            if (!item.description || !item.material || !item.unit)
                return true;

            let imageFile = $(row).find('input[type="file"]')[0].files[0];
            if (imageFile) {
                formData.append('image_' + index, imageFile);
            }
            items.push(item);
        });

        let quotation = {
            projectId: $('#inProjectName').attr('projectId'),
            client: {
                id: $('#inClientId').val(),
                mainContactName: $('#seClientContactNames').val(),
                mainContactPhone: $('#seClientContactPhones').val()
            },
            payment: {
                id: $('#seQuotationPaymentType').val()
            },
            user: {
                id: $('#userContactId').val(),
                employee: {
                    mainContactPhone: $('#seEmployeeContactPhone').val()
                }
            },
            status: '1',
            currency: {
                id: $('#quotationCurrencyType').val(),
            },
            subTotal: parseFloat($('#tdQuotationSubTotal').text().replace('$', '').trim()),
            taxRate: parseFloat($('#inQuotationTax').val()),
            taxAmount: parseFloat($('#tdQuotationTaxAmount').text().replace('$', '').trim()),
            totalAmount: parseFloat($('#thQuotationTotalAmount').text().replace('$', '').trim()),
            generalNotes: $('textarea[placeholder="Notes"]').eq(1).val(),
            items: items
        };

        if(!isValidForm(quotation))
            return;

        $('#loader').show();
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
        $('#loader').hide();
    }
}

function isValidForm(quotation) {
    try {
        if (!quotation.projectId || !quotation.client.id || !quotation.client.mainContactName || !quotation.client.mainContactPhone ||
            !quotation.payment.id || !quotation.user.id || !quotation.user.employee.mainContactPhone || !quotation.currency.id || quotation.items.length == 0) {
            Swal.fire({
                title: 'Error!!',
                html: 'The fields Client, Payment Type, User Phone Contact, Currency and Items cannot be empty.',
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: !1,
                footer: '',
                showCloseButton: !1
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

$(document).ready(function() {
    $('#loader').show();
    updateAddAndRemoveButtons();
    initializeClientAutocomplete();
    initializeProjectAutocomplete();
    initializeInputTaxMasks();
    initializeInputNumericalMasks();
    initializeCounter(1);
    initializeUpperCaseInput();
    initializeCatalogs().then(() => {
        $('#loader').hide();
    });
});