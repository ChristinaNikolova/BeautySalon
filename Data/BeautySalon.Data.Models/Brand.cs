namespace BeautySalon.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;

    public class Brand : BaseDeletableModel<string>
    {
        public Brand()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Products = new HashSet<Product>();
        }

        [Required]
        [MaxLength(DataValidation.BrandNameMaxLenght)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DataValidation.BrandDescriptionMaxLenght)]
        public string Description { get; set; }

        [Required]
        public string Logo { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
