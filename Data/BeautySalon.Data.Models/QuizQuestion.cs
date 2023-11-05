namespace BeautySalon.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Data.Common.Models;

    public class QuizQuestion : BaseDeletableModel<string>
    {
        public QuizQuestion()
        {
            this.Id = Guid.NewGuid().ToString();
            this.QuizAnswers = new HashSet<QuizAnswer>();
        }

        [Required]
        public string Text { get; set; }

        public virtual ICollection<QuizAnswer> QuizAnswers { get; set; }
    }
}
