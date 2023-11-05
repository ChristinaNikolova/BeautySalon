namespace BeautySalon.Web.ViewModels.Administration.Stylists.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllStylistsAdministrationViewModel
    {
        public IEnumerable<StylistAdministrationViewModel> Stylists { get; set; }

        [Required]
        [EmailAddress]
        public string StylistEmail { get; set; }
    }
}
