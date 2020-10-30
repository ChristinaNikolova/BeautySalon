namespace BeautySalon.Web.ViewModels.Articles.ViewModel
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ArticleStylistViewModel : IMapFrom<Article>
    {
        public string ArticleId { get; set; }

        public string ArticleTitle { get; set; }

        public DateTime ArticleCreatedOn { get; set; }
    }
}
