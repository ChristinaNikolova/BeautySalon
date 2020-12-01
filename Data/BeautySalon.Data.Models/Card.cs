namespace BeautySalon.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Data.Common.Models;

    public class Card : BaseDeletableModel<string>
    {
        public Card()
        {
            this.Id = Guid.NewGuid().ToString();
            this.StartDate = DateTime.UtcNow.AddDays(1);
            this.IsPaid = false;
        }

        [Required]
        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndEnd { get; set; }

        [Required]
        public string TypeCardId { get; set; }

        public virtual TypeCard TypeCard { get; set; }

        public bool IsPaid { get; set; }
    }
}
