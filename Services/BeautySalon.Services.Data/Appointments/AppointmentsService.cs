namespace BeautySalon.Services.Data.Appointments
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Models.Enums;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class AppointmentsService : IAppointmentsService
    {
        private readonly IRepository<Appointment> appointmentsRepository;

        public AppointmentsService(IRepository<Appointment> appointmentsRepository)
        {
            this.appointmentsRepository = appointmentsRepository;
        }

        public async Task CreateAsync(string userId, string stylistId, string procedureId, DateTime date, string time, string comment)
        {
            var appointment = new Appointment()
            {
                ClientId = userId,
                StylistId = stylistId,
                ProcedureId = procedureId,
                DateTime = date,
                StartTime = time,
                Comment = comment,
                Status = Status.Processing,
                CreatedOn = DateTime.UtcNow,
            };

            await this.appointmentsRepository.AddAsync(appointment);
            await this.appointmentsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAppointmentsForTodayAsync<T>()
        {
            var currentDate = DateTime.UtcNow.Date;

            var appointments = await this.appointmentsRepository
                .AllAsNoTracking()
                .Where(a => a.DateTime.Date == currentDate)
                .OrderBy(a => a.StartTime)
                .To<T>()
                .ToListAsync();

            return appointments;
        }

        public async Task<IEnumerable<T>> GetAllForStylistAsync<T>(string stylistId)
        {
            //TODO STATUS!!!!
            var appointments = await this.appointmentsRepository
                .All()
                .Where(a => a.StylistId == stylistId)
                .To<T>()
                .ToListAsync();

            return appointments;
        }

        public async Task<IEnumerable<string>> GetFreeHoursAsync(string selectedDate, string selectedStylistId)
        {
            var selectedDateAsDateTime = DateTime.ParseExact(selectedDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            var takenHours = await this.appointmentsRepository
                .All()
                .Where(a => a.DateTime == selectedDateAsDateTime && a.StylistId == selectedStylistId)
                .Select(a => a.StartTime)
                .ToListAsync();

            var allHours = new string[]
            {
                "10:00",
                "11:00",
                "12:00",
                "13:00",
                "14:00",
                "15:00",
                "16:00",
                "17:00",
                "18:00",
            };

            var freeHours = allHours
                .Where(a => !takenHours.Contains(a))
                .ToList();

            return freeHours;
        }
    }
}
