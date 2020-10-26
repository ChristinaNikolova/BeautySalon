namespace BeautySalon.Web.ViewModels.Procedures.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ProceduresByCategoryViewModel : IMapFrom<Category>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CategoryNameToDisplay
            => this.Name + " Procedures";
    }
}
