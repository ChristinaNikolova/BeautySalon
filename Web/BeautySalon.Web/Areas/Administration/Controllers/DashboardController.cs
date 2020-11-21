namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Services.Data.ChatMessages;
    using BeautySalon.Web.ViewModels.Administration.Dashboard.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly IAppointmentsService appointmentsService;
        private readonly IChatsService chatsService;

        public DashboardController(
            IAppointmentsService appointmentsService,
            IChatsService chatsService)
        {
            this.appointmentsService = appointmentsService;
            this.chatsService = chatsService;
        }

        public async Task<IActionResult> Index()
        {
            var isNewChatMessage = await this.chatsService.IsNewMessageAsync();

            var model = new DashboardViewModel()
            {
                Appointments = await this.appointmentsService.GetAllAppointmentsForTodayAsync<AppointmentViewModel>(),
                IsNewChatMessage = isNewChatMessage,
            };

            return this.View(model);
        }
    }
}
