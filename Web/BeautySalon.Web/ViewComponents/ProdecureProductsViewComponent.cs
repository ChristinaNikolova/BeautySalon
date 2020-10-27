namespace BeautySalon.Web.ViewComponents
{
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

        public IViewComponentResult Invoke(string id)
        {
            var model = new AllProcedureProductsViewModel()
            {
                Products = this.proceduresService
                .GetProcedureProducts<ProcedureProductViewModel>(id),
            };

            return this.View(model);
        }
    }
}
