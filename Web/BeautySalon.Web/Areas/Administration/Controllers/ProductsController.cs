namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Brands;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.Products;
    using BeautySalon.Web.ViewModels.Administration.Products.InputModels;
    using BeautySalon.Web.ViewModels.Administration.Products.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : AdministrationController
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categoriesService;
        private readonly IBrandsService brandsService;

        public ProductsController(IProductsService productsService, ICategoriesService categoriesService, IBrandsService brandsService)
        {
            this.productsService = productsService;
            this.categoriesService = categoriesService;
            this.brandsService = brandsService;
        }

        public async Task<IActionResult> GetAll()
        {
            var model = new AllProductsAdministrationViewModel()
            {
                Products = await this.productsService.GetAllAdministrationAsync<ProductAdministrationViewModel>(),
            };

            return this.View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new AddProductInputModel()
            {
                Categories = await this.categoriesService.GetAllAsSelectListItemAsync(),
                Brands = await this.brandsService.GetAllAsSelectListItemAsync(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddProductInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.categoriesService.GetAllAsSelectListItemAsync();
                input.Brands = await this.brandsService.GetAllAsSelectListItemAsync();

                return this.View(input);
            }

            var id = await this.productsService.CreateAsync(input.Name, input.Description, input.Price, input.Picture, input.BrandId, input.CategoryId);

            //return this.RedirectToAction(nameof(this.Update), new { Id = id });
            return this.Redirect(nameof(this.GetAll));
        }
    }
}
