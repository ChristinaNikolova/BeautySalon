function searchProcedures() {
    let searchButton = document.getElementById("searchBy");
    searchButton.addEventListener("click", SearchBy);

    let clearButton = document.getElementById("clearSearch");
    clearButton.addEventListener("click", ClearSearch);

    function ClearSearch() {
        window.location.reload();
    }

    function SearchBy(event) {
        let pagination = document.getElementById("procedure-pagination");
        pagination.style.display = "none";

        let parentElementSkinType = Array.from(event.target.parentElement.children[0].children);

        parentElementSkinType.shift();
        let skinTypeId = "";

        parentElementSkinType.forEach(option => {

            if (option.children[0].checked === true) {
                skinTypeId = option.children[0].value;
            }
        });

        let parentElementCriteria = Array.from(event.target.parentElement.children[1].children);

        parentElementCriteria.shift();
        var criteria = "";

        parentElementCriteria.forEach(option => {
            if (option.children[0].checked === true) {
                criteria = option.children[0].value;
            }
        });

        var categoryName = document.getElementsByClassName("category-name")[0].innerHTML;

        var token = $(".search-bar input[name=__RequestVerificationToken]").val();

        $.ajax({
            url: "/Procedures/SearchBy/",
            type: "POST",
            data: JSON.stringify({ skinTypeId, criteria, categoryName }),
            contentType: "application/json; charset=utf-8",
            headers: { 'X-CSRF-TOKEN': token },
            dataType: "json",
            success: function (data) {
                let result = '<div class="row">';
                data.procedures.forEach(procedure => {
                    result += '<div class="col-md-4">' + '<div class="block-7">' + '<div class="text-center">' + `<h3 class="heading-2 my-4 heading-color">${procedure.name}</h3>` + `<span class="price ">€${procedure.formattedPrice}</span>` + '<ul class="pricing-text mb-5">' + `<li><strong>Category:</strong> ${procedure.categoryName}</li>` + ` <li><strong>Skin Type:</strong> ${procedure.skinTypeToDisplay}</li>` + `<li><strong>Rating: </strong>${procedure.formattedRating}</li>` + '</ul>' + `<a class="btn btn-primary d-block px-2 py-4 custom-hover" href="/Procedures/GetDetails/${procedure.id}"">Details</a>` + '</div>' + '</div>' + '</div>';
                });

                result += '</div>'

                $('#showByCategories').html(result);
            }
        });
    }
}