document.querySelectorAll('.uppercase-input').forEach(input => {
    input.addEventListener('input', function() {
        this.value = this.value.toUpperCase();
    });
});

$('#inQuantity, #inUnitValue').on('input', function() {
    var quantity = parseFloat($('#inQuantity').val().replace(/[^0-9.]/g, '')) || 0
    var unitValue = parseFloat($('#inUnitValue').val().replace(/[^0-9.]/g, '')) || 0
    $('#inTotalValue').val('$' + (quantity * unitValue).toFixed(2));
});

$('#inUnitValue').on('input', function() {
    var value = $(this).val().replace(/[^0-9.]/g, '');
    var decimalIndex = value.indexOf('.');
    if (decimalIndex !== -1) {
        value = value.substring(0, decimalIndex + 1) + value.substring(decimalIndex + 1).replace(/\./g, '');
    }

    var parts = value.split('.');
    if (parts[0].length > 1) {
        parts[0] = parts[0].replace(/^0+/, '');
        if (parts[0] === '') {
            parts[0] = '0';
        }
    }

    value = parts.join('.');
    if (value === '') {
        $(this).val('$0.00');
    } else {
        $(this).val('$' + value);
    }
});

$('#inUnitValue').on('blur', function() {
    if ($(this).val() === '$') {
        $(this).val('$0.00');
    }
});

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
        if (!catalogs || !catalogs.isSuccess || catalogs.results.length !== 9) {
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
            seStatus: 0,
            seMaterial: 1,
            seFinishType: 2,
            seUnitDiameter: 3,
            seUnitLength: 3,
            seUnitWeight: 4,
            seUnitTolerance: 5,
            seWarehouseLocation: 6,
            seUnit: 7,
            seCurrency: 8
        };

        for (var selectId in selectMapping) {
            var index = selectMapping[selectId];
            var $select = $('#' + selectId);
            $select.empty().append('<option value="">Select option</option>');
        
            if (selectId === 'seUnitDiameter' || selectId === 'seUnitLength') {
                catalogs.results[index].forEach(function(item) {
                    $select.append(
                        $('<option>', {
                            value: item.id,
                            text: item.description
                        })
                    );
                });
            } else {
                catalogs.results[index].forEach(function(item) {
                    $select.append(
                        $('<option>', {
                            value: item.id,
                            text: item.description
                        })
                    );
                });
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

function update() {
    try {
        let formData = new FormData();
        let inventoryItem = {
            id: $('#inInventoryItemId').val(),
            itemCode: $('#inItemCode').val(),
            itemName: $('#inItemName').val(),
            status: $('#seStatus').val(),
            description: $('#taDescription').val(),
            material: $('#seMaterial').val(),
            finishType: $('#seFinishType').val(),
            diameter: parseFloat($('#inDiameter').val()),
            unitDiameter: $('#seUnitDiameter').val(),
            length: parseFloat($('#inLength').val()),
            unitLength: $('#seUnitLength').val(),
            weight: parseFloat($('#inWeight').val()),
            unitWeight: $('#seUnitWeight').val(),
            tolerance: parseFloat($('#inTolerance').val()),
            unitTolerance: $('#seUnitTolerance').val(),
            warehouseLocation: $('#seWarehouseLocation').val(),
            quantity: parseFloat($('#inQuantity').val()),
            reorderQty: parseFloat($('#inReorderQty').val()),
            unit: $('#seUnit').val(),
            currency: $('#seCurrency').val(),
            unitValue: parseFloat($('#inUnitValue').val().replace('$', '').trim()),
            totalValue: parseFloat($('#inTotalValue').val().replace('$', '').trim()),
            notes: $('#taNotes').val()
        };

        let itemImageFile = $('#inItemImage')[0].files[0];
        if (itemImageFile) {
            formData.append('itemImage', itemImageFile);
            inventoryItem.itemImageHasNewImage = true;
            inventoryItem.itemImageHasOriginalImage = false;
            inventoryItem.itemImageHasNotImage = false;
        } else if ($('#itemImage').length > 0) {
            inventoryItem.itemImageHasNewImage = false;
            inventoryItem.itemImageHasOriginalImage = true;
            inventoryItem.itemImageHasNotImage = false;
        } else {
            inventoryItem.itemImageHasNewImage = false;
            inventoryItem.itemImageHasOriginalImage = false;
            inventoryItem.itemImageHasNotImage = true;
        }

        let bluePrintsFile = $('#inBluePrints')[0].files[0];
        if (bluePrintsFile) {
            formData.append('bluePrints', bluePrintsFile);
            inventoryItem.bluePrintsHasNewDocument = true;
            inventoryItem.bluePrintsHasOriginalDocument = false;
            inventoryItem.bluePrintsHasNotDocument = false;
        } else if ($('#downloadBluePrints').length > 0) {
            inventoryItem.bluePrintsHasNewDocument = false;
            inventoryItem.bluePrintsHasOriginalDocument = true;
            inventoryItem.bluePrintsHasNotDocument = false;
        } else {
            inventoryItem.bluePrintsHasNewDocument = false;
            inventoryItem.bluePrintsHasOriginalDocument = false;
            inventoryItem.bluePrintsHasNotDocument = true;
        }

        let techSpecFile = $('#inTechnicalSpecifications')[0].files[0];
        if (techSpecFile) {
            formData.append('technicalSpecifications', techSpecFile);
            inventoryItem.technicalSpecificationsHasNewDocument = true;
            inventoryItem.technicalSpecificationsHasOriginalDocument = false;
            inventoryItem.technicalSpecificationsHasNotDocument = false;
        } else if ($('#downloadTechnicalSpecifications').length > 0) {
            inventoryItem.technicalSpecificationsHasNewDocument = false;
            inventoryItem.technicalSpecificationsHasOriginalDocument = true;
            inventoryItem.technicalSpecificationsHasNotDocument = false;
        } else {
            inventoryItem.technicalSpecificationsHasNewDocument = false;
            inventoryItem.technicalSpecificationsHasOriginalDocument = false;
            inventoryItem.technicalSpecificationsHasNotDocument = true;
        }

        if(!isValidForm(inventoryItem))
            return;

        $('#loader').show();
        formData.append('inventoryItem', JSON.stringify(inventoryItem));
        fetch('update', {
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
                location.reload(true);
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

function isValidForm(inventoryItem) {
    try {
        if (!inventoryItem.itemCode ||
            !inventoryItem.itemName ||
            !inventoryItem.status ||
            !inventoryItem.description ||
            !inventoryItem.notes ||
            !inventoryItem.material ||
            !inventoryItem.finishType ||
            isNaN(inventoryItem.diameter) ||
            !inventoryItem.unitDiameter ||
            isNaN(inventoryItem.length) ||
            !inventoryItem.unitLength ||
            isNaN(inventoryItem.weight) ||
            !inventoryItem.unitWeight ||
            isNaN(inventoryItem.tolerance) ||
            !inventoryItem.unitTolerance ||
            !inventoryItem.warehouseLocation ||
            isNaN(inventoryItem.quantity) ||
            isNaN(inventoryItem.reorderQty) ||
            !inventoryItem.unit ||
            !inventoryItem.currency ||
            isNaN(inventoryItem.unitValue) ||
            isNaN(inventoryItem.totalValue)) {
            Swal.fire({
                title: 'Error!!',
                html: 'The fields Item Code, Item Name, Status, Description, Notes, Material, Finish Type, Diameter, Diameter Unit, Length, Length Unit, Weight, Weight Unit, Tolerance, Tolerance Unit, Warehouse Location, Quantity, Reorder Quantity, Package Unit, Currency, Unit value, Total Value and Notes cannot be empty.',
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

$('#inItemImage').on('change', function(event) {
    const file = event.target.files[0];
    if (file) {
        $('#itemImage').remove();
        $('#deleteItemImage').remove();
    }
});

$('#inBluePrints').on('change', function(event) {
    const file = event.target.files[0];
    if (file) {
        $('#downloadBluePrints').remove();
        $('#deleteBluePrints').remove();
    }
});

$('#inTechnicalSpecifications').on('change', function(event) {
    const file = event.target.files[0];
    if (file) {
        $('#downloadTechnicalSpecifications').remove();
        $('#deleteTechnicalSpecifications').remove();
    }
});

$('#deleteItemImage').on('click', function () {
    $('#itemImage').remove();
    $(this).remove();
});

$('#deleteBluePrints').on('click', function () {
    $('#downloadBluePrints').remove();
    $(this).remove();
});

$('#deleteTechnicalSpecifications').on('click', function () {
    $('#downloadTechnicalSpecifications').remove();
    $(this).remove();
});

$(document).ready(function() {
    $('#loader').show();
    initializeInputNumericalMasks();
    initializeCatalogs().then(() => {
        onCatalogsInitialized();
        $('#loader').hide();
    });
});