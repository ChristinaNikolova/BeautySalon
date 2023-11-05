namespace BeautySalon.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Models;

    public class Article : BaseDeletableModel<string>
    {
        public Article()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Likes = new HashSet<ClientArticleLike>();
            this.Comments = new HashSet<Comment>();
        }

        [Required]
        [MaxLength(DataValidation.ArticleTitleMaxLenght)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DataValidation.ArticleContentMaxLenght)]
        public string Content { get; set; }

        [Required]
        public string Picture { get; set; }

        [Required]
        public string CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        public string StylistId { get; set; }

        public virtual ApplicationUser Stylist { get; set; }

        public virtual ICollection<ClientArticleLike> Likes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
