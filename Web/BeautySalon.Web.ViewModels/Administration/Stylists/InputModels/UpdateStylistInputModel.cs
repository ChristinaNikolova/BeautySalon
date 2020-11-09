namespace BeautySalon.Web.ViewModels.Administration.Stylists.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using BeautySalon.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UpdateStylistInputModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        [Required]
        [StringLength(DataValidation.UserFirstNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.UserFirstNameMinLenght)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(DataValidation.UserLastNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.UserLastNameMinLenght)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public IEnumerable<SelectListItem> JobTypes { get; set; }

        [Required]
        [Display(Name = "Job Type")]
        [ValidateSelectedDropDownOption]
        public string JobTypeId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        [Required]
        [Display(Name = "Category")]
        [ValidateSelectedDropDownOption]
        public string CategoryId { get; set; }

        [Required]
        [StringLength(DataValidation.StylistDescriptionMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.StylistDescriptionMinLenght)]
        public string Description { get; set; }

        public string Picture { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "New Picture")]
        public IFormFile NewPicture { get; set; }
    }
}
