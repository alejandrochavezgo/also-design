$(document).ready(function () {
    $("#password-addon").click(function () {
        try {
            if ($('#password').prop('type') == 'text') {
                $("#password").prop('type', 'password');
            }
            else {
                $("#password").prop('type', 'text');
            }
        }
        catch(exception) {
            Swal.fire(
                'Oops!',
                'An error has ocurred when revealing the password.',
                'error'
              );
        }
    });

    $("#btnSubmit").on("click", function (event) {
        try {
            event.preventDefault();
            $(this).prop('disabled', true);
            clearValidationsSummary();

            if ($("#email").val() == '' || $("#email").val() == null || $("#email").val() == undefined) {
                restartSignInProcess();
                addValidationMessage("* You forgot to put the email above.");   
                return;           
            }

            if ($("#email").val() == '' || $("#email").val() == null || $("#email").val() == undefined) {
                restartSignInProcess();
                addValidationMessage("* You forgot to put the username above.");   
                return;           
            }

            if (!(/^[^\s@]+@[^\s@]+\.[^\s@]+$/).test($("#email").val())) {
                restartSignInProcess();
                addValidationMessage("* The email has an invalid format.");   
                return;
            }

            $(this).prop('disabled', false);

            // var payload = {
            //     email: $("#email").val(),
            // };

            // fetch("login/passwordReset", {
            //     method: 'post',
            //     headers: {
            //         'Content-Type': 'application/json',
            //     },
            //     body: JSON.stringify(payload),
            // })
            // .then(response => response.json())
            // .then(data => {
            //     if (!data.isSuccess) {
            //         Swal.fire({
            //             title: 'Oops!',
            //             icon: 'error',
            //             html: 'An unexpected error has ocurred :(<br>' + data.message,
            //         });
            //     }
            // })
            // .catch(exception => {
            //     console.log("catch");
            //     console.log(exception);
            //     Swal.fire({
            //         title: 'Oops!',
            //         icon: 'error',
            //         html: 'An unexpected error has ocurred :(<br>Please refresh this page and try again.',
            //     });
            // });
        }
        catch(exception) {
            console.log(exception);
                Swal.fire({
                    title: 'Oops!',
                    icon: 'error',
                    html: 'An unexpected error has ocurred :(<br>Please refresh this page and try again.',
                });
        } 
    });

    $("#email").keypress(function (event) {
        if (event.keyCode == 13) {
            $("#btnSubmit").click();
        }
    });

    function addValidationMessage(msg) {
        try {
            if($("#validationsSummary").hasClass('display-none')) 
                $("#validationsSummary").removeClass("display-none");
            if($("#itemsSummary").text() == "")
                $("#itemsSummary").html('<muted>' + msg + '</muted>');
            else
                $("#itemsSummary").html($("#itemsSummary").html() + '<br><muted>' + msg + '</muted>');
        } catch (exception) {
            console.log(exception);
            Swal.fire({
                title: 'Oops!',
                icon: 'error',
                html: 'An unexpected error has ocurred :(<br>Please refresh this page and try again.',
            });
        }
    }

    function restartSignInProcess() {
        try {
            $("#btnSubmit").prop('disabled', false);
            $("#submitText").html('Sign In');
            $("#submitStatus").addClass('display-none');
        } catch (exception) {
            console.log(exception);
            Swal.fire({
                title: 'Oops!',
                icon: 'error',
                html: 'An unexpected error has ocurred :(<br>Please refresh this page and try again.',
            });
        }
    }

    function clearValidationsSummary() {
        try {
            if(!$("#validationsSummary").hasClass('display-none')) 
                $("#validationsSummary").addClass("display-none");
            $("#itemsSummary").text('');
        } catch (exception) {
            console.log(exception);
            Swal.fire({
                title: 'Oops!',
                icon: 'error',
                html: 'An unexpected error has ocurred :(<br>Please refresh this page and try again.',
            });
        }
    }
});