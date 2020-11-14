function showAppoitmentsRequests() {
    let requestButton = document.getElementById("request-appointment");
    requestButton.addEventListener("click", showRequests);

    function showRequests() {
        let requests = document.getElementById("requests");

        if (requests.style.display === "block") {
            requests.style.display = "none";
            requestButton.innerText = "Show";
        } else {
            requests.style.display = "block";
            requestButton.innerText = "Hide";
        }

    }
}