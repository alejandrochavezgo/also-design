document.querySelectorAll('.uppercase-input').forEach(input => {
    input.addEventListener('input', function() {
        this.value = this.value.toUpperCase();
    });
});

$(document).ready(function() {
    $('#loader').show();
    $('#loader').hide();
});