namespace BeautySalon.Web.ViewModels.Users.ViewModels
{
    using System;
    using System.Globalization;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class UserCardViewModel : IMapFrom<Card>
    {
        public DateTime StartDate { get; set; }

        public string FormattedStartDate
            => this.StartDate.ToString("dd.MMMM.yyyy", CultureInfo.CreateSpecificCulture("en"));

        public DateTime EndEnd { get; set; }

        public string FormattedEndDate
            => this.EndEnd.ToString("dd.MMMM.yyyy", CultureInfo.CreateSpecificCulture("en"));

        public string TypeCardName { get; set; }

        public int TypeCardPrice { get; set; }

        public string AuthenticatorUri { get; set; }
    }
}
