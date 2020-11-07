﻿namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Web.ViewModels.Appointments.InputModels;
    using BeautySalon.Web.ViewModels.Appointments.ViewModels;
    using BeautySalon.Web.ViewModels.Appoitments.InputModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class AppointmentsController : BaseController
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

        [HttpPost]
        public async Task<IActionResult> Book(BookAppointmentInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);

            if (!this.ModelState.IsValid)
            {
                var categories = await this.categoriesService.GetAllAsSelectListItemAsync();
                input.Categories = categories;
                input.Id = userId;

                // TODO Fix bug with selectedCategory
                return this.View(input);
            }

            await this.appointmentsService.CreateAsync(userId, input.StylistId, input.ProcedureId, input.Date, input.Time, input.Comment);

            // TODO Redirect to AllAppointmnets
            return this.Redirect("/");
        }

        [HttpPost]
        public async Task<ActionResult<FreeTimesViewModel>> GetFreeTimes([FromBody] WantedAppointmentInputModel input)
        {
            var freeHours = await this.appointmentsService.GetFreeHoursAsync(input.SelectedDate, input.SelectedStylistId);

            return new FreeTimesViewModel { FreeHours = freeHours };
        }
    }
}
