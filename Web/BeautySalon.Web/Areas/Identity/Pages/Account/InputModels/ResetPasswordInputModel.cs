namespace BeautySalon.Web.Areas.Identity.Pages.Account.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;

    public class ResetPasswordInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(DataValidation.PasswordMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.PasswordMinLenght)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = ErrorMessages.DifferentPasswords)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
