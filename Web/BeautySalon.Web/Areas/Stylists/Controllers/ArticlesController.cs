namespace BeautySalon.Web.Areas.Stylists.Controllers
{
    using System.Threading.Tasks;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Articles;
    using BeautySalon.Web.ViewModels.StylistsArea.Articles.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : StylistsController
    {
        private readonly IArticlesService articlesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ArticlesController(IArticlesService articlesService, UserManager<ApplicationUser> userManager)
        {
            this.articlesService = articlesService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetAllForStylist()
        {
            var stylistId = this.userManager.GetUserId(this.User);

            var model = new AllArticleStylistAreaViewModel()
            {
                Articles = await this.articlesService
                .GetAllForStylistAsync<ArticleStylistAreaViewModel>(stylistId),
            };

            return this.View(model);
        }
    }
}
