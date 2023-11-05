namespace BeautySalon.Web.ViewModels.Answers.ViewModels
{
    using System.Collections.Generic;

    public class AllAnswersForUserViewModel
    {
        public IEnumerable<AnswerForUserViewModel> Answers { get; set; }
    }
}
