namespace BeautySalon.Web.ViewModels.Appointments.ViewModels
{
    using System.Collections.Generic;

    public class BaseAppointmentsAndUserInfoViewModel
    {
        public IEnumerable<BaseAppoitmentViewModel> Appoitments { get; set; }

        public string UserId { get; set; }
    }
}