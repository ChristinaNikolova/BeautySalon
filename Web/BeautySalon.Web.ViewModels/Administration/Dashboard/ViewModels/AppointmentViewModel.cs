namespace BeautySalon.Web.ViewModels.Administration.Dashboard.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class AppointmentViewModel : IMapFrom<Appointment>
    {
        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public string ClientFullName
            => this.ClientFirstName + " " + this.ClientLastName;

        public string StylistId { get; set; }

        public string StylistFirstName { get; set; }

        public string StylistLastName { get; set; }

        public string StylistFullName
           => this.StylistFirstName + " " + this.StylistLastName;

        public string ProcedureId { get; set; }

        public string ProcedureName { get; set; }

        public string StartTime { get; set; }
    }
}
