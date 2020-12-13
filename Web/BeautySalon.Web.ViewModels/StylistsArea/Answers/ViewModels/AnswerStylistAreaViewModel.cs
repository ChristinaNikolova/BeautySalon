namespace BeautySalon.Web.ViewModels.StylistsArea.Answers.ViewModels
{
    using System;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class AnswerStylistAreaViewModel : IMapFrom<Answer>
    {
        public string Id { get; set; }

        public string QuestionTitle { get; set; }

        public string QuestionClientUsername { get; set; }

        public DateTime QuestionCreatedOn { get; set; }

        public string FormattedQuestionCreatedOn
             => string.Format(
                 GlobalConstants.DateTimeFormat,
                 TimeZoneInfo.ConvertTimeFromUtc(this.QuestionCreatedOn, TimeZoneInfo.FindSystemTimeZoneById(GlobalConstants.LocalTimeZone)));
    }
}
