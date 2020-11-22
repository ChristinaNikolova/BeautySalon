namespace BeautySalon.Web.ViewModels.Answers.ViewModels
{
    using System;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class DetailsAnswerViewModel : NewAnswerForUserViewModel, IMapFrom<Answer>
    {
        public string Content { get; set; }

        public string StylistUsername { get; set; }

        public string QuestionTitle { get; set; }

        public string QuestionContent { get; set; }

        public DateTime QuestionCreatedOn { get; set; }

        public string FormattedQuestionCreatedOn
            => string.Format(GlobalConstants.DateTimeFormat, this.QuestionCreatedOn.ToLocalTime());

        public string ClientPicture { get; set; }

        public string ClientUsername { get; set; }
    }
}
