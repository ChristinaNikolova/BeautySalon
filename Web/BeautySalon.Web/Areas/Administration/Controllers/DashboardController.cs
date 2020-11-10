namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using BeautySalon.Services.Data.Settings;
    using BeautySalon.Web.ViewModels.Administration.Dashboard;

    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        public DashboardController()
        {
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}
