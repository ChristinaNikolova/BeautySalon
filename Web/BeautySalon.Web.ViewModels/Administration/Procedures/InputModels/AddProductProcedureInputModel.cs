namespace BeautySalon.Web.ViewModels.Administration.Procedures.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;

    public class AddProductProcedureInputModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        [StringLength(DataValidation.ProductNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.ProductNameMinLenght)]
        public string ProductName { get; set; }
    }
}
