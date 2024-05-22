/*
 * Copyright© 2017 Cross Border Xpress
 * All rights reserved.
 * Total or partial distribution is prohibited.
*/

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
            $("#submitText").html('Validating...');
            $("#submitStatus").removeClass('display-none');
            clearValidationsSummary();

            if ($("#username").val() == '' || $("#username").val() == null || $("#username").val() == undefined) {
                restartSignInProcess();
                addValidationMessage("* You forgot to put the username above.");   
                return;           
            }

            if ($("#password").val() == '' || $("#password").val() == null || $("#password").val() == undefined) {
                restartSignInProcess();
                addValidationMessage("* You forgot to put the password above.");
                return;
            }

            var payload = {
                username: $("#username").val(),
                password: $("#password").val(),
                rememberMe: $('#rememberMe').is(":checked")
            };

            fetch("Login/Login", {
                method: 'post',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(payload),
            })
            .then(response => response.json())
            .then(data => {
                if (!data.isSuccess) {
                    Swal.fire({
                        title: 'Oops!',
                        icon: 'error',
                        html: 'An unexpected error has ocurred :(<br>' + data.message,
                    });
                }
            })
            .catch(exception => {
                console.log("catch");
                console.log(exception);
                Swal.fire({
                    title: 'Oops!',
                    icon: 'error',
                    html: 'An unexpected error has ocurred :(<br>Please refresh this page and try again.',
                });
            });
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

    $("#password").keypress(function (event) {
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

    function revealPassword() {
        try {
            if ($('#password').prop('type') == 'text') {
                $("#password").prop('type', 'password');
            }
            else {
                $("#password").prop('type', 'text');
            }
        }
        catch(exception)
        {
            console.log(exception);
            Swal.fire({
                title: 'Oops!',
                icon: 'error',
                html: 'An unexpected error has ocurred :(<br>Please refresh this page and try again.',
            });
        }
    }
});