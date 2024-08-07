$(document).ready(function () {
    try {
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
                            '<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/user/update?id=' + user.id + '\'"><i class="ri-pencil-fill"></i></button>' +
                        '</td>' +
                    '</tr>';
                    tbody.append(row);
                });
                $('#dvTotalUsers').html('Total users: ' + data.results.length);
            }, 
            error: function (xhr, status, error) {
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