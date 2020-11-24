namespace BeautySalon.Web.Areas.Stylists.ViewComponents
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Web.ViewModels.Appointments.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class RequestAppoitmentViewComponent : ViewComponent
    {
        private readonly IAppointmentsService appointmentsService;

        public RequestAppoitmentViewComponent(IAppointmentsService appointmentsService)
        {
            this.appointmentsService = appointmentsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string stylistId)
        {
            var model = new AllBaseAppoitmentViewModel()
            {
                Appoitments = await this.appointmentsService
                .GetRequestsAsync<BaseAppoitmentViewModel>(stylistId),
            };

            return this.View(model);
        }
    }
}
