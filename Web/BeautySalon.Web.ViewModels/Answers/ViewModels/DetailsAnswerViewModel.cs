namespace BeautySalon.Web.ViewModels.Answers.ViewModels
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class DetailsAnswerViewModel : NewAnswerForUserViewModel, IMapFrom<Answer>
    {
        //public string Title { get; set; }

        public string Content { get; set; }

        //public DateTime CreatedOn { get; set; }

        //public string FormattedCreatedOn
        //    => this.CreatedOn.ToString("dd.MM.yyyy hh:mm:ss");

        //public string StylistPicture { get; set; }

        public string StylistUsername { get; set; }

        public string QuestionTitle { get; set; }

        public string QuestionContent { get; set; }

        public DateTime QuestionCreatedOn { get; set; }

        public string FormattedQuestionCreatedOn
            => this.QuestionCreatedOn.ToString("dd.MM.yyyy hh:mm:ss");

        public string ClientPicture { get; set; }

        public string ClientUsername { get; set; }
    }
}
