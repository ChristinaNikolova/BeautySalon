namespace BeautySalon.Web.ViewComponents
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Web.ViewModels.Procedures.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class ProceduresUseProductViewComponent : ViewComponent
    {
        private readonly IProceduresService proceduresService;

        public ProceduresUseProductViewComponent(IProceduresService proceduresService)
        {
            this.proceduresService = proceduresService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string productId)
        {
            var model = new AllProcedurePrimaryViewModel()
            {
                Procedures = await this.proceduresService
                .GetProceduresUseProductAsync<ProcedurePrimaryViewModel>(productId),
            };

            return this.View(model);
        }
    }
}
