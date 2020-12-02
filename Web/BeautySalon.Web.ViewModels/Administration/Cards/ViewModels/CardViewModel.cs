namespace BeautySalon.Web.ViewModels.Administration.Cards.ViewModels
{
    using System;
    using System.Globalization;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class CardViewModel : IMapFrom<Card>
    {
        public string ClientId { get; set; }

        public string ClientUsername { get; set; }

        public DateTime StartDate { get; set; }

        public string FormattedStartDate
            => this.StartDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("en"));

        public DateTime EndDate { get; set; }

        public string FormattedEndEnd
           => this.EndDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("en"));

        public string TypeCardName { get; set; }
    }
}
