document.querySelectorAll('.uppercase-input').forEach(input => {
    input.addEventListener('input', function() {
        this.value = this.value.toUpperCase();
    });
});

function reset() {
    try {
        $('#inEmail').val('');
        $('#inFirstname').val('');
        $('#inLastname').val('');
        $('#inUsername').val('');
        $('#inPassword').val('');
        $('#seStatus').val(1);
        $('#seGender').val('M');
        $('#inAddress').val('');
        $('#inCity').val('');
        $('#inState').val('');
        $('#inZipcode').val('');
        $('#inJobPosition').val('');
        $('#inContactPhones').val('');
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

function add() {
    try {
        fetch('add', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                email: $('#inEmail').val(),
                firstname: $('#inFirstname').val(),
                lastname: $('#inLastname').val(),
                status: $('#seStatus').val(),
                username: $('#inUsername').val(),
                password: $('#inPassword').val(),
                employee: {
                    gender: $('#seGender').val(),
                    address: $('#inAddress').val(),
                    city: $('#inCity').val(),
                    state: $('#inState').val(),
                    zipcode: $('#inZipcode').val(),
                    jobPosition: $('#inJobPosition').val(),
                    profession: $('#inProfession').val(),
                    contactPhones: $('#inContactPhones').val().split(',')
                }
            })
        })
        .then(response => {
            return response.json();
        })
        .then(data => {
            if(!data.isSuccess) {
                Swal.fire({
                    title: 'Error!!',
                    html: data.message,
                    icon: 'error',
                    confirmButtonClass: 'btn btn-danger w-xs mt-2',
                    buttonsStyling: !1,
                    footer: '',
                    showCloseButton:!1
                });
                return;
            }

            Swal.fire({
                title: 'Succcess',
                html: data.message,
                icon: 'success',
                confirmButtonClass: 'btn btn-success w-xs mt-2',
                buttonsStyling: !1,
                footer: '',
                showCloseButton:!1
            }).then(function(t) {
                window.location.href = 'list';
            });
        })
        .catch(error => {
            Swal.fire({
                title: 'Error!!',
                html: error,
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: !1,
                footer: '',
                showCloseButton:!1
            });
        });
    }
    catch(exception) {
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