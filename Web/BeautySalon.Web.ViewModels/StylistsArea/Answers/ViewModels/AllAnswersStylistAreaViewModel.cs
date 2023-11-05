namespace BeautySalon.Web.ViewModels.StylistsArea.Answers.ViewModels
{
    using System.Collections.Generic;

    public class AllAnswersStylistAreaViewModel
    {
        public IEnumerable<AnswerStylistAreaViewModel> AnsweredQuestions { get; set; }

        public string StylistId { get; set; }
    }
}
