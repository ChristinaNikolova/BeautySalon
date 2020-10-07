namespace BeautySalon.Web.Areas.Identity.Pages.Account.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using Microsoft.AspNetCore.Http;

    public class UpdateProfileInputModel
    {
        [Required]
        [StringLength(DataValidation.UserFirstNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.UserFirstNameMinLenght)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(DataValidation.UserLastNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.UserLastNameMinLenght)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(DataValidation.AddressMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.AddressMinLenght)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        public IFormFile Picture { get; set; }

        [Display(Name = "Skin type")]
        public string SkinType { get; set; }

        [Display(Name = "Sensitive skin")]
        public string IsSkinSensitive { get; set; }

        [Display(Name = "Job Category")]
        public string Category { get; set; }

        [Display(Name = "Job Type")]
        public string JobType { get; set; }

        [Display(Name = "About me")]
        public string Description { get; set; }
    }
}
