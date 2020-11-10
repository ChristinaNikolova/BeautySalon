namespace BeautySalon.Web.ViewModels.Administration.Comments.ViewModels
{
    using System.Collections.Generic;

    public class AllCommentsAdminstrationViewModel
    {
        public IEnumerable<CommentAdminstrationViewModel> Comments { get; set; }
    }
}
