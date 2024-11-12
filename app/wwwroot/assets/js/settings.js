$(document).ready(function() {
    $('#aSettings').click(function(e) {
        e.preventDefault();
        window.location.href = window.location.origin + '/settings/user';
    });
});