namespace BeautySalon.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;
    using BeautySalon.Data.Models.Enums;

    public class Order : BaseDeletableModel<string>
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Status = Status.Processing;
            this.ProductOrders = new HashSet<ProductOrder>();
        }

        [Required]
        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }

        [MaxLength(DataValidation.OrderCommentMaxLenght)]
        public string Comment { get; set; }

        public decimal TotalPrice { get; set; }

        public Status Status { get; set; }

        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
