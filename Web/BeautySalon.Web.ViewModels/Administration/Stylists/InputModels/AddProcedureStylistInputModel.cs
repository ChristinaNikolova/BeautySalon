namespace BeautySalon.Web.ViewModels.Administration.Stylists.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;

    public class AddProcedureStylistInputModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Procedure Name")]
        [StringLength(DataValidation.ProcedureNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.ProcedureNameMinLenght)]
        public string ProcedureName { get; set; }
    }
}
