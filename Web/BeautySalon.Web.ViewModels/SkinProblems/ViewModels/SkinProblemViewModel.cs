namespace BeautySalon.Web.ViewModels.SkinProblems.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class SkinProblemViewModel : IMapFrom<SkinProblem>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
