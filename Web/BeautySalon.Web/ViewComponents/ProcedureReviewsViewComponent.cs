namespace BeautySalon.Web.ViewComponents
{
    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Web.ViewModels.ProcedureReviews.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class ProcedureReviewsViewComponent : ViewComponent
    {
        private readonly IProceduresService proceduresService;

        public ProcedureReviewsViewComponent(IProceduresService proceduresService)
        {
            this.proceduresService = proceduresService;
        }

        public IViewComponentResult Invoke(string id)
        {
            var model = new AllProcedureReviews()
            {
                Reviews = this.proceduresService
                .GetProcedureReviewsAsync<ProcedureReviewViewModel>(id),
            };

            return this.View(model);
        }
    }
}
