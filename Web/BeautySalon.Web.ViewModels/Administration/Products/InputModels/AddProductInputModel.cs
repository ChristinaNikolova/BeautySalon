namespace BeautySalon.Web.ViewModels.Administration.Products.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AddProductInputModel
    {
        [Required]
        [StringLength(DataValidation.ProductNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.ProductNameMinLenght)]
        public string Name { get; set; }

        [Required]
        [StringLength(DataValidation.ProductDescriptionMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.ProductDescriptionMinLenght)]
        public string Description { get; set; }

        [Range(typeof(decimal), DataValidation.ProductMinPrice, DataValidation.ProductMaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile Picture { get; set; }

        public IEnumerable<SelectListItem> Brands { get; set; }

        [Required]
        [Display(Name = "Brand")]
        [ValidateSelectedDropDownOption]
        public string BrandId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        [Required]
        [Display(Name = "Category")]
        [ValidateSelectedDropDownOption]
        public string CategoryId { get; set; }
    }
}
