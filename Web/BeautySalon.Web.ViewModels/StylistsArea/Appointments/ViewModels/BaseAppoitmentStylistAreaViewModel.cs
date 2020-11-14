namespace BeautySalon.Web.ViewModels.StylistsArea.Appointments.ViewModels
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class BaseAppoitmentStylistAreaViewModel : IMapFrom<Appointment>
    {
        public string Id { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public string ClientFullName
            => this.ClientFirstName + " " + this.ClientLastName;

        public string StylistFirstName { get; set; }

        public string StylistLastName { get; set; }

        public string StylistFullName
            => this.StylistFirstName + " " + this.StylistLastName;

        public string ProcedureName { get; set; }

        public DateTime DateTime { get; set; }

        public string FormattedDate
            => this.DateTime.ToShortDateString();

        public string StartTime { get; set; }
    }
}
