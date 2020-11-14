namespace BeautySalon.Web.ViewModels.StylistsArea.Appointments.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Models.Enums;
    using BeautySalon.Services.Mapping;

    public class DetailsAppointmentStylistAreaViewModel : AppointmentStylistAreaViewModel, IMapFrom<Appointment>
    {
        public string ClientUsername { get; set; }

        public string ClientPhoneNumber { get; set; }

        public string ClientEmail { get; set; }

        public string ProcedureId { get; set; }

        public string StylistId { get; set; }

        public string StylistFirstName { get; set; }

        public string StylistLastName { get; set; }

        public string StylistFullName
            => this.StylistFirstName + " " + this.StylistLastName;

        public Status Status { get; set; }

        public string Comment { get; set; }
    }
}
