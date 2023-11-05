namespace BeautySalon.Web.ViewModels.Products.ViewModels
{
    using System.Collections.Generic;

    public class AllUsersFavouriteProductsViewModel
    {
        public IEnumerable<UsersFavouriteProductsViewModel> Products { get; set; }
    }
}
