namespace BeautySalon.Web.ViewModels.Quiz.ViewModels
{
    using System.Collections.Generic;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class QuestionQuizViewModel : IMapFrom<Question>
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public IEnumerable<AnswerQuizViewModel> QuizAnswers { get; set; }
    }
}
