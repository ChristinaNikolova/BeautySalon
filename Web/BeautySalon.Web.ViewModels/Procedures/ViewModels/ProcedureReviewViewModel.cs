namespace BeautySalon.Web.ViewModels.Procedures.ViewModels
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ProcedureReviewViewModel : IMapFrom<Review>
    {
        public string ClientUsername { get; set; }

        public string Content { get; set; }

        public string ClientPicture { get; set; }

        public DateTime Date { get; set; }

        public string FormattedDate
            => this.Date.ToString("dd.MM.yyyy hh:mm:ss");
    }
}
