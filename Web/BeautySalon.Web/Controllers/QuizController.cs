namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Quiz;
    using BeautySalon.Services.Data.SkinTypes;
    using BeautySalon.Web.ViewModels.MLModels;
    using BeautySalon.Web.ViewModels.Quiz.InputModels;
    using BeautySalon.Web.ViewModels.Quiz.ViewModels;
    using BeautySalon.Web.ViewModels.SkinTypes.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.ML;

    public class QuizController : BaseController
    {
        private readonly PredictionEnginePool<SkinTypeModelInput, SkinTypeModelOutput> predictionEngine;
        private readonly IQuizService quizService;
        private readonly ISkinTypesService skinTypesService;

        public QuizController(PredictionEnginePool<SkinTypeModelInput, SkinTypeModelOutput> predictionEngine, IQuizService quizService, ISkinTypesService skinTypesService)
        {
            this.predictionEngine = predictionEngine;
            this.quizService = quizService;
            this.skinTypesService = skinTypesService;
        }

        public async Task<IActionResult> Make()
        {
            var model = new QuizInputModel()
            {
                Quiz = await this.quizService.GetQuizAsync<QuestionQuizViewModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<ActionResult<ResultViewModel>> Make([FromBody] AnswerQuizInputModel input)
        {
            string result = string.Join(" ", input.Answers);

            var inputML = new SkinTypeModelInput()
            {
                Description = result,
            };

            var outputML = this.predictionEngine.Predict(inputML);

            var model = new ResultViewModel()
            {
                SkinType = await this.skinTypesService.GetSkinTypeResultAsync<SkinTypeDescriptionViewModel>(outputML.Prediction),
                IsSkinSensitive = input.LastAnswer.Contains("Yes") ? true : false,
            };

            return model;
        }
    }
}
