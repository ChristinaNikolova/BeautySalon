namespace BeautySalon.Web.Areas.Identity.Pages.Account.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;

    public class EnableAuthenticatorInputModel
    {
        [Required]
        [StringLength(DataValidation.VerificationCodeMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.VerificationCodeMinLenght)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }
    }
}
