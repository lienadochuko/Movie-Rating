async function resetjs() {
    try {
        const searchInput = document.getElementById('searchInput').value.trim();
        const errorSpan = document.getElementById('searchError');
        let formIsValid = true;

            errorSpan.textContent = "";
            errorSpan.textContent = "";

        if (formIsValid) {
            $("#global-loader").fadeIn("slow");
            console.log("Search input is valid");

            try {
                // Redirect to the search results page
                window.location.href = `https://localhost:7006/home/Index`;
            } catch (error) {
                console.error(error);
                throw error;
            }
        } else {
            console.log("Form has errors");
            //toastr.warning("Please ensure to properly fill all fields as indicated!");
        }
    } catch (error) {
        console.log(error);
        //toastr.error("An error occurred. Please try again.");
    } finally {
        $("#global-loader").fadeOut("slow");
    }
}

async function searchjs() {
    try {
        const searchInput = document.getElementById('searchInput').value.trim();
        const errorSpan = document.getElementById('searchError');
        let formIsValid = true;

        if (!searchInput) {
            errorSpan.textContent = "Search field cannot be empty.";
            formIsValid = false;
        } else if (!/^[a-zA-Z0-9\s]+$/.test(searchInput)) {
            errorSpan.textContent = "Search input contains invalid characters.";
            formIsValid = false;
        } else {
            errorSpan.textContent = "";
        }

        if (formIsValid) {
            $("#global-loader").fadeIn("slow");
            console.log("Search input is valid");

            try {
                // Redirect to the search results page
                window.location.href = `https://localhost:7006/home/Index?title=${encodeURIComponent(searchInput)}`;
            } catch (error) {
                console.error(error);
                throw error;
            }
        } else {
            console.log("Form has errors");
            //toastr.warning("Please ensure to properly fill all fields as indicated!");
        }
    } catch (error) {
        console.log(error);
        //toastr.error("An error occurred. Please try again.");
    } finally {
        $("#global-loader").fadeOut("slow");
    }
}


async function postRegisterData(data, token) {
    try {
        const controller = new AbortController();
        const signal = controller.signal;

        window.addEventListener('beforeunload', () => {
            controller.abort();
        });

        //console.log(data);
        const response = await fetch("https://localhost:7006/Simos/Register", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify(data),
            signal: signal
        });

        if (response.ok) {
            const result = await response.status;
            window.location.href = '/Auth/Login';
            $("#global-loader").fadeOut("slow");
        } else {
            window.location.href = '/Auth/Login';
            $("#global-loader").fadeOut("slow");
        }
    } catch (error) {
        console.error(error);
        throw error; // Re-throw the error for the caller to handle
    }
}

