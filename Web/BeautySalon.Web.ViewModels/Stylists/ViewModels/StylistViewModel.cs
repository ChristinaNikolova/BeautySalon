namespace BeautySalon.Web.ViewModels.Stylists.ViewModels
{
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

        public double AverageRating { get; set; }

        public string Description { get; set; }

        public string ShortContent
        {
            get
            {
                return this.Description.Length > 50
                        ? this.Description.Substring(0, 50) + "..."
                        : this.Description;
            }
        }
    }
}
