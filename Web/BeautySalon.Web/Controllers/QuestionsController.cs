namespace BeautySalon.Web.Controllers
{
    using BeautySalon.Services.Data.Questions;
    using Microsoft.AspNetCore.Mvc;

    public class QuestionsController : Controller
    {
        private readonly IQuestionsService questionsService;

        public QuestionsController(IQuestionsService questionsService)
        {
            this.questionsService = questionsService;
        }
    }
}
