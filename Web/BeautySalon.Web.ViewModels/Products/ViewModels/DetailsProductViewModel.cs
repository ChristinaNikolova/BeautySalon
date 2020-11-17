namespace BeautySalon.Web.ViewModels.Products.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class DetailsProductViewModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Picture { get; set; }

        public string BrandName { get; set; }

        public string BrandDescription { get; set; }

        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public bool IsFavourite { get; set; }

        public int LikesCount { get; set; }
    }
}
