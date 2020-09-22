namespace BeautySalon.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;

    public class JobType : BaseDeletableModel<string>
    {
        public JobType()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Stylists = new HashSet<ApplicationUser>();
        }

        [Required]
        [MaxLength(DataValidation.JobTypeNameMaxLenght)]
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Stylists { get; set; }
    }
}
