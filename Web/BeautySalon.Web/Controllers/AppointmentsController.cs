namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Web.ViewModels.Appoitments.InputModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class AppointmentsController : Controller
    {
        private readonly IAppointmentsService appointmentsService;
        private readonly IUsersService usersService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<ApplicationUser> userManager;

        public AppointmentsController(IAppointmentsService appointmentsService, IUsersService usersService, ICategoriesService categoriesService, UserManager<ApplicationUser> userManager)
        {
            this.appointmentsService = appointmentsService;
            this.usersService = usersService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Book()
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = await this.usersService.GetUserDataAsync<BookAppointmentInputModel>(userId);

            var categories = await this.categoriesService.GetAllAsSelectListItemAsync();
            model.Categories = categories;

            return this.View(model);
        }
    }
}
