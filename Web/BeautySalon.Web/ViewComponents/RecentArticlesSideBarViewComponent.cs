namespace BeautySalon.Web.ViewComponents
{
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

        public IViewComponentResult Invoke()
        {
            var model = new AllRecentArticlesViewModel()
            {
                RecentArticles = this.articlesService
                .GetRecentArticles<RecentArticlesViewModel>(),
            };

            return this.View(model);
        }
    }
}
