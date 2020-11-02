namespace BeautySalon.Services.Data.Appointments
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAppointmentsService
    {
        Task<IEnumerable<string>> GetFreeTimesAsync(string selectedDate, string selectedStylistId);
    }
}
