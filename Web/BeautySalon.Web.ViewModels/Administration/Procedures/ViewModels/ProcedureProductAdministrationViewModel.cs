namespace BeautySalon.Web.ViewModels.Administration.Procedures.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ProcedureProductAdministrationViewModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
