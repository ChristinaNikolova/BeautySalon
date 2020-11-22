namespace BeautySalon.Web.ViewModels.Comments.ViewModels
{
    using System;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class CommentViewModel : IMapFrom<Comment>
    {
        public string Id { get; set; }

        public string ApplicationUserUsername { get; set; }

        public string ApplicationUserPicture { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FormattedDate
            => string.Format(GlobalConstants.DateTimeFormat, this.CreatedOn.ToLocalTime());
    }
}
