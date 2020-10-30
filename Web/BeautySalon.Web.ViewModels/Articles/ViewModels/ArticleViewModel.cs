namespace BeautySalon.Web.ViewModels.Articles.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using System;

    public class ArticleViewModel : IMapFrom<Article>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ShortContent
        {
            get
            {
                return this.Content.Length > 200
                        ? this.Content.Substring(0, 200) + "..."
                        : this.Content;
            }
        }

        public string Picture { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Day
            => this.CreatedOn.Day;

        public string Month
           => this.CreatedOn.ToString("MMMM");

        public int Year
           => this.CreatedOn.Year;

        public string CategoryName { get; set; }

        public string StylistId { get; set; }

        public string StylistFirstName { get; set; }

        public string StylistLastName { get; set; }

        public string StylistFullName
            => this.StylistFirstName + " " + this.StylistLastName;

        public int LikesCount { get; set; }
    }
}
