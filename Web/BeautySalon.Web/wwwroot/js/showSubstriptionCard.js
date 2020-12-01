function showSubstriptionCard() {
    var cardButton = document.getElementById("show-card");
    cardButton.addEventListener("click", showCard);

    function showCard() {
        let card = document.getElementById("qrCode");

        if (card.style.display === "block") {
            card.style.display = "none";
            cardButton.innerText = "Show card";
        } else {
            card.style.display = "block";
            cardButton.innerText = "Hide card";
        }
    }
}