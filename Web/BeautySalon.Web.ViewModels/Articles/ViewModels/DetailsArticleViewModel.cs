namespace BeautySalon.Web.ViewModels.Articles.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using BeautySalon.Web.ViewModels.Comments.ViewModels;

    public class DetailsArticleViewModel : ArticleViewModel, IHaveCustomMappings
    {
        public string StylistPicture { get; set; }

        public string StylistDescription { get; set; }

        public string StylistShortDescription
        {
            get
            {
                return this.StylistDescription.Length > GlobalConstants.StylistArticleShortDescriptionLength
                        ? this.StylistDescription.Substring(0, GlobalConstants.StylistArticleShortDescriptionLength) + "..."
                        : this.StylistDescription;
            }
        }

        public string FormattedDate
            => string.Format(GlobalConstants.DateTimeFormat, this.CreatedOn.ToLocalTime());

        public bool IsFavourite { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public IEnumerable<CommentViewModel> OrderedComments
            => this.Comments
            .OrderByDescending(c => c.CreatedOn)
            .ToList();

        public int CommentsCount
            => this.Comments.Count();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, DetailsArticleViewModel>().ForMember(
               m => m.Comments,
               opt => opt.MapFrom(x => x.Comments.Select(y => new CommentViewModel()
               {
                   Id = y.Id,
                   Content = y.Content,
                   ApplicationUserUsername = y.Client.UserName,
                   ApplicationUserPicture = y.Client.Picture,
                   CreatedOn = y.CreatedOn,
               })));
        }
    }
}
