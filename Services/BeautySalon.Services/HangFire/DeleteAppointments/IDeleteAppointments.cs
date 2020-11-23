namespace BeautySalon.Services.HangFire.DeleteAppointments
{
    using System.Threading.Tasks;

    public interface IDeleteAppointments
    {
        Task DeletePastAppointmentsAsync();

        Task DeleteCancelledAppointmentsAsync();
    }
}
