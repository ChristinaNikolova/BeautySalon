namespace BeautySalon.Web.ViewModels.Procedures.ViewModels
{
    using System.Collections.Generic;

    public class SearchProcedureCriteriaViewModelModel
    {
        public string CategoryId { get; set; }

        public IEnumerable<ProcedureViewModel> Procedures { get; set; }
    }
}
