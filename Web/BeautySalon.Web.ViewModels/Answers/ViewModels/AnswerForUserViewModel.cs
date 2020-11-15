namespace BeautySalon.Web.ViewModels.Answers.ViewModels
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class AnswerForUserViewModel : IMapFrom<Answer>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string StylistId { get; set; }

        public string StylistFirstName { get; set; }

        public string StylistLastName { get; set; }

        public string StylistFullName
            => this.StylistFirstName + " " + this.StylistLastName;

        public DateTime CreatedOn { get; set; }

        public string FormattedCreatedOn
            => this.CreatedOn.ToString("dd.MM.yyyy hh:mm:ss");
    }
}
