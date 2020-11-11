﻿namespace BeautySalon.Web.ViewModels.Articles.ViewModels
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Ganss.XSS;

    public class ArticleViewModel : IMapFrom<Article>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public string ShortContent
        {
            get
            {
                var content = WebUtility.HtmlDecode(Regex.Replace(this.SanitizedContent, @"<[^>]+>", string.Empty));

                return content.Length > 200
                        ? content.Substring(0, 200) + "..."
                        : content;
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
