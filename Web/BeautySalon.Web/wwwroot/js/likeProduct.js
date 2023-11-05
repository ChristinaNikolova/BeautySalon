function likeProduct() {
    document.getElementById("likeProduct").addEventListener("click", likeProduct);

    function likeProduct() {
        let productId = document.getElementById("productId").innerHTML;
        var token = $("#likes input[name=__RequestVerificationToken]").val();

        $.ajax({
            url: "/Products/Like/",
            type: "POST",
            data: JSON.stringify(productId),
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

                $("#likeProduct").html(result);
                $("#likesCount").html(data.likesCount + " Likes");
            }
        });
    }
}