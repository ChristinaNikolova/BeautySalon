namespace BeautySalon.Web.ViewModels.StylistsArea.Questions.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class SeeQuestionViewModel : QuestionForStylistViewModel, IMapFrom<Question>
    {
        public string Content { get; set; }

        public string StylistId { get; set; }
    }
}
