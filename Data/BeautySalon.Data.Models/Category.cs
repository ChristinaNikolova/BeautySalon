namespace BeautySalon.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;

    public class Category : BaseDeletableModel<string>
    {
        public Category()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Articles = new HashSet<Article>();
            this.Procedures = new HashSet<Procedure>();
            this.Products = new HashSet<Product>();
            this.Stylists = new HashSet<ApplicationUser>();
        }

        [Required]
        [MaxLength(DataValidation.CategoryNameMaxLenght)]
        public string Name { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public virtual ICollection<Procedure> Procedures { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<ApplicationUser> Stylists { get; set; }
    }
}
