namespace BeautySalon.Web.ViewComponents
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Services.Data.Reviews;
    using BeautySalon.Web.ViewModels.Reviews.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent]
    public class ReviewViewComponent : ViewComponent
    {
        private readonly IReviewsService reviewsService;

        public ReviewViewComponent(IReviewsService reviewsService)
        {
            this.reviewsService = reviewsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string appointmentId)
        {
            var model = await this.reviewsService.GetReviewAsync<ReviewViewModel>(appointmentId);

            return this.View(model);
        }
    }
}
