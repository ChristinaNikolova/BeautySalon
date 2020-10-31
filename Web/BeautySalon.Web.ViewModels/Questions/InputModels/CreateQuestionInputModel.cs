namespace BeautySalon.Web.ViewModels.Questions.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Web.ViewModels.Stylists.ViewModels;

    public class CreateQuestionInputModel
    {
        [Required]
        [StringLength(DataValidation.QuestionTitleMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.QuestionTitleMinLenght)]
        public string Title { get; set; }

        [Required]
        [StringLength(DataValidation.QuestionContentMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.QuestionContentMinLenght)]
        public string Content { get; set; }

        public string StylistId { get; set; }

        public StylistNamesViewModel Stylist { get; set; }
    }
}
