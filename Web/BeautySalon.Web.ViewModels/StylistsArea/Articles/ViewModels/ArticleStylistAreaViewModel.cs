namespace BeautySalon.Web.ViewModels.StylistsArea.Articles.ViewModels
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ArticleStylistAreaViewModel : IMapFrom<Article>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FormattedDate
            => this.CreatedOn.ToString("dd.MM.yyyy");

        public string CategoryName { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }
    }
}
