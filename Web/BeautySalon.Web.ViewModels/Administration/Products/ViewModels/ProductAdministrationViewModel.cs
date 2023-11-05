namespace BeautySalon.Web.ViewModels.Administration.Products.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ProductAdministrationViewModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string FormattedPrice
            => this.Price.ToString("F2");

        public string BrandName { get; set; }

        public string CategoryName { get; set; }
    }
}
