namespace BeautySalon.Web.Areas.Stylists.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Answers;
    using BeautySalon.Web.ViewModels.StylistsArea.Answers.InputModels;
    using Microsoft.AspNetCore.Mvc;

    public class AnswersController : StylistsController
    {
        private readonly IAnswersService answersService;

        public AnswersController(IAnswersService answersService)
        {
            this.answersService = answersService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAnswerInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            string answerId = await this.answersService.CreateAsync(input.Title, input.Content, input.Question.StylistId, input.Question.ClientId, input.Question.Id);

            //TODO better redirect(send messages)!!!!!!!!!
            return this.Redirect("/");
        }
    }
}
