namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Stylists;
    using BeautySalon.Web.ViewModels.Stylists.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class StylistsController : BaseController
    {
        private readonly IStylistsService stylistsService;

        public StylistsController(IStylistsService stylistsService)
        {
            this.stylistsService = stylistsService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var model = new AllStylistsViewModel()
            {
                Stylists = await this.stylistsService.GetAllAsync<StylistViewModel>(),
            };

            return this.View(model);
        }

        public async Task<IActionResult> GetDetails(string id)
        {
            var model = await this.stylistsService.GetStylistDetailsAsync<DetailsStylistViewModel>(id);

            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<AllStylistsViewModel>> SearchBy([FromBody] string categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
            {
                return this.RedirectToAction(nameof(this.GetAll));
            }

            var stylists = await this.stylistsService.SearchByCategoryAsync<StylistViewModel>(categoryId);

            return new AllStylistsViewModel { Stylists = stylists };
        }

        [HttpPost]
        public async Task<ActionResult<AllStylistNamesViewModel>> GetStylistsByCategory([FromBody] string categoryId)
        {
            var stylistNames = await this.stylistsService.SearchByCategoryAsync<StylistNamesViewModel>(categoryId);

            return new AllStylistNamesViewModel { StylistNames = stylistNames };
        }
    }
}
