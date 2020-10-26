namespace BeautySalon.Web.ViewModels.Procedures.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ProcedureViewModel : IMapFrom<Procedure>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public double AverageRating { get; set; }

        public string FormattedRaiting
            => this.AverageRating.ToString("F2");

        public string CategoryName { get; set; }

        public string SkinTypeName { get; set; }

        public string SkinTypeToDisplay
            => this.SkinTypeName + " Skin";
    }
}
