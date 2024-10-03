document.querySelectorAll('.uppercase-input').forEach(input => {
    input.addEventListener('input', function() {
        this.value = this.value.toUpperCase();
    });
});

function reset() {
    try {
        $('#inBusinessName').val('');
        $('#inRfc').val('');
        $('#inAddress').val('');
        $('#inZipCode').val('');
        $('#inCity').val('');
        $('#inState').val('');
        $('#inCountry').val('');
        $('#inContactEmails').val('');
        $('#inContactPhones').val('');
        $('#inContactNames').val('');
        $('#seStatus').val(1);
    } catch (exception) {
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
}

function addSupplier() {
    try {
        if(!isValidForm())
            return;

        $('#loader').show();
        fetch('add', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                businessName: $('#inBusinessName').val(),
                rfc: $('#inRfc').val(),
                address: $('#inAddress').val(),
                zipCode: $('#inZipCode').val(),
                city: $('#inCity').val(),
                state: $('#inState').val(),
                country: $('#inCountry').val(),
                contactNames: $('#inContactNames').val().split(','),
                contactEmails: $('#inContactEmails').val().split(','),
                contactPhones: $('#inContactPhones').val().split(','),
                status: $('#seStatus').val()
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
                title: 'Error!!',
                html: error,
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: !1,
                footer: '',
                showCloseButton: !1
            });
            $('#loader').hide();
        });
    }
    catch (exception) {
        Swal.fire({
            title: 'Error!!',
            html: exception,
            icon: 'error',
            confirmButtonClass: 'btn btn-danger w-xs mt-2',
            buttonsStyling: !1,
            footer: '',
            showCloseButton: !1
        });
        $('#loader').hide();
    }
}

function isValidForm() {
    try {
        var businessName = $('#inBusinessName').val();
        var rfc = $('#inRfc').val();
        var address = $('#inAddress').val();
        var zipCode = $('#inZipCode').val();
        var status = $('#seStatus').val();
        var city = $('#inCity').val();
        var state = $('#inState').val();
        var country = $('#inCountry').val();

        if (!businessName || !rfc || !address || !zipCode || !status || !city || !state || !country) {
            Swal.fire({
                title: 'Error!!',
                html: 'The fields Bussiness Name, RFC, Address, ZIP Code, Status, City, State and Country cannot be empty.',
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