namespace BeautySalon.Web.ViewModels.Answers.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class NewAnswerForUserViewModel : AnswerForUserViewModel, IMapFrom<Answer>
    {
        public string StylistPicture { get; set; }
    }
}
