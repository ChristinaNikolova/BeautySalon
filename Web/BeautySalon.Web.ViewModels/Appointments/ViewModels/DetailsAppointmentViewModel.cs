namespace BeautySalon.Web.ViewModels.Appointments.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using BeautySalon.Web.ViewModels.StylistsArea.Appointments.ViewModels;

    public class DetailsAppointmentViewModel : AppointmentStylistAreaViewModel, IMapFrom<Appointment>
    {
        public string ClientUsername { get; set; }

        public string ClientPhoneNumber { get; set; }

        public string ClientEmail { get; set; }

        public string ProcedureId { get; set; }

        public decimal ProcedurePrice { get; set; }

        public string Comment { get; set; }
    }
}
