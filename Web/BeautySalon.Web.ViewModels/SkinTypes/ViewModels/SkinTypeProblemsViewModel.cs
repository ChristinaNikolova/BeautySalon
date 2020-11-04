namespace BeautySalon.Web.ViewModels.SkinTypes.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class SkinTypeProblemsViewModel : IMapFrom<SkinProblem>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
