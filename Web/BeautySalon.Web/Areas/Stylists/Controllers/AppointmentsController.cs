﻿namespace BeautySalon.Web.Areas.Stylists.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Web.ViewModels.StylistsArea.Appointments.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class AppointmentsController : StylistsController
    {
        private readonly IAppointmentsService appointmentsService;
        private readonly UserManager<ApplicationUser> userManager;

        public AppointmentsController(
            IAppointmentsService appointmentsService,
            UserManager<ApplicationUser> userManager)
        {
            this.appointmentsService = appointmentsService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> GetAppointments()
        {
            var stylistId = this.userManager.GetUserId(this.User);

            var model = new AllAppointmentStylistAreaViewModel()
            {
                Appointments = await this.appointmentsService.GetAllForStylistAsync<AppointmentStylistAreaViewModel>(stylistId),
            };

            return this.Json(model);
        }

        public async Task<IActionResult> GetInfoCurrentAppointment(string id)
        {
            var model = await this.appointmentsService.GetDetailsAsync<DetailsAppointmentStylistAreaViewModel>(id);

            return this.View(model);
        }

        public async Task<IActionResult> Cancel(string id)
        {
            await this.appointmentsService.CancelAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
