function changeCardsInformation() {
    let active = document.getElementById("active");

    active.addEventListener("click", showActiveCards);
    active.style.backgroundColor = "#d9bf77";

    let expired = document.getElementById("expired");
    expired.addEventListener("click", showExpiredCards);

    function showActiveCards() {
        document.getElementById("active-cards").style.display = "block";
        document.getElementById("expired-cards").style.display = "none";
        active.classList.add("active");
        expired.classList.remove("active");

        active.style.backgroundColor = "#d9bf77";
        expired.style.backgroundColor = "";
    }

    function showExpiredCards() {
        document.getElementById("active-cards").style.display = "none";
        document.getElementById("expired-cards").style.display = "block";
        active.classList.remove("active");
        expired.classList.add("active");

        expired.style.backgroundColor = "#d9bf77";
        active.style.backgroundColor = "";
    }
}