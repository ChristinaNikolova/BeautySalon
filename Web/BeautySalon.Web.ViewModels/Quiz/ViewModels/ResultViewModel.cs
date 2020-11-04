namespace BeautySalon.Web.ViewModels.Quiz.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using BeautySalon.Web.ViewModels.SkinTypes.ViewModels;

    public class ResultViewModel : IMapFrom<SkinType>
    {
        public SkinTypeDescriptionViewModel SkinType { get; set; }

        public bool IsSkinSensitive { get; set; }
    }
}