async function submitRegisterForm() {
    try {
        let formIsValid = true;
        $("#global-loader").fadeIn("slow");

        $('#myRegisterForm input, #myRegisterForm select, #myRegisterForm textarea').each(function () {
            let $this = $(this);
            let value = $this.val();
            let id = $this.attr('id');
            let name = $this.attr('name');
            let errorId = id + 'Error';
            let errorMessage = '';
            let password = $('#Password').val();
            let ConfirmPassword = $('#ConfirmPassword').val();

            if (password === ConfirmPassword) {
                if (id === 'FirstName' || id === 'LastName' || id === 'Email' || id === 'Password' || id === 'PhoneNumber'
                    || id === 'Gender' || id === 'UserType') {
                    if (!value || value === '' || value === null) {
                        errorMessage = 'Please input your ' + name;
                        formIsValid = false;
                        $('#' + errorId).text(errorMessage);
                        //console.log('Please input your ' + id);
                    } else if (Array.isArray(value) && value.length === 0) {
                        let errorMessage = 'Please select at least one company';
                        $('#' + id + 'Error').text(errorMessage);
                        formIsValid = false;
                        //console.log('Please input your ' + id);
                    }
                } else {
                    return true;
                }
            } else {
                errorMessage = 'Passwords do not match';
                formIsValid = false;
                $('#ConfirmPasswordError').text(errorMessage);
            }
        });

        if (formIsValid) {
            console.log('Form filled');
            const form = document.getElementById('myRegisterForm');
            const firstName = form.querySelector('[id="FirstName"]').value;
            const lastName = form.querySelector('[id="LastName"]').value;
            const email = form.querySelector('[id="Email"]').value;
            const password = form.querySelector('[id="Password"]').value;
            const confirmPassword = form.querySelector('[id="ConfirmPassword"]').value;
            const phoneNumber = form.querySelector('[id="PhoneNumber"]').value;
            const gender = form.querySelector('input[name="Gender"]:checked')?.value;
            const userType = form.querySelector('input[name="UserType"]:checked')?.value;

            let formData = {
                FirstName: firstName,
                LastName: lastName,
                Email: email,
                Password: password,
                ConfirmPassword: confirmPassword,
                PhoneNumber: phoneNumber,
                Gender: gender,
                UserType: userType,
            };

            // Get the anti-forgery token
            const antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();

            // Add token to headers and send the request
            await postRegisterData(formData, antiForgeryToken);
        }
        else {
            console.log('Form has errors');
            //toastr.warning("Please ensure to properly fill all fields as indicated!");

        }
    }
    catch (error) {
        console.log(error);
        //toastr.error("An error occurred pls try again");
    }
    finally {
        $("#global-loader").fadeOut("slow");
    }

}
async function postData(data, token) {
    try {
        const controller = new AbortController();
        const signal = controller.signal;

        window.addEventListener('beforeunload', () => {
            controller.abort();
        });

        //console.log(data);
        const response = await fetch("https://localhost:7006/Simos/Login", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify(data),
            signal: signal
        });

        if (response.ok) {
            console.log(response.status);
            window.location.href = '/home/Index';
            $("#global-loader").fadeOut("slow");
        } else if (response.status === 401) {
            console.log("")
            window.location.href = '/home/Index';
        } else if (response.status === 400) {
            let message = await response.text();
            let IsRecNotFound = message == '"No records found!"';
            console.log(IsRecNotFound ? 'No records found!' : 'The details provided were invalid!');
            window.location.href = '/home/Index';
        } else {
            console.log('Internal Server Error');
            window.location.href = '/home/Index';
        }
    } catch (error) {
        console.log(error);
    }
}


async function submitForm() {
    try {
        let formIsValid = true;
        $("#global-loader").fadeIn("slow");

        $('#myLoginForm input, #myLoginForm select, #myLoginForm textarea').each(function () {
            let $this = $(this);
            let value = $this.val();
            let id = $this.attr('id');
            let name = $this.attr('name');
            let errorId = id + 'Error';
            let errorMessage = '';
            let Empty = []


            if (id === 'Email' || id === 'Password') {
                if (!value || value === '' || value === null) {
                    errorMessage = 'Please input your ' + name;
                    formIsValid = false;
                    $('#' + errorId).text(errorMessage);
                    //console.log('Please input your ' + id);
                } else if (Array.isArray(value) && value.length === 0) {
                    let errorMessage = 'Please select at least one company';
                    $('#' + id + 'Error').text(errorMessage);
                    formIsValid = false;
                    //console.log('Please input your ' + id);
                }
            } else {
                return true;
            }
        });

        if (formIsValid) {
            console.log('Form filled');

            let formData = {
                Email: $('#Email').val(),
                Password: $('#Password').val(),
            };

            // Get the anti-forgery token
            const antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();

            // Add token to headers and send the request
            await postData(formData, antiForgeryToken);
        }
        else {
            console.log('Form has errors');
            //toastr.warning("Please ensure to properly fill all fields as indicated!");

        }
    }
    catch (error) {
        console.log(error);
        //toastr.error("An error occurred pls try again");
    }
    finally {
        $("#global-loader").fadeOut("slow");
    }

}

function validateEmail(input) {
    //const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    input.value = input.value.replace(/[^a-zA-Z0-9@._\-]/g, '');

}
function validate(input) {
    input.value = input.value.replace(/[^a-zA-Z0-9_\-]/g, '');
    const errorSpan = document.getElementById('searchError');
    errorSpan.textContent = "";

}
function validateNo(input) {
    input.value = input.value.replace(/[^a-zA-Z0-9_\-]/g, '');

}