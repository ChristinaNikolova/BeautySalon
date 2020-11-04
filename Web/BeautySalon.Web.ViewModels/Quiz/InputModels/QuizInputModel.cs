namespace BeautySalon.Web.ViewModels.Quiz.InputModels
{
    using System.Collections.Generic;

    using BeautySalon.Web.ViewModels.Quiz.ViewModels;

    public class QuizInputModel
    {
        public IEnumerable<QuestionQuizViewModel> Quiz { get; set; }
    }
}
