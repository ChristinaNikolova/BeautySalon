namespace BeautySalon.Web.Areas.Identity.Pages.Account.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;

    public class ExternalLoginInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(DataValidation.UsernameAllowedSymbols, ErrorMessage = ErrorMessages.UsernameErrorRegex)]
        [StringLength(DataValidation.UsernameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.UsernameMinLenght)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
    }
}
