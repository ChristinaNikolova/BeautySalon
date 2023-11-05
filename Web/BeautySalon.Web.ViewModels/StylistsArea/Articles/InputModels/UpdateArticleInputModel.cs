namespace BeautySalon.Web.ViewModels.StylistsArea.Articles.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using BeautySalon.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UpdateArticleInputModel : IMapFrom<Article>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(DataValidation.ArticleTitleMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.ArticleTitleMinLenght)]
        public string Title { get; set; }

        [Required]
        [StringLength(DataValidation.ArticleContentMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.ArticleContentMinLenght)]
        public string Content { get; set; }

        public string Picture { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "New Picture")]
        public IFormFile NewPicture { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        [Required]
        [Display(Name = "Category")]
        [ValidateSelectedDropDownOption]
        public string CategoryId { get; set; }
    }
}
