namespace BeautySalon.Services.HangFire.DeleteAppointments
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Models.Enums;
    using Microsoft.EntityFrameworkCore;

    public class DeleteAppointments : IDeleteAppointments
    {
        private readonly IDeletableEntityRepository<Appointment> appointmentsRepository;

        public DeleteAppointments(IDeletableEntityRepository<Appointment> appointmentsRepository)
        {
            this.appointmentsRepository = appointmentsRepository;
        }

        public async Task DeleteCancelledAppointmentsAsync()
        {
            var appointmentsToDelete = await this.appointmentsRepository
               .All()
               .Where(m => m.DateTime.AddDays(GlobalConstants.DaysOneMonth).Date <= DateTime.Today.Date
                   && m.Status == Status.Cancelled)
               .ToListAsync();

            foreach (var appointment in appointmentsToDelete)
            {
                this.appointmentsRepository.HardDelete(appointment);
            }

            await this.appointmentsRepository.SaveChangesAsync();
        }

        public async Task DeletePastAppointmentsAsync()
        {
            var appointmentsToDelete = await this.appointmentsRepository
               .All()
               .Where(m => m.DateTime.AddDays(GlobalConstants.DaysOneYear).Date <= DateTime.Today.Date
                   && m.Status == Status.Done)
               .ToListAsync();

            foreach (var appointment in appointmentsToDelete)
            {
                appointment.IsDeleted = true;
                this.appointmentsRepository.Update(appointment);
            }

            await this.appointmentsRepository.SaveChangesAsync();
        }
    }
}
