namespace BeautySalon.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BeautySalon.Common;
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

        public async Task<IActionResult> GetAll(int currentPage = 1)
        {
            var articlesCount = await this.articlesService.GetTotalCountArticlesAsync();

            var pageCount = (int)Math.Ceiling((double)articlesCount / GlobalConstants.ArticlesPerPage);

            var articles = await this.articlesService
               .GetAllAsync<ArticleViewModel>(GlobalConstants.ArticlesPerPage, (currentPage - 1) * GlobalConstants.ArticlesPerPage);

            var model = new AllArticlesViewModel()
            {
                Articles = articles,
                CurrentPage = currentPage,
                PagesCount = pageCount,
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
        public async Task<ActionResult<LikeArticleViewModel>> Like([FromBody] string articleId)
        {
            var userId = this.userManager.GetUserId(this.User);

            var isAdded = await this.articlesService.LikeArticleAsync(articleId, userId);
            var likesCount = await this.articlesService.GetLikesCountAsync(articleId);

            return new LikeArticleViewModel { IsAdded = isAdded, LikesCount = likesCount };
        }
    }
}
