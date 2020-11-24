namespace BeautySalon.Web.ViewModels.Products.ViewModels
{
    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class UsersFavouriteProductsViewModel : IMapFrom<ClientProductLike>
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public string ShortDescription
        {
            get
            {
                return this.ProductDescription.Length > GlobalConstants.FavouriteProductShortDescriptionLength
                        ? this.ProductDescription.Substring(0, GlobalConstants.FavouriteProductShortDescriptionLength) + "..."
                        : this.ProductDescription;
            }
        }

        public string ProductPicture { get; set; }

        public string ProductBrandName { get; set; }

        public string ProductCategoryName { get; set; }
    }
}
