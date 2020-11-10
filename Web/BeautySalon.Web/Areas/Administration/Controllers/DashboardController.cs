namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Web.ViewModels.Administration.Dashboard.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly IAppointmentsService appointmentsService;

        public DashboardController(IAppointmentsService appointmentsService)
        {
            this.appointmentsService = appointmentsService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AllAppointmentsViewModel()
            {
                Appointments = await this.appointmentsService.GetAllAppointmentsForTodayAsync<AppointmentViewModel>(),
            };

            return this.View(model);
        }
    }
}
