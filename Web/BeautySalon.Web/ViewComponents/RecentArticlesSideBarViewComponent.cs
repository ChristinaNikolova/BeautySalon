namespace BeautySalon.Web.ViewComponents
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Articles;
    using BeautySalon.Web.ViewModels.Articles.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class RecentArticlesSideBarViewComponent : ViewComponent
    {
        private readonly IArticlesService articlesService;

        public RecentArticlesSideBarViewComponent(IArticlesService articlesService)
        {
            this.articlesService = articlesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new AllRecentArticlesViewModel()
            {
                RecentArticles = await this.articlesService
                .GetRecentArticlesAsync<RecentArticlesViewModel>(),
            };

            return this.View(model);
        }
    }
}
