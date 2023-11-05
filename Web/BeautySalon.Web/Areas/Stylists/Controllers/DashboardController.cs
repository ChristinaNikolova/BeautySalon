namespace BeautySalon.Web.Areas.Stylists.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Services.Data.Questions;
    using BeautySalon.Web.ViewModels.StylistsArea.Dashboard.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : StylistsController
    {
        private readonly IAppointmentsService appointmentsService;
        private readonly IQuestionsService questionsService;
        private readonly UserManager<ApplicationUser> userManager;

        public DashboardController(
            IAppointmentsService appointmentsService,
            IQuestionsService questionsService,
            UserManager<ApplicationUser> userManager)
        {
            this.appointmentsService = appointmentsService;
            this.questionsService = questionsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var stylistId = this.userManager.GetUserId(this.User);

            var model = new DashboardViewModel()
            {
                AppointmentsForToday = await this.appointmentsService.GetAppointmentsForTodayCountAsync(stylistId),
                NewAppointmentRequests = await this.appointmentsService.GetAppointmentsRequestsCountAsync(stylistId),
                NewQuestions = await this.questionsService.GetNewQuestionsCountAsync(stylistId),
            };

            return this.View(model);
        }
    }
}
