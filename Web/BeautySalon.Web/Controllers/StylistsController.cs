namespace BeautySalon.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Stylists;
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
        public async Task<ActionResult<SearchStylistCriteriaViewModelModel>> SearchBy([FromBody] SearchStylistCriteriaInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.CategoryId) && string.IsNullOrWhiteSpace(input.Criteria))
            {
                return this.RedirectToAction("GetAll");
            }

            var stylists = await this.stylistsService.SearchByAsync<StylistViewModel>(input.CategoryId, input.Criteria);

            return new SearchStylistCriteriaViewModelModel { Stylists = stylists };
        }
    }

    public class SearchStylistCriteriaInputModel
    {
        public string CategoryId { get; set; }

        public string Criteria { get; set; }
    }

    public class SearchStylistCriteriaViewModelModel
    {
        public IEnumerable<StylistViewModel> Stylists { get; set; }
    }
}
