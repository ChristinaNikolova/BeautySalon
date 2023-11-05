namespace BeautySalon.Web.ViewModels.StylistsArea.Answers.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Web.ViewModels.StylistsArea.Questions.ViewModels;

    public class CreateAnswerInputModel
    {
        public SeeQuestionViewModel Question { get; set; }

        [Required]
        [StringLength(DataValidation.AnswerTitleMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.AnswerTitleMinLenght)]
        public string Title { get; set; }

        [Required]
        [StringLength(DataValidation.AnswerContentMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.AnswerContentMinLenght)]
        public string Content { get; set; }
    }
}
