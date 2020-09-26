namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using BeautySalon.Common;
    using BeautySalon.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area(GlobalConstants.AdminArea)]
    public class AdministrationController : BaseController
    {
    }
}
