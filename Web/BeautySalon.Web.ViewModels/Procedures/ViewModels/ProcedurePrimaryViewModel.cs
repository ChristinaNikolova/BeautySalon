namespace BeautySalon.Web.ViewModels.Procedures.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ProcedurePrimaryViewModel : IMapFrom<Procedure>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
