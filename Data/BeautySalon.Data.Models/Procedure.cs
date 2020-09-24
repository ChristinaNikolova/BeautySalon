namespace BeautySalon.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;

    public class Procedure : BaseDeletableModel<string>
    {
        public Procedure()
        {
            this.Id = Guid.NewGuid().ToString();
            this.AverageRating = 0;
            this.SkinProblemProcedures = new HashSet<SkinProblemProcedure>();
            this.ProcedureProducts = new HashSet<ProcedureProduct>();
            this.ProcedureReviews = new HashSet<ProcedureReview>();
            this.ProcedureStylists = new HashSet<ProcedureStylist>();
            this.Appointments = new HashSet<Appointment>();
        }

        [Required]
        [MaxLength(DataValidation.ProcedureNameMaxLenght)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DataValidation.ProcedureDescriptionMaxLenght)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public double AverageRating { get; set; }

        [Required]
        public string CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public string SkinTypeId { get; set; }

        public virtual SkinType SkinType { get; set; }

        public virtual ICollection<SkinProblemProcedure> SkinProblemProcedures { get; set; }

        public virtual ICollection<ProcedureProduct> ProcedureProducts { get; set; }

        public virtual ICollection<ProcedureReview> ProcedureReviews { get; set; }

        public virtual ICollection<ProcedureStylist> ProcedureStylists { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
