namespace BeautySalon.Web.Controllers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Comments;
    using BeautySalon.Web.ViewModels.Comments.InputModels;
    using BeautySalon.Web.ViewModels.Comments.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CommentsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICommentsService commentsService;

        public CommentsController(UserManager<ApplicationUser> userManager, ICommentsService commentsService)
        {
            this.userManager = userManager;
            this.commentsService = commentsService;
        }

        [HttpPost]
        public async Task<ActionResult<AllCommentsViewModel>> Create([FromBody] CreateCommentInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return new AllCommentsViewModel { Comments = null, ArticleId = input.ArticleId };
            }

            var userId = this.userManager.GetUserId(this.User);

            await this.commentsService.CreateAsync(input.Content, input.ArticleId, userId);

            var comments = await this.commentsService.GetAllAsync<CommentViewModel>(input.ArticleId);

            return new AllCommentsViewModel { Comments = comments, ArticleId = input.ArticleId };
        }
    }
}
