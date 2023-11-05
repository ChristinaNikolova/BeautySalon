function hideShowQuestion() {
    let button = document.getElementById("hide-question");
    button.addEventListener("click", hideQuestion);

    function hideQuestion() {
        let question = document.getElementById("question-section");

        if (question.style.display === "block") {
            question.style.display = "none";
            button.innerText = "Show Question";
        } else {
            question.style.display = "block";
            button.innerText = "Hide Question";
        }
    }
}