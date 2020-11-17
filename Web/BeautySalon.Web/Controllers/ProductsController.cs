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
            var userId = this.userManager.GetUserId(this.User);

            var isAdded = await this.productsService.LikeProductAsync(productId, userId);
            var likesCount = await this.productsService.GetLikesCountAsync(productId);

            return new LikeProductViewModel { IsAdded = isAdded, LikesCount = likesCount };
        }
    }

    public class LikeProductViewModel
    {
        public bool IsAdded { get; set; }

        public int LikesCount { get; set; }
    }
}
