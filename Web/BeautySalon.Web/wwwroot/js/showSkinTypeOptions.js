function showSkinTypeOptions() {

    window.onload = function () {


        let category = document.getElementById("category");

        [...category.children].forEach(child => {
            if (child.innerText !== "Please select beauty category") {
                child.selected = false;
            }
        });
    }

    let category = document.getElementById("category");
    category.addEventListener("change", checkCategory);

    let categorySkinCareId = document.getElementById("category-skin-care-id").innerText;

    function checkCategory() {
        if (category.value === categorySkinCareId) {
            document.getElementById("is-sensitive").style.display = "block";
            document.getElementById("skin-type").style.display = "block";
            document.getElementById("skin-problem").style.display = "block";
        } else {
            document.getElementById("is-sensitive").style.display = "none";
            document.getElementById("skin-type").style.display = "none";
            document.getElementById("skin-problem").style.display = "none";
        }
    }
}