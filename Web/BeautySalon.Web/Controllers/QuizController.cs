namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Quiz;
    using BeautySalon.Web.MLModels;
    using BeautySalon.Web.ViewModels.Quiz.InputModels;
    using BeautySalon.Web.ViewModels.Quiz.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.Extensions.ML;

    public class QuizController : BaseController
    {
        private readonly PredictionEnginePool<SkinTypeModelInput, SkinTypeModelOutput> predictionEngine;
        private readonly IQuizService quizService;

        public QuizController(PredictionEnginePool<SkinTypeModelInput, SkinTypeModelOutput> predictionEngine, IQuizService quizService)
        {
            this.predictionEngine = predictionEngine;
            this.quizService = quizService;
        }

        public async Task<IActionResult> LoadQuiz()
        {
            var model = new LoadQuizInputModel()
            {
                Quiz = await this.quizService.GetQuizAsync<QuestionQuizViewModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LoadQuiz([FromBody] string[] answers)
        {
            string result = string.Join(" ", answers);

            var input = new SkinTypeModelInput()
            {
                Description = result,
            };

            var output = this.predictionEngine.Predict(input);

            return this.View(output);
        }
    }
}
