namespace BeautySalon.Web.ViewModels.Articles.ViewModels
{
    using System.Collections.Generic;

    public class AllArticlesViewModel
    {
        public IEnumerable<ArticleViewModel> Articles { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
