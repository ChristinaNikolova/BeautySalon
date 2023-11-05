namespace BeautySalon.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ClientProductLike
    {
        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }
    }
}
