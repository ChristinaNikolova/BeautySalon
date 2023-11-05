namespace BeautySalon.Web.ViewModels.Answers.ViewModels
{
    using System.Collections.Generic;

    public class AllNewAnswersForUserViewModel
    {
        public IEnumerable<NewAnswerForUserViewModel> Answers { get; set; }
    }
}
