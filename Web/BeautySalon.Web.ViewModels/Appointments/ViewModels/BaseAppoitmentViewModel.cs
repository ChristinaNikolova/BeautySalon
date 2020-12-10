namespace BeautySalon.Web.ViewModels.Appointments.ViewModels
{
    using System;
    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Models.Enums;
    using BeautySalon.Services.Mapping;

    public class BaseAppoitmentViewModel : IMapFrom<Appointment>
    {
        public string Id { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public string ClientFullName
            => this.ClientFirstName + " " + this.ClientLastName;

        public string StylistId { get; set; }

        public string StylistFirstName { get; set; }

        public string StylistLastName { get; set; }

        public string StylistFullName
            => this.StylistFirstName + " " + this.StylistLastName;

        public string ProcedureName { get; set; }

        public string ShortProcedureName
        {
            get
            {
                return this.ProcedureName.Length > GlobalConstants.ProcedureNameShort
                        ? this.ProcedureName.Substring(0, GlobalConstants.ProcedureNameShort) + "..."
                        : this.ProcedureName;
            }
        }

        public DateTime DateTime { get; set; }

        public string FormattedDate
            => this.DateTime.ToShortDateString();

        public string StartTime { get; set; }

        public Status Status { get; set; }

        public bool IsReview { get; set; }
    }
}
