﻿namespace BeautySalon.Web.ViewComponents
{
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

        public IViewComponentResult Invoke(string id)
        {
            var model = new AllProcedureReviewsViewModel()
            {
                Reviews = this.proceduresService
                .GetProcedureReviews<ProcedureReviewViewModel>(id),
            };

            return this.View(model);
        }
    }
}
