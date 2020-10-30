namespace BeautySalon.Web.ViewModels.Articles.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ArticleViewModel : IMapFrom<Article>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ShortContent
        {
            get
            {
                return this.Content.Length > 300
                        ? this.Content.Substring(0, 300) + "..."
                        : this.Content;
            }
        }

        public string Picture { get; set; }

        public string CategoryName { get; set; }

        public string StylistFirstName { get; set; }

        public string StylistLastName { get; set; }

        public string StylistFullName
            => this.StylistFirstName + " " + this.StylistLastName;

        public int LikesCount { get; set; }
    }
}
