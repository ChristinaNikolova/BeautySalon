namespace BeautySalon.Web.ViewComponents
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Web.ViewModels.Procedures.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class ProcedureReviewsViewComponent : ViewComponent
    {
        private readonly IProceduresService proceduresService;

        public ProcedureReviewsViewComponent(IProceduresService proceduresService)
        {
            this.proceduresService = proceduresService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var model = new AllProcedureReviewsViewModel()
            {
                Reviews = await this.proceduresService
                .GetProcedureReviewsAsync<ProcedureReviewViewModel>(id),
            };

            return this.View(model);
        }
    }
}
