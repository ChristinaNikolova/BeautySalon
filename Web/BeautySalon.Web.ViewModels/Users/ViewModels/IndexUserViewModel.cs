namespace BeautySalon.Web.ViewModels.Users.ViewModels
{
    using System.Collections.Generic;

    using BeautySalon.Web.ViewModels.Appointments.ViewModels;

    public class IndexUserViewModel
    {
        public IEnumerable<BaseAppoitmentViewModel> Appoitments { get; set; }

        public bool IsNewAnswer { get; set; }

        public bool HasToReview { get; set; }
    }
}
