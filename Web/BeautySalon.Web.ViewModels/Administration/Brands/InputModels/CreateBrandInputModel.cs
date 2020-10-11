namespace BeautySalon.Web.ViewModels.Administration.Brands.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using Microsoft.AspNetCore.Http;

    public class CreateBrandInputModel
    {
        [Required]
        [StringLength(DataValidation.BrandNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.BrandNameMinLenght)]
        public string Name { get; set; }

        [Required]
        [StringLength(DataValidation.BrandDescriptionMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.BrandDescriptionMinLenght)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile Logo { get; set; }
    }
}
