namespace BeautySalon.Web.ViewModels.StylistsArea.Appointments.ViewModels
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class AppointmentStylistAreaViewModel : IMapFrom<Appointment>
    {
        public string Id { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public string ClientFullName
            => this.ClientFirstName + " " + this.ClientLastName;

        public string ProcedureName { get; set; }

        public DateTime DateTime { get; set; }

        public string StartTime { get; set; }

        public int FormattedStartTime
        {
            get
            {
                var splittedHour = this.StartTime.Split(":")[0];
                var hourAsInt = int.Parse(splittedHour);
                return hourAsInt;
            }
        }

        public DateTime FormattedStart
            => this.DateTime.AddHours(this.FormattedStartTime);

        public DateTime FormattedEnd
            => this.DateTime.AddHours(this.FormattedStartTime + 1);
    }
}
