﻿namespace BeautySalon.Web.Areas.Stylists.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Questions;
    using BeautySalon.Web.ViewModels.StylistsArea.Answers.InputModels;
    using BeautySalon.Web.ViewModels.StylistsArea.Questions.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class QuestionsController : StylistsController
    {
        private readonly IQuestionsService questionsService;
        private readonly UserManager<ApplicationUser> userManager;

        public QuestionsController(
            IQuestionsService questionsService,
            UserManager<ApplicationUser> userManager)
        {
            this.questionsService = questionsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetAllForStylist()
        {
            var stylistId = this.userManager.GetUserId(this.User);

            var model = new AllQuestionForStylistViewModel()
            {
                Questions = await this.questionsService
                .GetAllNewQuestionsForStylistAsync<QuestionForStylistViewModel>(stylistId),
            };

            return this.View(model);
        }

        public async Task<IActionResult> SeeQuestion(string id)
        {
            var question = await this.questionsService.GetQuestionDetailsAsync<SeeQuestionViewModel>(id);

            var model = new CreateAnswerInputModel()
            {
                Question = question,
            };

            return this.View(model);
        }
    }
}
