namespace BeautySalon.Web.ViewModels.ProcedureReviews.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ProcedureReviewViewModel : IMapFrom<ProcedureReview>
    {
        public string UserUsername { get; set; }

        public string Content { get; set; }

        public string UserPicture { get; set; }
    }
}
