namespace BeautySalon.Web.Controllers
{
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
    }
}
