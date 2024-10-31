async function fileToBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result); // Base64 data URL
        reader.onerror = error => reject(error);
    });
}

async function fileToBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}

async function resizeImage(base64Data, width, height) {
    return new Promise((resolve, reject) => {
        const img = new Image();
        img.src = base64Data;
        img.onload = () => {
            const canvas = document.createElement('canvas');
            canvas.width = width;
            canvas.height = height;
            const ctx = canvas.getContext('2d');
            ctx.drawImage(img, 0, 0, width, height);
            resolve(canvas.toDataURL().split(',')[1]); // Return only the base64 part
        };
        img.onerror = error => reject(error);
    });
}

async function convert() {
    const fileInput = document.getElementById('MovieImg');
    const errorMessage = document.getElementById('errorMessage');
    const preview = document.getElementById('preview');

    errorMessage.textContent = '';
    preview.innerHTML = '';

    if (!fileInput || !fileInput.files || fileInput.files.length === 0) {
        errorMessage.textContent = 'Please select a file to upload.';
        return;
    }

    const file = fileInput.files[0];
    try {
        const base64Data = await fileToBase64(file);

        // Resize image to 206.25x305.25
        const resizedBase64Data = await resizeImage(base64Data, 206.25, 305.25);
        console.log('Resized Base64 data:', resizedBase64Data);

        // Display the resized image preview
        const img = document.createElement('img');
        img.src = `data:image/png;base64,${resizedBase64Data}`;
        img.alt = "Resized Image Preview";
        img.width = 206.25;
        img.height = 305.25;
        preview.appendChild(img);

    } catch (error) {
        console.error('Error processing the image:', error);
        errorMessage.textContent = 'Error processing the image. Please try again.';
    }
}


//async function convert() {
//    const fileInput = document.getElementById('MovieImg');
//    const errorMessage = document.getElementById('errorMessage');
//    const preview = document.getElementById('preview');

//    errorMessage.textContent = ''; // Clear previous error messages
//    preview.innerHTML = ''; // Clear previous preview

//    if (!fileInput || !fileInput.files || fileInput.files.length === 0) {
//        errorMessage.textContent = 'Please select a file to upload.';
//        return;
//    }

//    const file = fileInput.files[0];
//    try {
//        const base64Data = await fileToBase64(file);
//        console.log('Base64 data:', base64Data);

//        // Display preview if it's an image
//        if (file.type.startsWith('image/')) {
//            const img = document.createElement('img');
//            img.src = base64Data;
//            img.alt = "Image preview";
//            img.style.maxWidth = "200px";
//            preview.appendChild(img);
//        }

//    } catch (error) {
//        console.error('Error reading file as Base64:', error);
//        errorMessage.textContent = 'Error reading file. Please try again.';
//    }
//}


//async function fileToBinary(file) {
//        return new Promise((resolve, reject) => {
//            const reader = new FileReader();

//            // Read the file as an ArrayBuffer (raw binary data)
//            reader.readAsArrayBuffer(file);

//            reader.onload = () => {
//                const arrayBuffer = reader.result;  // This is the binary data
//                resolve(arrayBuffer);
//            };

//            reader.onerror = error => reject(error);
//        });
//}
