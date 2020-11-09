namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using BeautySalon.Services.Data.Products;
    using BeautySalon.Web.ViewModels.Administration.Products.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class ProductsController : AdministrationController
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<IActionResult> GetAll()
        {
            var model = new AllProductsAdministrationViewModel()
            {
                Products = await this.productsService.GetAllAdministrationAsync<ProductAdministrationViewModel>(),
            };

            return this.View(model);
        }
    }
}
