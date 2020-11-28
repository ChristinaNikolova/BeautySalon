namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Questions;
    using BeautySalon.Services.Data.Stylists;
    using BeautySalon.Web.ViewModels.Questions.InputModels;
    using BeautySalon.Web.ViewModels.Stylists.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class QuestionsController : BaseController
    {
        private readonly IQuestionsService questionsService;
        private readonly IStylistsService stylistsService;
        private readonly UserManager<ApplicationUser> userManager;

        public QuestionsController(
            IQuestionsService questionsService,
            IStylistsService stylistsService,
            UserManager<ApplicationUser> userManager)
        {
            this.questionsService = questionsService;
            this.stylistsService = stylistsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Create(string id)
        {
            var model = new CreateQuestionInputModel()
            {
                StylistId = id,
                Stylist = await this.stylistsService.GetStylistDetailsAsync<StylistNamesViewModel>(id),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQuestionInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var userId = this.userManager.GetUserId(this.User);

            await this.questionsService.CreateAsync(input.Title, input.Content, input.StylistId, userId);

            this.TempData["InfoMessage"] = "Your question was successfully send!";

            return this.RedirectToAction("GetDetails", "Stylists", new { id = input.StylistId });
        }
    }
}
