namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Articles;
    using BeautySalon.Web.ViewModels.Articles.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : Controller
    {
        private readonly IArticlesService articlesService;

        public ArticlesController(IArticlesService articlesService)
        {
            this.articlesService = articlesService;
        }

        public async Task<IActionResult> GetAll()
        {
            var model = new AllArticlesViewModel()
            {
                Articles = await this.articlesService.GetAllAsync<ArticleViewModel>(),
            };

            return this.View(model);
        }
    }
}
