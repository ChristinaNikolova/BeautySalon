function postComment() {
    let button = document.getElementById("comment-button")
    button.addEventListener("click", showCommentForm);

    function showCommentForm() {
        let commentForm = document.getElementById("comment-form");

        if (commentForm.style.display == "block") {
            commentForm.style.display = "none";
        } else {
            commentForm.style.display = "block";
        }
    }

    let postComment = document.getElementById("post-comment");
    postComment.addEventListener("click", comment);

    function comment(event) {
        console.log(event);
        event.preventDefault();
        let content = document.getElementById("content").value;
        let articleId = document.getElementById("articleId").innerText;

        var token = $("#form input[name=__RequestVerificationToken]").val();

        $.ajax({
            url: "/Comments/Create/",
            type: "POST",
            data: JSON.stringify({ articleId, content }),
            contentType: "application/json; charset=utf-8",
            headers: { 'X-CSRF-TOKEN': token },
            dataType: "json",
            success: function (data) {
                let errorMessage = document.getElementById("content-error");

                if (data.comments === null) {
                    errorMessage.style.display = "block";
                    return;
                }

                let result = '<ul class="comment-list">';

                data.comments.forEach(comment => {
                    result += '<li class="comment">' + '<div class="vcard bio">' + `<img src="${comment.applicationUserPicture}" alt="user-picture">` + '</div>' + '<div class="comment-body">' + `<h3>${comment.applicationUserUsername}</h3>` + `<div class="meta">${comment.formattedDate}</div>` + `<p class="small">${comment.content}</p>` + '</div>' + '</li>';
                })

                result += '</ul>'

                $('#result-new-commend').html(result);
                window.location.reload();
            }
        });
    }
}