namespace BeautySalon.Services.Data.Appointments
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAppointmentsService
    {
        Task<IEnumerable<string>> GetFreeHoursAsync(string selectedDate, string selectedStylistId);

        Task CreateAsync(string userId, string stylistId, string procedureId, DateTime date, string time, string comment);
    }
}
