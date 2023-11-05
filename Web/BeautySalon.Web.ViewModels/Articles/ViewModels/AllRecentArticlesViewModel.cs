namespace BeautySalon.Web.ViewModels.Articles.ViewModels
{
    using System.Collections.Generic;

    public class AllRecentArticlesViewModel
    {
        public IEnumerable<RecentArticlesViewModel> RecentArticles { get; set; }
    }
}
