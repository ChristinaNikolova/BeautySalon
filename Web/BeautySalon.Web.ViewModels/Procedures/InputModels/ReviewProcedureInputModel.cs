namespace BeautySalon.Web.ViewModels.Procedures.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using BeautySalon.Common;
    using BeautySalon.Web.ViewModels.Appointments.ViewModels;

    public class ReviewProcedureInputModel
    {
        //TODO Remove unnesessery
        //TODO Validate model attr
        public AppointmentViewModel Appointment { get; set; }

        public string ProcedureId { get; set; }

        public string ClientId { get; set; }

        [Required]
        [StringLength(DataValidation.ProcedureReviewContentMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.ProcedureReviewContentMinLenght)]
        [Display(Name = "Review")]
        public string Content { get; set; }

        [Range(typeof(double), DataValidation.ProcedureReviewMinPoints, DataValidation.ProcedureReviewMaxPoints)]
        public int Points { get; set; }
    }
}
