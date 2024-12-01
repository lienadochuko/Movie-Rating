async function likeAction(data) {
    try {
        const controller = new AbortController();
        const signal = controller.signal;

        window.addEventListener('beforeunload', () => {
            controller.abort();
        });

        //console.log(data);
        const response = await fetch(`/Simos/Like/${data}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
            signal: signal
        });

        if (response.ok) {
            console.log(response.status);
            $("#global-loader").fadeOut("slow");
        } else if (response.status === 401) {
            console.log("")
        } else if (response.status === 400) {
            let message = await response.text();
            let IsRecNotFound = message == '"No records found!"';
            console.log(IsRecNotFound ? 'No records found!' : 'The details provided were invalid!');
        } else {
            console.log('Internal Server Error');
        }
    } catch (error) {
        console.log(error);
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