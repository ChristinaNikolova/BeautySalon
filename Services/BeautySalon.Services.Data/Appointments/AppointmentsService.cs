namespace BeautySalon.Services.Data.Appointments
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class AppointmentsService : IAppointmentsService
    {
        private readonly IRepository<Appointment> appointmentsRepository;

        public AppointmentsService(IRepository<Appointment> appointmentsRepository)
        {
            this.appointmentsRepository = appointmentsRepository;
        }

        public async Task<IEnumerable<string>> GetFreeTimesAsync(string selectedDate, string selectedStylistId)
        {
            var selectedDateAsDateTime = DateTime.ParseExact(selectedDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            var takenApp = await this.appointmentsRepository
                .All()
                .Where(a => a.DateTime == selectedDateAsDateTime && a.StylistId == selectedStylistId)
                .Select(a => a.StartTime)
                .ToListAsync();

            var allApp = new string[]
            {
                "9:00",
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

            var freeApp = allApp
                .Where(a => !takenApp.Contains(a))
                .ToList();

            return freeApp;
        }
    }
}
