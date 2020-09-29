namespace BeautySalon.Web.Areas.Identity.Pages.Account.InputModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class RegisterInputModel
    {
        [Required]
        [RegularExpression(DataValidation.UsernameAllowedSymbols, ErrorMessage = ErrorMessages.UsernameErrorRegex)]
        [StringLength(DataValidation.UsernameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.UsernameMinLenght)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(DataValidation.PasswordMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.PasswordMinLenght)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = ErrorMessages.DifferentPasswords)]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(DataValidation.UserFirstNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.UserFirstNameMinLenght)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(DataValidation.UserLastNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.UserLastNameMinLenght)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(DataValidation.AddressMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.AddressMinLenght)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        public DateTime Birthday { get; set; }

        public IFormFile Picture { get; set; }
    }
}
