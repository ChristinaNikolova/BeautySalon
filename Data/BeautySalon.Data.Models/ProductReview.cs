namespace BeautySalon.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;

    public class ProductReview
    {
        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }

        [Required]
        [MaxLength(DataValidation.ProductReviewContentMaxLenght)]
        public string Content { get; set; }

        public int? Points { get; set; }
    }
}
