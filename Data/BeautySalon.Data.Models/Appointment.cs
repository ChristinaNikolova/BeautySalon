namespace BeautySalon.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;
    using BeautySalon.Data.Models.Enums;

    public class Appointment : BaseDeletableModel<string>
    {
        public Appointment()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Status = Status.Processing;
            this.IsReview = false;
        }

        [Required]
        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }

        [Required]
        public string StylistId { get; set; }

        public virtual ApplicationUser Stylist { get; set; }

        [Required]
        public string ProcedureId { get; set; }

        public virtual Procedure Procedure { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        public string StartTime { get; set; }

        public Status Status { get; set; }

        [MaxLength(DataValidation.AppointmentMaxLenght)]
        public string Comment { get; set; }

        public bool IsReview { get; set; }

        public string ReviewId { get; set; }

        public virtual Review Review { get; set; }
    }
}
