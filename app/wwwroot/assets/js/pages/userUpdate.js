document.querySelectorAll('.uppercase-input').forEach(input => {
    input.addEventListener('input', function() {
        this.value = this.value.toUpperCase();
    });
});

function update() {
    try {
        fetch('update', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                id: $('#inUserId').val(),
                email: $('#inUserEmail').val(),
                password: $('#inUserPassword').val(),
                firstname: $('#inUserFirstname').val(),
                lastname: $('#inUserLastname').val(),
                status: $('#seUserStatus').val(),
                employee: {
                    id: $('#inEmployeeId').val(),
                    gender: $('#seEmployeeGender').val(),
                    address: $('#inEmployeeAddress').val(),
                    city: $('#inEmployeeCity').val(),
                    state: $('#inEmployeeState').val(),
                    zipcode: $('#inEmployeeZipcode').val(),
                    jobPosition: $('#inEmployeeJobPosition').val(),
                    profession: $('#inEmployeeProfession').val(),
                    contactPhones: $('#inEmployeeContactPhones').val().split(',')
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
                location.reload(true);
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