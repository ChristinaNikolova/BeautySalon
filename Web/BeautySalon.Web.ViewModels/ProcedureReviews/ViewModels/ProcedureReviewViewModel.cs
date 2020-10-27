namespace BeautySalon.Web.ViewModels.ProcedureReviews.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using System;

    public class ProcedureReviewViewModel : IMapFrom<ProcedureReview>
    {
        public string ClientUsername { get; set; }

        public string Content { get; set; }

        public string ClientPicture { get; set; }

        public DateTime DateTime { get; set; }

        public string FormattedDate
            => this.DateTime.ToShortDateString();
    }
}
