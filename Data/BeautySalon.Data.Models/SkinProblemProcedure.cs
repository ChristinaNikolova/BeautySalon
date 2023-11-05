namespace BeautySalon.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SkinProblemProcedure
    {
        [Required]
        public string SkinProblemId { get; set; }

        public virtual SkinProblem SkinProblem { get; set; }

        [Required]
        public string ProcedureId { get; set; }

        public virtual Procedure Procedure { get; set; }
    }
}
