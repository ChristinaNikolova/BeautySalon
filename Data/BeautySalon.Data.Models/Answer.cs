namespace BeautySalon.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;

    public class Answer : BaseDeletableModel<string>
    {
        public Answer()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(DataValidation.AnswerTitleMaxLenght)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DataValidation.AnswerContentMaxLenght)]
        public string Content { get; set; }

        [Required]
        public string StylistId { get; set; }

        public virtual ApplicationUser Stylist { get; set; }

        [Required]
        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }

        [Required]
        public string QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
