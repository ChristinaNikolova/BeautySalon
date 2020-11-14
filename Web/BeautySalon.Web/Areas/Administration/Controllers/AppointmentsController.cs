namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Web.ViewModels.StylistsArea.Appointments.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class AppointmentsController : AdministrationController
    {
        private readonly IAppointmentsService appointmentsService;

        public AppointmentsController(IAppointmentsService appointmentsService)
        {
            this.appointmentsService = appointmentsService;
        }

        public async Task<IActionResult> GetHistory()
        {
            var model = new AllBaseAppoitmentStylistAreaViewModel()
            {
                Appoitments = await this.appointmentsService.GetHistoryAllStylistsAsync<BaseAppoitmentStylistAreaViewModel>(),
            };

            return this.View(model);
        }
    }
}
