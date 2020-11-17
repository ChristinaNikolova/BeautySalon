namespace BeautySalon.Web.ViewModels.Products.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class DetailsProductViewModel : IMapFrom<Product>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Picture { get; set; }

        public string BrandName { get; set; }

        public string BrandDescription { get; set; }

        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        //public virtual ICollection<ProductReview> ProductReviews { get; set; }

        //public virtual ICollection<ClientProductLike> Likes { get; set; }
    }
}
