function showProcedureProducts() {
    let button = document.getElementById("button-show-info");
    button.addEventListener("click", showMore)

    function showMore() {
        let info = document.getElementsByClassName("show-more-products")[0];

        if (info.style.display === "block") {
            info.style.display = "none";
            button.innerText = "Show products";
        } else {
            info.style.display = "block";
            button.innerText = "Hide products";
        }
    }
}