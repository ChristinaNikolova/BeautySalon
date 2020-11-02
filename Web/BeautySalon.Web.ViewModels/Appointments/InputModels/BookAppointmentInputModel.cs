namespace BeautySalon.Web.ViewModels.Appoitments.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class BookAppointmentInputModel : IMapFrom<ApplicationUser>
    {
        [Required]
        [StringLength(DataValidation.UserFirstNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.UserFirstNameMinLenght)]
        [Display(Name = "Firstname")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(DataValidation.UserLastNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.UserLastNameMinLenght)]
        [Display(Name = "Lastname")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Stylist")]
        public string StylistName { get; set; }

        [Required]
        [Display(Name = "Procedure")]
        public string ProcedureName { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string Time { get; set; }

        [MaxLength(DataValidation.AppointmentMaxLenght, ErrorMessage = "The {0} must be at max {1} characters long.")]
        public string Comment { get; set; }
    }
}
