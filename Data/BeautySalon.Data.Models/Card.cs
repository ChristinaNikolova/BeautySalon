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
            this.StartDate = DateTime.UtcNow;
            this.IsPaid = false;
            this.CounterUsed = 0;
            this.TotalSumUsedProcedures = 0;
        }

        [Required]
        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        public string TypeCardId { get; set; }

        public virtual TypeCard TypeCard { get; set; }

        public bool IsPaid { get; set; }

        public int CounterUsed { get; set; }

        public int TotalSumUsedProcedures { get; set; }
    }
}
