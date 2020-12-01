namespace BeautySalon.Web.ViewModels.TypeCards.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class TypeCardViewModel : IMapFrom<TypeCard>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string FormattedPrice
            => this.Price.ToString("F0");
    }
}
