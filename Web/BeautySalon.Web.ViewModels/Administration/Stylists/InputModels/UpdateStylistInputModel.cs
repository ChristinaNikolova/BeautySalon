﻿namespace BeautySalon.Web.ViewModels.Administration.Stylists.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Http;

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

        [Required]
        [Display(Name = "Job Type")]
        public JobType JobType { get; set; }

        [Required]
        public Category Category { get; set; }

        [MaxLength(DataValidation.StylistDescriptionMaxLenght)]
        public string Description { get; set; }

        [Required]
        public string Picture { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "New Picture")]
        public IFormFile NewPicture { get; set; }
    }
}
