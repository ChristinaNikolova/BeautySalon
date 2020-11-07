namespace BeautySalon.Web.ViewModels.Administration.Stylists.ViewModels
{
    using System.Collections.Generic;

    public class AllStylistsAdministrationViewModel
    {
        public IEnumerable<StylistAdministrationViewModel> Stylists { get; set; }
    }
}
