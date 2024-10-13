
async function fileToBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result.split(',')[1]);
        reader.onerror = error => reject(error);
    });
}

async function convert() {
    const file = $('#MovieImg')[0].files[0];
    try {
        const binaryData = await fileToBase64(file);
        console.log('Binary data:', binaryData);

        // You can now send this binary data via a network request or process it as needed
    } catch (error) {
        console.error('Error reading file as binary:', error);
    }
}

async function fileToBinary(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();

            // Read the file as an ArrayBuffer (raw binary data)
            reader.readAsArrayBuffer(file);

            reader.onload = () => {
                const arrayBuffer = reader.result;  // This is the binary data
                resolve(arrayBuffer);
            };

            reader.onerror = error => reject(error);
        });
}
