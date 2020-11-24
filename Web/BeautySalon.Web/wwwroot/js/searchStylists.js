function searchStylists() {
    let searchButton = document.getElementById("searchBy");
    searchButton.addEventListener("click", SearchBy);

    let clearButton = document.getElementById("clearSearch");
    clearButton.addEventListener("click", ClearSearch);

    function ClearSearch() {
        window.location.reload();
    }

    function SearchBy(event) {
        let parentElementSkinType = Array.from(event.target.parentElement.children[0].children);

        parentElementSkinType.shift();
        let categoryId = "";

        parentElementSkinType.forEach(option => {

            if (option.children[0].checked === true) {
                categoryId = option.children[0].value;
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

        var token = $(".search-bar input[name=__RequestVerificationToken]").val();
        debugger;
        $.ajax({
            url: "/Stylists/SearchBy/",
            type: "POST",
            data: JSON.stringify({ categoryId, criteria }),
            contentType: "application/json; charset=utf-8",
            headers: { 'X-CSRF-TOKEN': token },
            dataType: "json",
            success: function (data) {
                let result = '';

                data.stylists.forEach(stylist => {
                    result += '<div class="col-lg-3 d-flex">' + '<div class="coach align-items-stretch">' +
                        `<div class="img" style="background-image: url(${stylist.picture});"></div>` +
                        '<div class="text bg-white p-4">' + `<h3><a href="/Stylists/GetDetails/${stylist.id}">${stylist.firstName} ${stylist.lastName}</a></h3>` + ` <div class="subheading"><strong>Category:</strong> ${stylist.categoryName}</div>` + `<div class="subheading"><strong>Speciality:</strong> ${stylist.jobTypeName}</div>` + `<strong>About me</strong><p class="about-me">${stylist.shortDescription}</p>` + `<a class="btn btn-primary p-1 px-1 py-1 mr-md-1 log" href="/Stylists/GetDetails/${stylist.id}">More info</a>` + ' <p></p>' +
                        '</div>' + '</div>' + '</div>'
                });

                $('#showResult').html(result);
            }
        });
    }
}