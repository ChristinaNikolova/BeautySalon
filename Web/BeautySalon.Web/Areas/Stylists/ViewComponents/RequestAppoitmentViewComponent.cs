namespace BeautySalon.Web.Areas.Stylists.ViewComponents
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Web.ViewModels.StylistsArea.Appointments.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class RequestAppoitmentViewComponent : ViewComponent
    {
        private readonly IAppointmentsService appointmentsService;

        public RequestAppoitmentViewComponent(IAppointmentsService appointmentsService)
        {
            this.appointmentsService = appointmentsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new AllBaseAppoitmentStylistAreaViewModel()
            {
                Appoitments = await this.appointmentsService
                .GetRequestsAsync<BaseAppoitmentStylistAreaViewModel>(),
            };

            return this.View(model);
        }
    }
}
