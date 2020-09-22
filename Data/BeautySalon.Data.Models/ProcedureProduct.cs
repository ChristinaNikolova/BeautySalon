namespace BeautySalon.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ProcedureProduct
    {
        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public string ProcedureId { get; set; }

        public virtual Procedure Procedure { get; set; }
    }
}
