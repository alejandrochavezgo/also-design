$(document).ready(function() {
    $('#alogout').click(function(e) {
        e.preventDefault();
        logout();
    });

    function logout() {
        document.cookie = 'userCookie=; Max-Age=0; path=/';
        fetch('/login/logout', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json',
            }
        })
        .then(response => response.json() )
        .then(data => {
            if (!data.isSuccess) {
                Swal.fire({
                    title: 'Error!!',
                    html: error,
                    icon: 'error',
                    confirmButtonClass: 'btn btn-danger w-xs mt-2',
                    buttonsStyling: false,
                    footer: '',
                    showCloseButton: true
                }).then(function (t) {
                    location.reload(true);
                });
            }
            window.location.href = data.url;
        })
        .catch(error => {
            Swal.fire({
                title: 'Error!!',
                html: error,
                icon: 'error',
                confirmButtonClass: 'btn btn-danger w-xs mt-2',
                buttonsStyling: false,
                footer: '',
                showCloseButton: true
            }).then(function (t) {
                location.reload(true);
            });
        });
    }
});