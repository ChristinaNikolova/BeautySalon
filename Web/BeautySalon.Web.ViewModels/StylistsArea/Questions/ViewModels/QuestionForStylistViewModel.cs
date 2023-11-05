namespace BeautySalon.Web.ViewModels.StylistsArea.Questions.ViewModels
{
    using System;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class QuestionForStylistViewModel : IMapFrom<Question>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FormattedDate
            => string.Format(
                GlobalConstants.DateTimeFormat,
                TimeZoneInfo.ConvertTimeFromUtc(this.CreatedOn, TimeZoneInfo.FindSystemTimeZoneById(GlobalConstants.LocalTimeZone)));

        public string ClientUsername { get; set; }

        public string ClientPicture { get; set; }
    }
}
