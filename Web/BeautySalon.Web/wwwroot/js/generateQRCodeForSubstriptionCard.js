function generateQRCodeForSubstriptionCard() {
    let clientId = document.getElementById("client-id").innerText;
    let startDate = document.getElementById("start-date").innerHTML.slice(26);
    let endDate = document.getElementById("end-date").innerHTML.slice(26);

    new QRCode(document.getElementById("qrCode"), {
        text: `${clientId}: Valid peripd: ${startDate} - ${endDate}.`,
        width: 250,
        height: 250,
        colorDark: "#d9bf77",
        colorLight: "#ffffff",
    });
}
