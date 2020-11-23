namespace BeautySalon.Web.ViewModels.Procedures.ViewModels
{
    using System.Collections.Generic;

    public class AllProceduresByCategoryViewModel
    {
        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<ProcedureViewModel> Procedures { get; set; }

        public int CurrentPage { get; set; }

        public int NextPage
        => this.CurrentPage + 1;

        public int PreviousPage
            => this.CurrentPage - 1;

        public int PagesCount { get; set; }
    }
}
