namespace BeautySalon.Web.ViewModels.Administration.Products.ViewModels
{
    using System.Collections.Generic;

    public class AllProductsAdministrationViewModel
    {
        public IEnumerable<ProductAdministrationViewModel> Products { get; set; }
    }
}
