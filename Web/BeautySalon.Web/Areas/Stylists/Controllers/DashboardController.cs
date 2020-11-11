namespace BeautySalon.Web.Areas.Stylists.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : StylistsController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
