document.querySelectorAll('.uppercase-input').forEach(input => {
    input.addEventListener('input', function() {
        this.value = this.value.toUpperCase();
    });
});

async function initializeCatalogs() {
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
        if (!catalogs || !catalogs.isSuccess || catalogs.results.length !== 4) {
            Swal.fire({
                title: 'Error!!',
                html: 'Catalog not downloaded. Please reload the page.',
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            });
            return;
        }

        var selectMapping = {
            seUserStatus: 0,
            seEmployeeGender: 1,
            seUserAccess: 2,
            seUserRoles: 3
        };

        for (var selectId in selectMapping) {
            var index = selectMapping[selectId];
            var $select = $('#' + selectId);

            if (index == 2)
            {
                $select.empty();
                catalogs.results[index].forEach(function(item) {
                    $select.append(
                        $('<option>', {
                            text: item.description
                        })
                    );
                });
            } else {
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
        }

        $('#seUserStatus').val($('#seUserStatus').attr('option-selected'));
        $('#seEmployeeGender').val($('#seEmployeeGender').attr('option-selected'));
        $('#seUserRoles').val($('#seUserRoles').attr('option-selected'));
        var optionSelected = $('#seUserAccess').attr('option-selected');
        const selectElement = document.getElementById('seUserAccess');
        Array.from(selectElement.options).forEach(option => {
            if (optionSelected.includes(option.value))
                option.selected = true;
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

function update() {
    try {
        if(!isValidForm())
            return;

        $('#loader').show();
        fetch('update', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                id: $('#inUserId').val(),
                email: $('#inUserEmail').val(),
                firstname: $('#inUserFirstname').val(),
                lastname: $('#inUserLastname').val(),
                status: $('#seUserStatus').val(),
                userAccess: Array.from($('#seUserAccess')[0].options).filter(option => option.selected).map(option => option.value),
                userRole: $('#seUserRoles').val(),
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
        .then(response => {return response.json();})
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
                $('#loader').hide();
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
                showCloseButton:!1
            });
            $('#loader').hide();
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
        $('#loader').hide();
    }
}

function isValidForm() {
    try {
        var userId = $('#inUserId').val();
        var employeeId = $('#inEmployeeId').val();
        var email = $('#inUserEmail').val();
        var firstname = $('#inUserFirstname').val();
        var lastname = $('#inUserLastname').val();
        var status = $('#seUserStatus').val();
        var gender = $('#seEmployeeGender').val();
        var address = $('#inEmployeeAddress').val();
        var city = $('#inEmployeeCity').val();
        var state = $('#inEmployeeState').val();
        var zipcode = $('#inEmployeeZipcode').val();
        var jobPosition = $('#inEmployeeJobPosition').val();
        var profession = $('#inEmployeeProfession').val();
        var contactPhones = $('#inEmployeeContactPhones').val();
        var userAccess = Array.from($('#seUserAccess')[0].options).filter(option => option.selected).map(option => option.value);
        var userRoles = $('#seUserRoles').val();

        
        if (!userId || !employeeId || !email || !firstname || !lastname || !status || !gender || !address ||
            !address || !city || !state || !zipcode || !jobPosition || !profession || !contactPhones || !userAccess || userAccess.length == 0 || !userRoles) {
            Swal.fire({
                title: 'Error!!',
                html: 'The fields Email, First Name, Last Name, Status, Gender, Address, City, State, ZipCode, Job Position, Profession, Contact Phones, User Access and User Roles cannot be empty.',
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