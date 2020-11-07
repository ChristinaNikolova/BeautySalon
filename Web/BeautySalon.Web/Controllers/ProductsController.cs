namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Products;
    using BeautySalon.Web.ViewModels.Products.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<IActionResult> GetDetails(string id)
        {
            var model = await this.productsService.GetDetailsAsync<DetailsProductViewModel>(id);

            return this.View(model);
        }
    }
}
