namespace BeautySalon.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;

    public class SkinType : BaseDeletableModel<string>
    {
        public SkinType()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Clients = new HashSet<ApplicationUser>();
            this.Procedures = new HashSet<Procedure>();
        }

        [Required]
        [MaxLength(DataValidation.SkinTypeNameMaxLenght)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DataValidation.SkinTypeDescriptionMaxLenght)]
        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> Clients { get; set; }

        public virtual ICollection<Procedure> Procedures { get; set; }
    }
}
