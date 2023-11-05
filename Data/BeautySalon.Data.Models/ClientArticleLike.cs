namespace BeautySalon.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ClientArticleLike
    {
        [Required]
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [Required]
        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }
    }
}
