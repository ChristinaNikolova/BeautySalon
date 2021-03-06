﻿namespace BeautySalon.Services.Data.Appointments
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAppointmentsService
    {
        Task<IEnumerable<string>> GetFreeHoursAsync(string selectedDate, string selectedStylistId);

        Task<string> CreateAsync(string userId, string stylistId, string procedureId, DateTime date, string time, string comment);

        Task<IEnumerable<T>> GetAllAppointmentsForTodayAsync<T>();

        Task<IEnumerable<T>> GetAllForStylistAsync<T>(string stylistId);

        Task<T> GetDetailsAsync<T>(string id);

        Task CancelAsync(string id);

        Task DoneAsync(string id);

        Task ApproveAsync(string id);

        Task<IEnumerable<T>> GetRequestsAsync<T>(string stylistId);

        Task<IEnumerable<T>> GetHistoryStylistAsync<T>(string stylistId);

        Task<IEnumerable<T>> GetHistoryAllStylistsAsync<T>();

        Task<int> GetAppointmentsForTodayCountAsync(string stylistId);

        Task<int> GetAppointmentsRequestsCountAsync(string stylistId);

        Task<IEnumerable<T>> GetClientsUpcomingAppointmentsAsync<T>(string userId);

        Task<bool> CheckPastProceduresAsync(string userId);

        Task<IEnumerable<T>> GetHistoryUserAsync<T>(string userId);

        Task<IEnumerable<T>> GetAppointmentsToReviewAsync<T>(string userId);

        Task<T> GetByIdAsync<T>(string appointmentId);
    }
}
