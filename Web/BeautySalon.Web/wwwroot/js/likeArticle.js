function likeArticle() {
    let likeButton = document.getElementById("likeArticle");
    likeButton.addEventListener("click", likeArticle);

    function likeArticle() {
        let articleId = document.getElementById("articleId").innerHTML;
        var token = $("#likes input[name=__RequestVerificationToken]").val();

        $.ajax({
            url: "/Articles/Like/",
            type: "POST",
            data: JSON.stringify(articleId),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: { 'X-CSRF-TOKEN': token },
            success: function (data) {
                let result = "";

                if (data.isAdded) {
                    result = '<a><i class="fa fa-heart"></i>';
                } else {
                    result = '<a><i class="far fa-heart"></i>';
                }

                $("#likeArticle").html(result);
                $("#likesCount").html(data.likesCount);
            }
        });
    }
}