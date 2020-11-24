namespace BeautySalon.Web.ViewModels.Stylists.ViewModels
{
    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class StylistViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Picture { get; set; }

        public string CategoryName { get; set; }

        public string JobTypeName { get; set; }

        public string Description { get; set; }

        public string ShortDescription
        {
            get
            {
                return this.Description.Length > GlobalConstants.StylistShortDescriptionLength
                        ? this.Description.Substring(0, GlobalConstants.StylistShortDescriptionLength) + "..."
                        : this.Description;
            }
        }
    }
}
