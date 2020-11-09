namespace BeautySalon.Web.ViewModels.Administration.Products.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ProcedureStylistViewModel : IMapFrom<Procedure>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
