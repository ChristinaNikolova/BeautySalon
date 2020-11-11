function quiz() {
    let answers = [];
    var token = $("#form-quiz input[name=__RequestVerificationToken]").val();

    let parentElementSection = document.getElementById("parent-element-section");
    let counterChildren = 1;

    let startButton = document.getElementById("start-quiz");
    startButton.addEventListener("click", startQuiz);

    function startQuiz() {
        parentElementSection.children[counterChildren].style.display = "block";
        startButton.style.display = "none";
    }

    let buttons = [...document.getElementsByClassName("btn btn-outline-primary answer-quiz")];
    buttons.forEach((button) => {
        button.addEventListener("click", getNextSection);
    });

    let totalQuestions = parentElementSection.children.length;

    function getNextSection(event) {
        event.preventDefault();
        let currentAnswer = event.target.textContent;

        parentElementSection.children[counterChildren].style.display = "none";
        counterChildren++;

        if (counterChildren === totalQuestions) {
            var lastAnswer = currentAnswer;

            $.ajax({
                type: "POST",
                url: "/Quiz/Make",
                data: JSON.stringify({ answers, lastAnswer }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                headers: { 'X-CSRF-TOKEN': token },
                success: function (data) {
                    if (data.isSkinSensitive) {
                        data.skinType.name = data.skinType.name + " - Sensitive"
                    }

                    let result = `<h2 class="mb-5" align="center">Your result</h2><div class="offer-deal text-center px-2 px-lg-5"><div class="img" style="background-image: url(/images/offer-deal-2.jpg);"></div>
                      <div class="text mt-4"><h3 class="mb-4">You have <span class="theme-color-gold">${data.skinType.name}</span> skin!</h3><p class="mb-5">${data.skinType.description}</p><div class="text-center skin-problems-buttons">
                       <a id="skin-problems-button" class="btn btn-primary py-3 px-5 skin-problems-button">Share your skin problems!</a><a href="/" class="btn btn-outline-primary py-3 px-5 skin-problems-button">Quit</a></div></div></div>`;

                    document.getElementById("make-quiz-heading").style.display = "none";

                    $("#parent-element-section").html(result);
                    document.getElementById("skin-problems-button").addEventListener("click", showProblems);

                    function showProblems() {
                        document.getElementById("skin-problems-section").style.display = "block";
                    }

                    let saveButton = document.getElementById("skin-problems-save-button");
                    saveButton.addEventListener("click", save);

                    function save(event) {
                        event.preventDefault();

                        let skinProblemNames = [];
                        let isSkinSensitive = data.isSkinSensitive;
                        let skinTypeId = data.skinType.id;

                        let skinProblems = [...document.getElementById("skin-problems-elements").children];

                        skinProblems.forEach(skinProblem => {
                            if (skinProblem.children[0].checked) {
                                skinProblemNames.push(skinProblem.children[0].value);
                            }
                        });

                        var tokenSkinProblemForm = $("#skin-problems-form input[name=__RequestVerificationToken]").val();

                        $.ajax({
                            url: "/Quiz/Save",
                            type: "POST",
                            data: JSON.stringify({ skinTypeId, isSkinSensitive, skinProblemNames }),
                            contentType: "application/json; charset=utf-8",
                            headers: { 'X-CSRF-TOKEN': tokenSkinProblemForm },
                            success(data) {
                                window.location.replace(data.url);
                            }
                        });
                    }
                }
            });
        } else {
            answers.push(currentAnswer);
            parentElementSection.children[counterChildren].style.display = "block";
        }
    }
}