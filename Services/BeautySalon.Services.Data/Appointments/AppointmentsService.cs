namespace BeautySalon.Services.Data.Appointments
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Models.Enums;
    using BeautySalon.Services.Data.Cards;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class AppointmentsService : IAppointmentsService
    {
        private readonly IRepository<Appointment> appointmentsRepository;
        private readonly IUsersService usersService;
        private readonly ICardsService cardsService;
        private readonly IProceduresService proceduresService;
        private readonly ICategoriesService categoriesService;

        public AppointmentsService(
            IRepository<Appointment> appointmentsRepository,
            IUsersService usersService,
            ICardsService cardsService,
            IProceduresService proceduresService,
            ICategoriesService categoriesService)
        {
            this.appointmentsRepository = appointmentsRepository;
            this.usersService = usersService;
            this.cardsService = cardsService;
            this.proceduresService = proceduresService;
            this.categoriesService = categoriesService;
        }

        public async Task<string> CreateAsync(string userId, string stylistId, string procedureId, DateTime date, string time, string comment)
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

            return appointment.Id;
        }

        public async Task CancelAsync(string id)
        {
            var appointment = await this.GetByIdAsync(id);

            appointment.Status = Status.CancelledByStylist;

            this.appointmentsRepository.Update(appointment);
            await this.appointmentsRepository.SaveChangesAsync();
        }

        public async Task DoneAsync(string id)
        {
            var appointment = await this.GetByIdAsync(id);

            appointment.Status = Status.Done;

            await this.CheckUsersSubscriptionCardAsync(appointment);

            this.appointmentsRepository.Update(appointment);
            await this.appointmentsRepository.SaveChangesAsync();
        }

        public async Task ApproveAsync(string id)
        {
            var appointment = await this.GetByIdAsync(id);

            appointment.Status = Status.Approved;

            this.appointmentsRepository.Update(appointment);
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
            var appointments = await this.appointmentsRepository
                .All()
                .Where(a => a.StylistId == stylistId && (a.Status == Status.Approved || a.Status == Status.Done))
                .To<T>()
                .ToListAsync();

            return appointments;
        }

        public async Task<IEnumerable<T>> GetClientsUpcomingAppointmentsAsync<T>(string userId)
        {
            var appointments = await this.appointmentsRepository
               .All()
               .Where(a => a.ClientId == userId && (a.Status == Status.Approved || a.Status == Status.Processing))
               .OrderBy(a => a.DateTime)
               .ThenBy(a => a.StartTime)
               .To<T>()
               .ToListAsync();

            return appointments;
        }

        public async Task<T> GetDetailsAsync<T>(string id)
        {
            var appointment = await this.appointmentsRepository
                .All()
                .Where(a => a.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return appointment;
        }

        public async Task<IEnumerable<string>> GetFreeHoursAsync(string selectedDate, string selectedStylistId)
        {
            var selectedDateAsDateTime = DateTime.ParseExact(selectedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var takenHours = await this.appointmentsRepository
                .All()
                .Where(a => a.DateTime.Date == selectedDateAsDateTime.Date
                    && a.StylistId == selectedStylistId
                    && a.Status != Status.CancelledByStylist)
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

        public async Task<IEnumerable<T>> GetRequestsAsync<T>(string stylistId)
        {
            var appoitments = await this.appointmentsRepository
                .All()
                .Where(a => a.Status == Status.Processing && a.StylistId == stylistId)
                .OrderBy(a => a.CreatedOn)
                .To<T>()
                .ToListAsync();

            return appoitments;
        }

        public async Task<IEnumerable<T>> GetHistoryStylistAsync<T>(string stylistId)
        {
            var appointments = await this.appointmentsRepository
                .All()
                .Where(a => a.StylistId == stylistId
                   && (a.Status == Status.Done || a.Status == Status.CancelledByStylist))
                .OrderByDescending(a => a.DateTime)
                .ThenBy(a => a.StartTime)
                .To<T>()
                .ToListAsync();

            return appointments;
        }

        public async Task<IEnumerable<T>> GetHistoryAllStylistsAsync<T>()
        {
            var appointments = await this.appointmentsRepository
                .All()
                .Where(a => a.Status == Status.Done || a.Status == Status.CancelledByStylist)
                .OrderByDescending(a => a.DateTime)
                .ThenBy(a => a.StartTime)
                .To<T>()
                .ToListAsync();

            return appointments;
        }

        public async Task<IEnumerable<T>> GetHistoryUserAsync<T>(string userId)
        {
            var appointments = await this.appointmentsRepository
               .All()
               .Where(a => a.ClientId == userId
                && ((a.Status == Status.Done && a.IsReview) || (a.Status == Status.CancelledByStylist && !a.IsReview)))
               .OrderByDescending(a => a.DateTime)
               .ThenBy(a => a.StartTime)
               .To<T>()
               .ToListAsync();

            return appointments;
        }

        public async Task<IEnumerable<T>> GetAppointmentsToReviewAsync<T>(string userId)
        {
            var appointments = await this.appointmentsRepository
               .All()
               .Where(a => a.ClientId == userId
                  && a.Status == Status.Done
                  && a.DateTime.Date < DateTime.UtcNow.Date
                  && !a.IsReview)
               .OrderByDescending(a => a.DateTime)
               .ThenBy(a => a.StartTime)
               .To<T>()
               .ToListAsync();

            return appointments;
        }

        public async Task<int> GetAppointmentsForTodayCountAsync(string stylistId)
        {
            var appointmentsCount = await this.appointmentsRepository
                .All()
                .Where(a => a.StylistId == stylistId
                    && a.Status == Status.Approved
                    && a.DateTime.Date == DateTime.UtcNow.Date)
                .CountAsync();

            return appointmentsCount;
        }

        public async Task<int> GetAppointmentsRequestsCountAsync(string stylistId)
        {
            var appointmentsCount = await this.appointmentsRepository
               .All()
               .Where(a => a.StylistId == stylistId
                   && a.Status == Status.Processing)
               .CountAsync();

            return appointmentsCount;
        }

        public async Task<bool> CheckPastProceduresAsync(string userId)
        {
            var hasToReview = await this.appointmentsRepository
                .All()
                .AnyAsync(a => a.ClientId == userId
                 && a.DateTime.Date < DateTime.UtcNow.Date
                 && a.Status == Status.Done
                 && a.IsReview == false);

            return hasToReview;
        }

        public async Task<T> GetByIdAsync<T>(string appointmentId)
        {
            var appointment = await this.appointmentsRepository
                .All()
                .Where(a => a.Id == appointmentId)
                .To<T>()
                .FirstOrDefaultAsync();

            return appointment;
        }

        private async Task<Appointment> GetByIdAsync(string id)
        {
            return await this.appointmentsRepository
                .All()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        private async Task CheckUsersSubscriptionCardAsync(Appointment appointment)
        {
            var hasSubcriptionCard = await this.usersService.HasSubscriptionCardAsync(appointment.ClientId);

            var procedure = await this.proceduresService.GetByIdAsync(appointment.ProcedureId);
            var category = await this.categoriesService.GetByIdAsync(procedure.CategoryId);

            var hasToChangeCounterSubcriptionCard = category.Name == GlobalConstants.CategorySkinName
                || category.Name == GlobalConstants.CategoryNailsName
                || procedure.Name == GlobalConstants.ProcedureHairCutName;

            if (hasSubcriptionCard && hasToChangeCounterSubcriptionCard)
            {
                await this.cardsService.ChangeCardCounterAsync(appointment.ClientId, appointment.Procedure.Price);
            }
        }
    }
}
