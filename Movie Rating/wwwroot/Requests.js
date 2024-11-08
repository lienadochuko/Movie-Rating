async function UpdateFilmPosters() {
    $("#global-loader").fadeIn("slow");
    try {
        const controller = new AbortController();
        const signal = controller.signal;

        window.addEventListener('beforeunload', () => {
            controller.abort();
        });

        //console.log(data);
        const response = await fetch("https://localhost:7006/data/UpdateFilmPosters", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
                
            },
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