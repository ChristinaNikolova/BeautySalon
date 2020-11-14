namespace BeautySalon.Web.ViewModels.StylistsArea.Appointments.ViewModels
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Data.Models.Enums;
    using BeautySalon.Services.Mapping;

    public class AppointmentStylistAreaViewModel : BaseAppoitmentStylistAreaViewModel, IMapFrom<Appointment>
    {
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

        public Status Status { get; set; }
    }
}
