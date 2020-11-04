namespace BeautySalon.Web.ViewModels.SkinTypes.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class SkinTypeDescriptionViewModel : IMapFrom<SkinType>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
