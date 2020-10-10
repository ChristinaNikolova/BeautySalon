namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Services.Data.Brands;
    using BeautySalon.Web.Controllers;
    using BeautySalon.Web.ViewModels.Administration.Brands.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class BrandsController : BaseController
    {
        private readonly IBrandsService brandsService;

        public BrandsController(IBrandsService brandsService)
        {
            this.brandsService = brandsService;
        }

        public async Task<IActionResult> GetAll()
        {
            var brands = new GetAllViewModel
            {
                Brands = await this.brandsService.GetAllAsync<DetailsViewModel>(),
            };

            return this.View(brands);
        }
    }
}
