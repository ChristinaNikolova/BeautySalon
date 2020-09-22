namespace BeautySalon.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;

    public class SkinProblem : BaseDeletableModel<string>
    {
        public SkinProblem()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ClientSkinProblems = new HashSet<ClientSkinProblem>();
            this.SkinProblemProcedures = new HashSet<SkinProblemProcedure>();
        }

        [Required]
        [MaxLength(DataValidation.SkinProblemNameMaxLenght)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DataValidation.SkinProblemDescriptionMaxLenght)]
        public string Description { get; set; }

        public virtual ICollection<ClientSkinProblem> ClientSkinProblems { get; set; }

        public virtual ICollection<SkinProblemProcedure> SkinProblemProcedures { get; set; }
    }
}
