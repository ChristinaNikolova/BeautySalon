namespace BeautySalon.Web.Areas.Stylists.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Answers;
    using BeautySalon.Web.ViewModels.Answers.ViewModels;
    using BeautySalon.Web.ViewModels.StylistsArea.Answers.InputModels;
    using BeautySalon.Web.ViewModels.StylistsArea.Answers.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class AnswersController : StylistsController
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateAnswerInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var answerId = await this.answersService.CreateAsync(input.Title, input.Content, input.Question.StylistId, input.Question.ClientId, input.Question.Id);

            this.TempData["InfoMessage"] = GlobalMessages.SuccessSendMessage;

            return this.RedirectToAction(nameof(this.SeeDetails), new { Id = answerId });
        }

        public async Task<IActionResult> GetStylistsAllAnswers()
        {
            var stylistId = this.userManager.GetUserId(this.User);

            var model = new AllAnswersStylistAreaViewModel()
            {
                AnsweredQuestions = await this.answersService.GetAllForStylistAsync<AnswerStylistAreaViewModel>(stylistId),
                StylistId = stylistId,
            };

            return this.View(model);
        }

        public async Task<IActionResult> SeeDetails(string id)
        {
            var model = await this.answersService.GetAnswerDetailsAsync<DetailsAnswerViewModel>(id);

            return this.View(model);
        }
    }
}
