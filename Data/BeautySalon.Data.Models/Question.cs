﻿namespace BeautySalon.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;

    public class Question : BaseDeletableModel<string>
    {
        public Question()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsAnswered = false;
        }

        [Required]
        [MaxLength(DataValidation.QuestionTitleMaxLenght)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DataValidation.QuestionContentMaxLenght)]
        public string Content { get; set; }

        [Required]
        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }

        [Required]
        public string StylistId { get; set; }

        public virtual ApplicationUser Stylist { get; set; }

        public string AnswerId { get; set; }

        public virtual Answer Answer { get; set; }

        public bool IsAnswered { get; set; }
    }
}
