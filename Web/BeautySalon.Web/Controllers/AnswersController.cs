namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Answers;
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

        public async Task<IActionResult> GetClientsAnswers()
        {
            var userId = this.userManager.GetUserId(this.User);

            return this.View();
        }
    }
}
