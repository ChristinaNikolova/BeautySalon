namespace BeautySalon.Web.ViewModels.SkinProblems.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class SkinProblemName : IMapFrom<SkinProblem>
    {
        public string Name { get; set; }
    }
}
