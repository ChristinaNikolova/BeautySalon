namespace BeautySalon.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ProductOrder
    {
        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public string OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int Quantity { get; set; }
    }
}
