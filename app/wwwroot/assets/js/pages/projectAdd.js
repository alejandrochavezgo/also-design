document.querySelectorAll('.uppercase-input').forEach(input => {
    input.addEventListener('input', function() {
        this.value = this.value.toUpperCase();
    });
});

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
        if (!catalogs || !catalogs.isSuccess || catalogs.results.length !== 1) {
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
            seStatus: 0
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
        var name = $('#inName').val();
        var clientId = $('#inClientBusinessName').attr('clientId');
        var description = $('#taDescription').val();
        var startDate = $('#inStartDate').val();
        var endDate = $('#inEndDate').val();
        var status = $('#seStatus').val();

        let project = {
            name,
            client : {
                id: clientId
            },
            description,
            startDate,
            endDate,
            status
        };

        if(!isValidForm(project))
            return;

        $('#loader').show();
        fetch('add', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(project)
        })
        .then(response => {return response.json()})
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
        $('#loader').hide();
    }
}

function isValidForm(project) {
    try {
        if (!project.name || !project.client || project.client.id <= 0 || !project.description || !project.startDate || !project.endDate || !project.status) {
            Swal.fire({
                title: 'Error!!',
                html: 'The fields Name, Client, Description, Start Date, End Date and Status cannot be empty.',
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: !1,
                footer: '',
                showCloseButton: !1
            });
            return false;
        }

        var today = new Date();
        today.setHours(0, 0, 0, 0);
        var startDate = new Date(project.startDate + 'T00:00:00-08:00');
        var endDate = new Date(project.endDate + 'T00:00:00-08:00');

        if (startDate < today) {
            Swal.fire({
                title: 'Error!!',
                html: 'The Start Date cannot be earlier than today.',
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
                $('#inClientBusinessName').val(ui.item.id);
                $('#inClientBusinessName').attr('clientId', ui.item.id);
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
    $('#loader').show();
    initializeClientAutocomplete();
    initializeCatalogs().then(() => {
        $('#loader').hide();
    });
});