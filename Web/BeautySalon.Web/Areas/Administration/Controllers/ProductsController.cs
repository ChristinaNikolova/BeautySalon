namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Common;
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

        public ProductsController(
            IProductsService productsService,
            ICategoriesService categoriesService,
            IBrandsService brandsService)
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

            this.TempData["InfoMessage"] = GlobalMessages.SuccessCreateMessage;

            return this.RedirectToAction(nameof(this.GetAll));
        }

        public async Task<IActionResult> Update(string id)
        {
            var model = await this.productsService.GetDetailsAsync<UpdateProductInputModel>(id);

            model.Categories = await this.categoriesService.GetAllAsSelectListItemAsync();
            model.Brands = await this.brandsService.GetAllAsSelectListItemAsync();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.categoriesService.GetAllAsSelectListItemAsync();
                input.Brands = await this.brandsService.GetAllAsSelectListItemAsync();
                input.Picture = await this.productsService.GetPictureUrlAsync(input.Id);

                return this.View(input);
            }

            await this.productsService.UpdateAsync(input.Id, input.Name, input.Description, input.Price, input.NewPicture, input.BrandId, input.CategoryId);

            this.TempData["InfoMessage"] = GlobalMessages.SuccessUpdateMessage;

            return this.RedirectToAction(nameof(this.GetAll));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.productsService.DeleteAsync(id);

            this.TempData["InfoMessage"] = GlobalMessages.SuccessDeleteMessage;

            return this.RedirectToAction(nameof(this.GetAll));
        }
    }
}
