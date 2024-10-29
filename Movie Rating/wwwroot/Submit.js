//fetch('http://localhost:5119/Simos', {
//    method: 'POST',
//    headers: {
//        'Content-Type': 'application/json'
//    },
//    body: JSON.stringify({
//        email: 'john.doe@example.com',
//        password: 'SecurePassword123!'
//    })
//})
//    .then(response => response.json())
//    .then(data => {
//        if (data) {
//            // Save the token or user data locally (e.g., in localStorage)
//            //localStorage.setItem('jwtToken', data.token);

//            // Redirect to the home page
//            window.location.href = '/home';
//        }
//    })
//    .catch(error => console.error('Error:', error));



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
            const result = await response.status;
            window.location.href = '/home/Index';
            $("#global-loader").fadeOut("slow");
        } else if (response.status === 401) {
            return new Promise((resolve) => {
                toastr.error('Session expired!', {
                    onClose: function () {
                        location.reload();
                        resolve();
                    }
                });
            });
        } else if (response.status === 400) {
            let message = await response.text();
            let IsRecNotFound = message == '"No records found!"';
            throw new Error(IsRecNotFound ? 'No records found!' : 'The details provided were invalid!');
        } else {
            throw new Error('Internal Server Error');
        }
    } catch (error) {
        console.error(error);
        throw error; // Re-throw the error for the caller to handle
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