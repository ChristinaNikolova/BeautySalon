namespace BeautySalon.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;

    public class TypeCard : BaseDeletableModel<string>
    {
        public TypeCard()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Cards = new HashSet<Card>();
        }

        [Required]
        [MaxLength(DataValidation.TypeCardNameMaxLength)]
        public string Name { get; set; }

        public int Price { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
    }
}
