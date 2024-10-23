$(document).ready(function () {
    try {
        $('#tbUsers tbody').html(
            '<tr>' +
                '<td colspan="9" class="text-center">Rendering results... <img width="30" src="../assets/images/infinity.gif"></td>' +
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

                var tbody = $('#tbUsers tbody');
                tbody.empty();

                if (data.results.length > 0) {
                    data.results.forEach(function (user) {
                        var row =
                        '<tr>' +
                            '<td>' + user.id + '</td>' +
                            '<td>' + user.email + '</td>' +
                            '<td>' + user.username + '</td>' +
                            '<td>' + user.firstname + '</td>' +
                            '<td>' + user.lastname + '</td>' +
                            '<td><span class="badge rounded-pill badge-soft-' + user.statusColor + '">' + user.statusName + '</span></td>' +
                            '<td>' + user.creationDateAsString + '</td>' +
                            '<td class="' + (user.modificationDateAsString === '-' ? 'text-center' : 'text-left') + '">' + user.modificationDateAsString + '</td>' +
                            '<td class="text-center">' +
                                '<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/user/update?id=' + user.id + '\'" title="Update"><i class="ri-pencil-fill"></i></button>' +
                                '<button type="button" class="btn btn-danger btn-icon waves-effect waves-light mx-1" onclick="showDeleteModal(' + user.id  + ')" title="Delete"><i class="ri-delete-bin-2-fill"></i></button>' +
                                '<button type="button" class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/user/detail?id=' + user.id + '\'" title="View"><i class="ri-eye-fill"></i></button>' +
                            '</td>' +
                        '</tr>';
                        tbody.append(row);
                    });
                } else {
                    $('#tbUsers tbody').html(
                        '<tr>' +
                            '<td colspan="9" class="text-center">We have nothing to show.</td>' +
                        '</tr>');
                }
                $('#dvTotalUsers').html('Total users: ' + data.results.length);
            }, 
            error: function (xhr, status, error) {
                $('#tbUsers tbody').html(
                    '<tr>' +
                        '<td colspan="9" class="text-center">We have nothing to show.</td>' +
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
        $('#tbUsers tbody').html(
            '<tr>' +
                '<td colspan="9" class="text-center">We have nothing to show.</td>' +
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