namespace BeautySalon.Web.ViewModels.Appointments.ViewModels
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class AppointmentViewModel : IMapFrom<Appointment>
    {
        public string Id { get; set; }

        public string ClientId { get; set; }

        public string StylistId { get; set; }

        public string StylistFirstName { get; set; }

        public string StylistLastName { get; set; }

        public string StylistFullName
            => this.StylistFirstName + " " + this.StylistLastName;

        public string ProcedureId { get; set; }

        public string ProcedureName { get; set; }

        public DateTime DateTime { get; set; }

        public string FormattedDateTime
            => this.DateTime.ToShortDateString();

        public string StartTime { get; set; }
    }
}
