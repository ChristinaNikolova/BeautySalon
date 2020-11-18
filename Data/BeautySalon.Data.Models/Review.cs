namespace BeautySalon.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;

    public class Review : BaseDeletableModel<string>
    {
        public Review()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string ProcedureId { get; set; }

        public virtual Procedure Procedure { get; set; }

        [Required]
        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }

        [Required]
        public string AppointmentId { get; set; }

        public virtual Appointment Appointment { get; set; }

        [Required]
        [MaxLength(DataValidation.ProcedureReviewContentMaxLenght)]
        public string Content { get; set; }

        public int? Points { get; set; }

        public DateTime Date { get; set; }
    }
}
