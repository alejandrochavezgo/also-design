$(document).ready(function () {
    try {
        $('#tbEmployees tbody').html(
            '<tr>' +
                '<td colspan="8" class="text-center">Rendering results... <img width="30" src="../assets/images/infinity.gif"></td>' +
            '</tr>');

        $.ajax({
            url: 'getAll',
            method: 'GET',
            success: function (data) {
                if(!data.isSuccess) {
                    Swal.fire({
                        title: 'Error!!',
                        html: data.message,
                        icon: 'error',
                        confirmButtonClass: 'btn btn-danger w-xs mt-2',
                        buttonsStyling: !1,
                        footer: '',
                        showCloseButton:!0
                    });
                }

                var tbody = $('#tbEmployees tbody');
                tbody.empty();

                if (data.results.length > 0) {
                    data.results.forEach(function (nonUser) {
                        var row =
                        '<tr>' +
                            '<td>' + nonUser.employee.code + '</td>' +
                            '<td>' + nonUser.email + '</td>' +
                            '<td>' + nonUser.firstname + '</td>' +
                            '<td>' + nonUser.lastname + '</td>' +
                            '<td><span class="badge rounded-pill badge-soft-' + nonUser.statusColor + '">' + nonUser.statusName + '</span></td>' +
                            '<td>' + nonUser.creationDateAsString + '</td>' +
                            '<td class="' + (nonUser.modificationDateAsString === '-' ? 'text-center' : 'text-left') + '">' + nonUser.modificationDateAsString + '</td>' +
                            '<td class="text-center">' +
                                '<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/employee/update?id=' + nonUser.id + '\'" title="Update"><i class="ri-pencil-fill"></i></button>' +
                                '<button type="button" class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal(' + nonUser.id  + ')" title="Delete"><i class="ri-delete-bin-2-fill"></i></button>' +
                                '<button type="button" class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/employee/detail?id=' + nonUser.id + '\'" title="View"><i class="ri-eye-fill"></i></button>' +
                            '</td>' +
                        '</tr>';
                        tbody.append(row);
                    });
                } else {
                    $('#tbEmployees tbody').html(
                        '<tr>' +
                            '<td colspan="8" class="text-center">We have nothing to show.</td>' +
                        '</tr>');
                }
                $('#dvTotalEmployees').html('Total employees: ' + data.results.length);
            }, 
            error: function (xhr, status, error) {
                $('#tbEmployees tbody').html(
                    '<tr>' +
                        '<td colspan="8" class="text-center">We have nothing to show.</td>' +
                    '</tr>');
                Swal.fire({
                    title: 'Error!!',
                    html: error,
                    icon: 'error',
                    confirmButtonClass: 'btn btn-danger w-xs mt-2',
                    buttonsStyling: !1,
                    footer: '',
                    showCloseButton:!0
                });
            }
        });
    }
    catch(exception) {
        $('#tbEmployees tbody').html(
            '<tr>' +
                '<td colspan="8" class="text-center">We have nothing to show.</td>' +
            '</tr>');
        Swal.fire({
            title: 'Error!!',
            html: exception,
            icon: 'error',
            confirmButtonClass: 'btn btn-danger w-xs mt-2',
            buttonsStyling: !1,
            footer: '',
            showCloseButton:!0
        });
    }
});