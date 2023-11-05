namespace BeautySalon.Web.ViewModels.Administration.Dashboard.ViewModels
{
    using System.Collections.Generic;

    public class DashboardViewModel
    {
        public IEnumerable<AppointmentViewModel> Appointments { get; set; }

        public bool IsNewChatMessage { get; set; }
    }
}
