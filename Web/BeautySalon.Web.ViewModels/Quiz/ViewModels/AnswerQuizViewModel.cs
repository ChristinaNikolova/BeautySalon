namespace BeautySalon.Web.ViewModels.Quiz.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class AnswerQuizViewModel : IMapFrom<QuizAnswer>
    {
        public string Id { get; set; }

        public string Text { get; set; }
    }
}
