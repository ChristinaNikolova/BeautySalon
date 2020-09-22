namespace BeautySalon.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ClientSkinProblem
    {
        [Required]
        public string SkinProblemId { get; set; }

        public virtual SkinProblem SkinProblem { get; set; }

        [Required]
        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }
    }
}
