namespace BeautySalon.Web.ViewModels.Administration.Procedures.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AddProcedureInputModel : IValidatableObject
    {
        [Required]
        [StringLength(DataValidation.ProcedureNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.ProcedureNameMinLenght)]
        public string Name { get; set; }

        [Required]
        [StringLength(DataValidation.ProcedureDescriptionMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.ProcedureDescriptionMinLenght)]
        public string Description { get; set; }

        [Range(typeof(decimal), DataValidation.ProcedureMinPrice, DataValidation.ProcedureMaxPrice)]
        public decimal Price { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        [Required]
        [Display(Name = "Category")]
        [ValidateSelectedDropDownOption]
        public string CategoryId { get; set; }

        public IEnumerable<SelectListItem> SkinTypes { get; set; }

        [Display(Name = "Skin Type")]
        public string SkinTypeId { get; set; }

        [Display(Name = "Is for sensitive skin")]
        public string IsSensitive { get; set; }

        public IList<SelectListItem> SkinProblems { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.CategoryId == GlobalConstants.CategorySkinCareId && this.SkinTypeId.StartsWith("Please select"))
            {
                yield return new ValidationResult(ErrorMessages.InvalidSkinTypeName);
            }

            if (this.CategoryId != GlobalConstants.CategorySkinCareId && this.SkinTypeId != null && !this.SkinTypeId.StartsWith("Please select"))
            {
                yield return new ValidationResult(ErrorMessages.InvalidCombinationCategoryAndSkinType);
            }
        }
    }
}
