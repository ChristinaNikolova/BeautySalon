function showSkinProblemsInfo() {
    let problems = [...document.getElementsByClassName("show-more-info")];

    problems.forEach((problem) => {
        problem.addEventListener("click", showInfo);
    });

    function showInfo(event) {
        let parent = event.target.parentElement;
        let infoMessage = parent.children[1];

        if (infoMessage.style.display === "block") {
            infoMessage.style.display = "none";
        }
        else {
            infoMessage.style.display = "block";
        }
    }
}