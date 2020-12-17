namespace BeautySalon.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Articles;
    using BeautySalon.Web.ViewModels.Articles.InputModels;
    using BeautySalon.Web.ViewModels.Articles.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ArticlesController(
            IArticlesService articlesService,
            UserManager<ApplicationUser> userManager)
        {
            this.articlesService = articlesService;
            this.userManager = userManager;
        }

        [AllowAnonymous]
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
            if (string.IsNullOrWhiteSpace(articleId))
            {
                return this.RedirectToAction(nameof(this.GetDetails), articleId);
            }

            var userId = this.userManager.GetUserId(this.User);

            var isAdded = await this.articlesService.LikeArticleAsync(articleId, userId);
            var likesCount = await this.articlesService.GetLikesCountAsync(articleId);

            return new LikeArticleViewModel { IsAdded = isAdded, LikesCount = likesCount };
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<AllArticlesViewModel>> SearchBy([FromBody] SearchArticleCriteriaInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.CategoryId))
            {
                return this.RedirectToAction(nameof(this.GetAll));
            }

            var articles = await this.articlesService.SearchByAsync<ArticleViewModel>(input.CategoryId);

            return new AllArticlesViewModel { Articles = articles };
        }

        public async Task<IActionResult> GetUsersFavouriteArticles()
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = new AllUsersFavouriteArticlesViewModel()
            {
                Articles = await this.articlesService.GetUsersFavouriteArticlesAsync<UsersFavouriteArticlesViewModel>(userId),
            };

            return this.View(model);
        }
    }
}
