namespace BeautySalon.Web.ViewModels.Comments.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;

    public class CreateCommentInputModel
    {
        public string ArticleId { get; set; }

        [Required]
        [StringLength(DataValidation.CommentContentMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.CommentContentMinLenght)]
        public string Content { get; set; }
    }
}
