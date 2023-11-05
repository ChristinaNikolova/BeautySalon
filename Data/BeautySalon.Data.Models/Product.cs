namespace BeautySalon.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;

    public class Product : BaseDeletableModel<string>
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Quantity = 0;
            this.AverageRating = 0;
            this.ProcedureProducts = new HashSet<ProcedureProduct>();
            this.ProductReviews = new HashSet<ProductReview>();
            this.Likes = new HashSet<ClientProductLike>();
            this.ProductOrders = new HashSet<ProductOrder>();
        }

        [Required]
        [MaxLength(DataValidation.ProductNameMaxLenght)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DataValidation.ProductDescriptionMaxLenght)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [Required]
        public string Picture { get; set; }

        public double AverageRating { get; set; }

        [Required]
        public string BrandId { get; set; }

        public virtual Brand Brand { get; set; }

        [Required]
        public string CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<ProcedureProduct> ProcedureProducts { get; set; }

        public virtual ICollection<ProductReview> ProductReviews { get; set; }

        public virtual ICollection<ClientProductLike> Likes { get; set; }

        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
