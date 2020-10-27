namespace BeautySalon.Web.ViewModels.Procedures.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ProcedureProductViewModel : IMapFrom<ProcedureProduct>
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductPicture { get; set; }

        public string ProductBrandName { get; set; }
    }
}
