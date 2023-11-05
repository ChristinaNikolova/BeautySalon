namespace BeautySalon.Web.ViewModels.Administration.Procedures.ViewModels
{
    using System.Collections.Generic;

    using BeautySalon.Web.ViewModels.Procedures.ViewModels;

    public class AllProceduresAdministrationViewModel
    {
        public IEnumerable<ProcedureViewModel> Procedures { get; set; }
    }
}
