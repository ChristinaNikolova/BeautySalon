namespace BeautySalon.Web.ViewComponents
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Web.ViewModels.Procedures.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class ProdecureProductsViewComponent : ViewComponent
    {
        private readonly IProceduresService proceduresService;

        public ProdecureProductsViewComponent(IProceduresService proceduresService)
        {
            this.proceduresService = proceduresService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var model = new AllProcedureProductsViewModel()
            {
                Products = await this.proceduresService
                .GetProcedureProductsAsync<ProcedureProductViewModel>(id),
            };

            return this.View(model);
        }
    }
}
