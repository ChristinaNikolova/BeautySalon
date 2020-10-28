namespace BeautySalon.Web.ViewModels.SkinTypes.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class SkinTypeViewModel : IMapFrom<SkinType>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
