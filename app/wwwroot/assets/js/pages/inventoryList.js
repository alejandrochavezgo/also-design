$(document).ready(function () {
    try {
        $.ajax({
            url: 'getAll',
            method: 'GET',
            success: function (data) {
                if (!data.isSuccess) {
                    Swal.fire({
                        title: 'Error!!',
                        html: data.message,
                        icon: 'error',
                        confirmButtonClass: 'btn btn-danger w-xs mt-2',
                        buttonsStyling: false,
                        footer: '',
                        showCloseButton: true
                    });
                }
                var tbody = $('#tbItems tbody');
                tbody.empty();
                data.results.forEach(function (item) {
                    var row =
                        '<tr>' +
                        '<td>' +
                            '<div class="d-flex align-items-center">' +
                                '<div class="flex-shrink-0 me-3">' +
                                    '<div class="avatar-sm bg-light rounded p-1">' +
                                        '<img src="'+ item.imagePath +'" alt="" class="img-fluid d-block">' +
                                    '</div>' +
                                '</div>' +
                                '<div class="flex-grow-1">' +
                                    '<h5 class="fs-14 mb-1">' +
                                        '<a href="\'/inventory/details?id=' + item.id + '\'" class="text-dark">' + item.productName + '</a>' +
                                    '</h5>' +
                                    '<p class="text-muted mb-0">Code : <span class="fw-medium">' + item.productCode + '</span></p>' +
                                '</div>' +
                            '</div>' +
                        '</td>' +
                        '<td>' + item.stock + '</td>' +
                        '<td>' + item.price + '</td>' +
                        '<td>' + item.lastRestock + '</td>' +
                        '<td class="text-center">' +
                            '<button type="button" class="btn btn-secondary btn-icon waves-effect waves-light mx-1" onclick="window.location.href=\'/inventory/details?id=' + item.id + '\'"><i class="ri-eye-fill"></i></button>' +
                            '<button type="button" class="btn btn-primary btn-icon waves-effect waves-light mx-1"><i class="ri-pencil-fill" onclick="window.location.href=\'/inventory/update?id=' + item.id + '\'"></i></button>' +
                            '<button type="button" class="btn btn-danger btn-icon waves-effect waves-light mx-1"><i class="ri-delete-bin-fill" onclick="delete(' + item.id + ')"></i></button>' +
                        '</td>' +
                        '</tr>';
                    
                    tbody.append(row);
                });

                $('#dvTotalClients').html('Total clients: ' + data.results.length);
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    title: 'Error!!',
                    html: error,
                    icon: 'error',
                    confirmButtonClass: 'btn btn-danger w-xs mt-2',
                    buttonsStyling: false,
                    footer: '',
                    showCloseButton: true
                });
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
});