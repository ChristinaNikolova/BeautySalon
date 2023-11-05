namespace BeautySalon.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ProcedureStylist
    {
        [Required]
        public string StylistId { get; set; }

        public virtual ApplicationUser Stylist { get; set; }

        [Required]
        public string ProcedureId { get; set; }

        public virtual Procedure Procedure { get; set; }
    }
}
