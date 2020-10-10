namespace BeautySalon.Web.ViewModels.Administration.Brands.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class DetailsViewModel : IMapFrom<Brand>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Logo { get; set; }
    }
}
