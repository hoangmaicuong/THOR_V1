window.barcodeScanner = {
    open: async function () {
        return new Promise((resolve, reject) => {
            const video = document.createElement('video');
            video.style.width = '100%';
            video.style.height = '100%';
            video.setAttribute('autoplay', true);

            const overlay = document.createElement('div');
            overlay.style.position = 'fixed';
            overlay.style.top = 0;
            overlay.style.left = 0;
            overlay.style.width = '100%';
            overlay.style.height = '100%';
            overlay.style.background = 'rgba(0,0,0,0.8)';
            overlay.style.display = 'flex';
            overlay.style.justifyContent = 'center';
            overlay.style.alignItems = 'center';
            overlay.appendChild(video);
            document.body.appendChild(overlay);

            navigator.mediaDevices.getUserMedia({ video: { facingMode: "environment" } })
                .then(stream => {
                    video.srcObject = stream;
                    const codeReader = new ZXing.BrowserBarcodeReader();
                    codeReader.decodeOnceFromVideoDevice(undefined, video)
                        .then(result => {
                            stream.getTracks().forEach(t => t.stop());
                            document.body.removeChild(overlay);
                            resolve(result.text);
                        })
                        .catch(err => {
                            stream.getTracks().forEach(t => t.stop());
                            document.body.removeChild(overlay);
                            reject(err);
                        });
                })
                .catch(reject);
        });
    }
};