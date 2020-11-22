function searchArticles() {
    let searchButton = document.getElementById("searchBy");
    searchButton.addEventListener("click", SearchBy);

    let clearButton = document.getElementById("clearSearch");
    clearButton.addEventListener("click", ClearSearch);

    function ClearSearch() {
        window.location.reload();
    }

    function SearchBy(event) {
        let pagination = document.getElementById("articles-pagination");
        pagination.style.display = "none";

        let parentElementSkinType = Array.from(event.target.parentElement.children[0].children);

        parentElementSkinType.shift();
        let categoryId = "";

        parentElementSkinType.forEach(option => {

            if (option.children[0].checked === true) {
                categoryId = option.children[0].value;
            }
        });

        console.log(categoryId)

        var token = $(".search-bar input[name=__RequestVerificationToken]").val();

        $.ajax({
            url: "/Articles/SearchBy/",
            type: "POST",
            data: JSON.stringify({ categoryId }),
            contentType: "application/json; charset=utf-8",
            headers: { 'X-CSRF-TOKEN': token },
            dataType: "json",
            success: function (data) {
                let result = '';
                debugger;
                data.articles.forEach(article => {
                    result += '<div class="col-md-4 d-flex"> <div class="blog-entry justify-content-end">' + `<a class="block-20" style="background-image: url('${article.picture}');"></a>` + '<div class="text p-4 float-right d-block">' + '<div class="d-flex align-items-center pt-2 mb-4">' + '<div class="one">' + `<span class="day">${article.day}</span>` + '</div>' + '<div class="two">' + `<span class="yr">${article.year}</span>` + `<span class="mos">${article.month}</span>` + '</div> </div >' + `<h3 class="heading mt-2"><a href="/Articles/GetDetails/${article.id}">${article.title}</a></h3>` + `<p class="small">${article.shortContent}</p>` + '<ul class="subheading article-category d-inline">' + `<li class="theme-color-gold small article-category"><strong>Category: </strong>${article.categoryName}</li>` +
                        `<li class="theme-color-gold small article-category"><strong>Author: </strong><a href="/Stylists/GetDetails/${article.stylistId}">${article.stylistFullName}</a></li>` +
                        `<li class="theme-color-gold small article-category"><strong>Likes: </strong>${article.likesCount}</li>` +
                        '</ul>' +
                        `<a class="btn btn-primary d-inline p-1 px-1 py-1 mr-md-1 log" href="/Articles/GetDetails/${article.id}">Read more</a>` + '</div></div></div>'
                });

                $('#showResult').html(result);
            }
        });
    }
}