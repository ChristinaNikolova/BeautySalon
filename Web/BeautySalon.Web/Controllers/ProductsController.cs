namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Products;
    using BeautySalon.Web.ViewModels.Products.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ProductsController(IProductsService productsService, UserManager<ApplicationUser> userManager)
        {
            this.productsService = productsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetDetails(string id)
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = await this.productsService.GetDetailsAsync<DetailsProductViewModel>(id);
            model.IsFavourite = await this.productsService.CheckFavouriteProductsAsync(id, userId);

            return this.View(model);
        }

        [HttpPost]
        public async Task<ActionResult<LikeProductViewModel>> Like([FromBody] string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                return this.RedirectToAction(nameof(this.GetDetails), productId);
            }

            var userId = this.userManager.GetUserId(this.User);

            var isAdded = await this.productsService.LikeProductAsync(productId, userId);
            var likesCount = await this.productsService.GetLikesCountAsync(productId);

            return new LikeProductViewModel { IsAdded = isAdded, LikesCount = likesCount };
        }

        public async Task<IActionResult> GetUsersFavouriteProducts()
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = new AllUsersFavouriteProductsViewModel()
            {
                Products = await this.productsService.GetUsersFavouriteProductsAsync<UsersFavouriteProductsViewModel>(userId),
            };

            return this.View(model);
        }
    }
}
