namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Stylists;
    using BeautySalon.Web.ViewModels.Stylists.InputModels;
    using BeautySalon.Web.ViewModels.Stylists.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class StylistsController : Controller
    {
        private readonly IStylistsService stylistsService;

        public StylistsController(IStylistsService stylistsService)
        {
            this.stylistsService = stylistsService;
        }

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
        public async Task<ActionResult<AllStylistsViewModel>> SearchBy([FromBody] SearchStylistCriteriaInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.CategoryId) && string.IsNullOrWhiteSpace(input.Criteria))
            {
                return this.RedirectToAction("GetAll");
            }

            var stylists = await this.stylistsService.SearchByAsync<StylistViewModel>(input.CategoryId, input.Criteria);

            return new AllStylistsViewModel { Stylists = stylists };
        }

        [HttpPost]
        public async Task<ActionResult<AllStylistNamesViewModel>> GetStylistsByCategory([FromBody] string categoryId)
        {
            var stylistNames = await this.stylistsService.GetStylistsByCategoryAsync<StylistNamesViewModel>(categoryId);

            return new AllStylistNamesViewModel { StylistNames = stylistNames };
        }
    }
}
