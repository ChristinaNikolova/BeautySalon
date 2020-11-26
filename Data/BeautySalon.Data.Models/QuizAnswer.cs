namespace BeautySalon.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Data.Common.Models;

    public class QuizAnswer : BaseDeletableModel<string>
    {
        public QuizAnswer()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Text { get; set; }

        [Required]
        public string QuizQuestionId { get; set; }

        public virtual Question QuizQuestion { get; set; }
    }
}
