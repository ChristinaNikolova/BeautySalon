function showCardStatistic() {
    let statisticButton = document.getElementById("show-statistic");
    statisticButton.addEventListener("click", showStatistic);

    function showStatistic() {
        let statistic = document.getElementById("statistic");

        if (statistic.style.display === "block") {
            statistic.style.display = "none"
            statisticButton.innerText = "Show Statistic";
        } else {
            statistic.style.display = "block"
            statisticButton.innerText = "Hide Statistic";
        }
    }
}
