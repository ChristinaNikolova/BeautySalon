namespace BeautySalon.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.Comments;
    using BeautySalon.Web.ViewModels.Administration.Comments.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class CommentsController : AdministrationController
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        public async Task<IActionResult> GetAllFromPreviousDay()
        {
            var model = new AllCommentsAdminstrationViewModel()
            {
                Comments = await
                this.commentsService.GetAllFromPreviousDayAsync<CommentAdminstrationViewModel>(),
            };

            return this.View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.commentsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.GetAllFromPreviousDay));
        }
    }
}
