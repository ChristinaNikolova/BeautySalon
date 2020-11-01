namespace BeautySalon.Services.Data.Appointments
{
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;

    public class AppointmentsService : IAppointmentsService
    {
        private readonly IRepository<Appointment> appointmentsRepository;

        public AppointmentsService(IRepository<Appointment> appointmentsRepository)
        {
            this.appointmentsRepository = appointmentsRepository;
        }
    }
}
