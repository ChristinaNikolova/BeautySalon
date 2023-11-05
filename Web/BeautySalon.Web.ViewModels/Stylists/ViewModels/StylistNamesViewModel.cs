namespace BeautySalon.Web.ViewModels.Stylists.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class StylistNamesViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
            => this.FirstName + " " + this.LastName;
    }
}
