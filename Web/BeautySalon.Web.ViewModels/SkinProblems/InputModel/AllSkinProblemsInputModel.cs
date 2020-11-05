namespace BeautySalon.Web.ViewModels.SkinProblems.InputModel
{
    using System.Collections.Generic;

    using BeautySalon.Web.ViewModels.SkinProblems.ViewModels;

    public class AllSkinProblemsInputModel
    {
        public IEnumerable<SkinProblemViewModel> SkinProblems { get; set; }
    }
}
