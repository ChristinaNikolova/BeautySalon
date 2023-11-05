namespace BeautySalon.Web.ViewModels.Administration.Dashboard.ViewModels
{
    using System.Collections.Generic;

    public class AllAppointmentsViewModel
    {
        public IEnumerable<AppointmentViewModel> Appointments { get; set; }
    }
}
