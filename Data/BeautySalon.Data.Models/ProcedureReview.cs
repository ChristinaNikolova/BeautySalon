namespace BeautySalon.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;

    public class ProcedureReview
    {
        [Required]
        public string ProcedureId { get; set; }

        public virtual Procedure Procedure { get; set; }

        [Required]
        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }

        [Required]
        [MaxLength(DataValidation.ProcedureReviewContentMaxLenght)]
        public string Content { get; set; }

        public int? Points { get; set; }
    }
}
