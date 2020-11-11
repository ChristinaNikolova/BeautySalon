namespace BeautySalon.Web.Areas.Stylists.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DashboardController : StylistsController
    {
        public IActionResult Index()
        {
            
            return this.View();
        }
    }
}
