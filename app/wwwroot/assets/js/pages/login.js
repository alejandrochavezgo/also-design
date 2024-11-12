$(document).ready(function () {
    document.querySelectorAll('.uppercase-input').forEach(input => {
        input.addEventListener('input', function() {
            this.value = this.value.toUpperCase();
        });
    });
    
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
            restartSignInProcess();
            addValidationMessage("An unexpected error has ocurred :(<br>Please refresh this page and try again.");
            return;
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
                addValidationMessage("You forgot to put the username above.");
                return;
            }

            if ($("#password").val() == '' || $("#password").val() == null || $("#password").val() == undefined) {
                restartSignInProcess();
                addValidationMessage("You forgot to put the password above.");
                return;
            }

            var payload = {
                username: $("#username").val(),
                password: $("#password").val(),
            };

            fetch('login/login', {
                method: 'post',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(payload),
            })
            .then(response => response.json())
            .then(data => {
                if (!data.isSuccess) {
                    restartSignInProcess();
                    addValidationMessage(data.message);
                    return;
                }
                window.location.href = data.url;
            })
            .catch(exception => {
                restartSignInProcess();
                addValidationMessage('An unexpected error has ocurred :(<br>Please refresh this page and try again.');
            });
        }
        catch(exception) {
            restartSignInProcess();
            addValidationMessage('An unexpected error has ocurred :(<br>Please refresh this page and try again.');
        }
    });

    $("#password").keypress(function (event) {
        if (event.keyCode == 13) {
            $('#btnSubmit').click();
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
            restartSignInProcess();
            addValidationMessage("An unexpected error has ocurred :(<br>Please refresh this page and try again.");
        }
    }

    function restartSignInProcess() {
        try {
            $("#btnSubmit").prop('disabled', false);
            $("#submitText").html('Sign In');
            $("#submitStatus").addClass('display-none');
        } catch (exception) {
            restartSignInProcess();
            addValidationMessage("An unexpected error has ocurred :(<br>Please refresh this page and try again.");
        }
    }

    function clearValidationsSummary() {
        try {
            if(!$("#validationsSummary").hasClass('display-none')) 
                $("#validationsSummary").addClass('display-none');
            $("#itemsSummary").text('');
        } catch (exception) {
            restartSignInProcess();
            addValidationMessage("An unexpected error has ocurred :(<br>Please refresh this page and try again.");
        }
    }

    function revealPassword() {
        try {
            if ($('#password').prop('type') == 'text') {
                $('#password').prop('type', 'password');
            }
            else {
                $('#password').prop('type', 'text');
            }
        }
        catch(exception)
        {
            restartSignInProcess();
            addValidationMessage("An unexpected error has ocurred :(<br>Please refresh this page and try again.");
        }
    }
});