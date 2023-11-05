namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Answers;
    using BeautySalon.Web.ViewModels.Answers.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class AnswersController : BaseController
    {
        private readonly IAnswersService answersService;
        private readonly UserManager<ApplicationUser> userManager;

        public AnswersController(
            IAnswersService answersService,
            UserManager<ApplicationUser> userManager)
        {
            this.answersService = answersService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetNewAnswersForUsersQuestion()
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = new AllNewAnswersForUserViewModel()
            {
                Answers = await this.answersService.GetAllNewAnswersForUserAsync<NewAnswerForUserViewModel>(userId),
            };

            return this.View(model);
        }

        public async Task<IActionResult> SeeAnswer(string id)
        {
            await this.answersService.ChangeIsRedAsync(id);

            var model = await this.answersService.GetAnswerDetailsAsync<DetailsAnswerViewModel>(id);

            return this.View(model);
        }

        public async Task<IActionResult> GetUsersAllAnswers()
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = new AllAnswersForUserViewModel()
            {
                Answers = await this.answersService.GetAllAnswersForUserAsync<AnswerForUserViewModel>(userId),
            };

            return this.View(model);
        }
    }
}
