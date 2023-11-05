namespace BeautySalon.Web.ViewModels.Articles.ViewModels
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Ganss.XSS;

    public class UsersFavouriteArticlesViewModel : IMapFrom<ClientArticleLike>
    {
        public string ArticleId { get; set; }

        public string ArticleTitle { get; set; }

        public string ArticleContent { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.ArticleContent);

        public string ShortContent
        {
            get
            {
                var content = WebUtility.HtmlDecode(Regex.Replace(this.SanitizedContent, @"<[^>]+>", string.Empty));

                return content.Length > GlobalConstants.ArticleShortDescriptionLength
                        ? content.Substring(0, GlobalConstants.ArticleShortDescriptionLength) + "..."
                        : content;
            }
        }

        public string ArticlePicture { get; set; }

        public DateTime ArticleCreatedOn { get; set; }

        public string FormattedDate
            => this.ArticleCreatedOn.ToString("dd.MM.yyyy");

        public string ArticleStylistId { get; set; }

        public string ArticleStylistFirstName { get; set; }

        public string ArticleStylistLastName { get; set; }

        public string StylistFullName
            => this.ArticleStylistFirstName + " " + this.ArticleStylistLastName;

        public int ArticleCommentsCount { get; set; }
    }
}
