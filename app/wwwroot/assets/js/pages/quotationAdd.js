document.querySelectorAll('.uppercase-input').forEach(input => {
    input.addEventListener('input', function() {
        this.value = this.value.toUpperCase();
    });
});

$(document).ready(function() {
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
});