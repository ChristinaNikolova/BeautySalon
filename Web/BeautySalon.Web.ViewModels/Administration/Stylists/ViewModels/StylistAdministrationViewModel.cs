namespace BeautySalon.Web.ViewModels.Administration.Stylists.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using BeautySalon.Web.ViewModels.Stylists.ViewModels;

    public class StylistAdministrationViewModel : StylistNamesViewModel, IMapFrom<ApplicationUser>
    {
        public string CategoryName { get; set; }

        public string JobTypeName { get; set; }
    }
}
