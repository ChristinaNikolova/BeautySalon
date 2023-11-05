namespace BeautySalon.Web.ViewModels.SkinTypes.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class SkinTypeDescriptionViewModel : SkinTypeViewModel, IMapFrom<SkinType>
    {
        public string Description { get; set; }
    }
}
