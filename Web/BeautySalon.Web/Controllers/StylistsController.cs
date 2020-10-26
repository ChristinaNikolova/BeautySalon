namespace BeautySalon.Web.Controllers
{
    using BeautySalon.Services.Data.Stylists;
    using BeautySalon.Web.ViewModels.Stylists.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

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
    }
}
