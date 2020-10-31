namespace BeautySalon.Web.ViewModels.Comments.ViewModels
{
    using System.Collections.Generic;

    public class AllCommentsViewModel
    {
        public string ArticleId { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
