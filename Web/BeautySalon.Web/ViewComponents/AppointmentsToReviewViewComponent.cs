namespace BeautySalon.Web.ViewComponents
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Web.ViewModels.Appointments.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class AppointmentsToReviewViewComponent : ViewComponent
    {
        private readonly IAppointmentsService appointmentsService;

        public AppointmentsToReviewViewComponent(IAppointmentsService appointmentsService)
        {
            this.appointmentsService = appointmentsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var model = new AllBaseAppoitmentViewModel()
            {
                Appoitments = await this.appointmentsService
                .GetAppointmentsToReviewAsync<BaseAppoitmentViewModel>(userId),
            };

            return this.View(model);
        }
    }
}
