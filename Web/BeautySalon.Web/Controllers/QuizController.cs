namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Quiz;
    using BeautySalon.Services.Data.SkinProblems;
    using BeautySalon.Services.Data.SkinTypes;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Web.ViewModels.MLModels;
    using BeautySalon.Web.ViewModels.Quiz.InputModels;
    using BeautySalon.Web.ViewModels.Quiz.ViewModels;
    using BeautySalon.Web.ViewModels.SkinProblems.ViewModels;
    using BeautySalon.Web.ViewModels.SkinTypes.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.ML;

    public class QuizController : BaseController
    {
        private readonly PredictionEnginePool<SkinTypeModelInput, SkinTypeModelOutput> predictionEngine;
        private readonly IQuizService quizService;
        private readonly ISkinTypesService skinTypesService;
        private readonly ISkinProblemsService skinProblemsService;
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;

        public QuizController(
            PredictionEnginePool<SkinTypeModelInput,
            SkinTypeModelOutput> predictionEngine,
            IQuizService quizService,
            ISkinTypesService skinTypesService,
            ISkinProblemsService skinProblemsService,
            IUsersService usersService,
            UserManager<ApplicationUser> userManager)
        {
            this.predictionEngine = predictionEngine;
            this.quizService = quizService;
            this.skinTypesService = skinTypesService;
            this.skinProblemsService = skinProblemsService;
            this.usersService = usersService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Make()
        {
            var model = new QuizInputModel()
            {
                Quiz = await this.quizService.GetQuizAsync<QuestionQuizViewModel>(),
                SkinProblems = await this.skinProblemsService.GetAllAsync<SkinProblemViewModel>(),
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

        [HttpPost]
        public async Task<ActionResult> Save([FromBody] Test input)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.usersService.AddSkinTypeData(userId, input.IsSkinSensitive, input.SkinTypeId, input.SkinProblemNames);
            ;
            return this.Json(new RedirectResult("/"));
        }
    }

    public class Test
    {
        public string SkinTypeId { get; set; }

        public string SkinTypeName { get; set; }

        public bool IsSkinSensitive { get; set; }

        public string[] SkinProblemNames { get; set; }
    }

    public class TestResult
    {
        public string[] SkinProblemNames { get; set; }
    }
}
