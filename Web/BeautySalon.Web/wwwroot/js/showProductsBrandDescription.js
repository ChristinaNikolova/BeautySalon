function showProductsBrandDescription() {
    document.getElementById("brand-name").addEventListener("click", showBrandDescroption);

    function showBrandDescroption() {
        let brandDescription = document.getElementById("brand-description");

        if (brandDescription.style.display === "none") {
            brandDescription.style.display = "block";
        } else {
            brandDescription.style.display = "none";
        }
    }
}