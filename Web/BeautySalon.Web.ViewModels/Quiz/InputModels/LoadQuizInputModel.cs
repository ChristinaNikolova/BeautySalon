namespace BeautySalon.Web.ViewModels.Quiz.InputModels
{
    using System.Collections.Generic;

    using BeautySalon.Web.ViewModels.Quiz.ViewModels;

    public class LoadQuizInputModel
    {
        public IEnumerable<QuestionQuizViewModel> Quiz { get; set; }
    }
}
