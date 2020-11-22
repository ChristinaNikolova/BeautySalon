function showAnswerForm() {
    document.getElementById("answer-button").addEventListener("click", showAnswerForm);
    let answerForm = document.getElementById("form-answer");

    function showAnswerForm() {
        if (answerForm.style.display === "block") {
            answerForm.style.display = "none";
        } else {
            answerForm.style.display = "block";
        }
    }
}