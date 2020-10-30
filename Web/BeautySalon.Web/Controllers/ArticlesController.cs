namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Articles;
    using BeautySalon.Web.ViewModels.Articles.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : Controller
    {
        private readonly IArticlesService articlesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ArticlesController(IArticlesService articlesService, UserManager<ApplicationUser> userManager)
        {
            this.articlesService = articlesService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetAll()
        {
            var model = new AllArticlesViewModel()
            {
                Articles = await this.articlesService.GetAllAsync<ArticleViewModel>(),
            };

            return this.View(model);
        }

        public async Task<IActionResult> GetDetails(string id)
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = await this.articlesService.GetArticleDetailsAsync<DetailsArticleViewModel>(id);
            model.IsFavourite = await this.articlesService.CheckFavouriteArticlesAsync(id, userId);

            return this.View(model);
        }

        [HttpPost]
        public void Like([FromBody] string articleId)
        {
            ;
            var userId = this.userManager.GetUserId(this.User);

            //await this.articlesService.LikeArticleAsync(articleId, userId);

            //var likes = this.blogService.GetVotesCount(id);

            //return new LikesResponseModel { LikesCount = likes };
        }
    }
}
