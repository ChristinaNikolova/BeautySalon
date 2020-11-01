namespace BeautySalon.Web.ViewModels.Articles.ViewModels
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class RecentArticlesViewModel : IMapFrom<Article>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Picture { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FormattedDate
            => this.CreatedOn.ToString("dd.MM.yyyy");

        public string StylistId { get; set; }

        public string StylistFirstName { get; set; }

        public string StylistLastName { get; set; }

        public string StylistFullName
            => this.StylistFirstName + " " + this.StylistLastName;

        public int CommentsCount { get; set; }
    }
}
