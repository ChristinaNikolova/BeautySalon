namespace BeautySalon.Web.ViewModels.StylistsArea.Questions.ViewModels
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class QuestionForStylistViewModel : IMapFrom<Question>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FormattedDate
            => this.CreatedOn.ToString("dd.MM.yyyy hh:mm:ss");

        public string ClientId { get; set; }

        public string ClientUsername { get; set; }

        public string ClientPicture { get; set; }
    }
}
