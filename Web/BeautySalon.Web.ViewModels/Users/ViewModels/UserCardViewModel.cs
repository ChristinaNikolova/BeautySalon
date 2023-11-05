namespace BeautySalon.Web.ViewModels.Users.ViewModels
{
    using System;
    using System.Globalization;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class UserCardViewModel : IMapFrom<Card>
    {
        public string Id { get; set; }

        public DateTime StartDate { get; set; }

        public string FormattedStartDate
            => this.StartDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("en"));

        public DateTime EndDate { get; set; }

        public string FormattedEndDate
            => this.EndDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("en"));

        public string TypeCardName { get; set; }

        public int TypeCardPrice { get; set; }

        public string AuthenticatorUri { get; set; }

        public string ClientId { get; set; }

        public int CounterUsed { get; set; }

        public int TotalSumUsedProcedures { get; set; }

        public int SavedMoney
        => this.TotalSumUsedProcedures - this.TypeCardPrice > 0 ? (this.TotalSumUsedProcedures - this.TypeCardPrice) : 0;
    }
}
