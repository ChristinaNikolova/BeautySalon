namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Stylists;
    using BeautySalon.Web.ViewModels.Administration.Stylists.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class StylistsController : AdministrationController
    {
        private readonly IStylistsService stylistsService;

        public StylistsController(IStylistsService stylistsService)
        {
            this.stylistsService = stylistsService;
        }

        public async Task<IActionResult> GetAll()
        {
            var model = new AllStylistsAdministrationViewModel()
            {
                Stylists = await this.stylistsService.GetAllAdministrationAsync<StylistAdministrationViewModel>(),
            };

            return this.View(model);
        }
    }
}
