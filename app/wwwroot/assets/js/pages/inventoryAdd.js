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
        // Si hay un punto, corta todo después de la segunda aparición
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

$(document).ready(function() {
    initializeInputNumericalMasks();
});