namespace BeautySalon.Web.ViewModels.Procedures.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ProcedureNameViewModel : IMapFrom<ProcedureStylist>
    {
        public string ProcedureId { get; set; }

        public string ProcedureName { get; set; }
    }
}
