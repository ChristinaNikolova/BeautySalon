namespace BeautySalon.Web.ViewModels.Quiz.InputModels
{
    using System.Collections.Generic;

    using BeautySalon.Web.ViewModels.Quiz.ViewModels;
    using BeautySalon.Web.ViewModels.SkinProblems.ViewModels;

    public class QuizInputModel
    {
        public IEnumerable<QuestionQuizViewModel> Quiz { get; set; }

        public IEnumerable<SkinProblemViewModel> SkinProblems { get; set; }
    }
}
