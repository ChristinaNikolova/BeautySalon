namespace BeautySalon.Web.Areas.Stylists.Controllers
{
    using BeautySalon.Common;
    using BeautySalon.Web.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.StylistRoleName)]
    [Area(GlobalConstants.StylistsArea)]
    public class StylistsController : BaseController
    {
    }
}
