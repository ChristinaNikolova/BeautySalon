namespace BeautySalon.Web.ViewModels.Procedures.ViewModels
{
    using System.Collections.Generic;

    public class AllProceduresByCategoryViewModel
    {
        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<ProcedureViewModel> Procedures { get; set; }
    }
}
