namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Models.Enums;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Appointments;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class AppointmentsServiceTests : BaseServiceTests
    {
        private readonly ApplicationUser stylist;
        private readonly ApplicationUser client;
        private readonly Procedure procedure;

        public AppointmentsServiceTests()
        {
            this.stylist = new ApplicationUser() { Id = "10" };
            this.client = new ApplicationUser() { Id = "1" };
            this.procedure = new Procedure() { Id = "1" };
        }

        [Fact]
        public async Task CheckCreatingAppointment()
        {
            var service = this.PrepareService();

            string appointmentId = await this.GetAppointmentIdAsync(service);

            Assert.NotNull(appointmentId);
        }

        [Fact]
        public async Task CheckGettingDetailsAppointment()
        {
            var service = this.PrepareService();

            string appointmentId = await this.GetAppointmentIdAsync(service);

            var resultAppointment = await service.GetDetailsAsync<TestAppointmentModel>(appointmentId);
            var expectedAppointment = await service.GetByIdAsync<TestAppointmentModel>(appointmentId);

            Assert.True(resultAppointment.Id.Equals(expectedAppointment.Id));
        }

        [Fact]
        public async Task CheckCancelingAppointment()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            string appointmentId = await this.GetAppointmentIdAsync(service);

            await service.CancelAsync(appointmentId);

            var appointment = await GetAppointmentAsync(repository, appointmentId);

            Assert.Equal(Status.CancelledByStylist, appointment.Status);
        }

        [Fact]
        public async Task CheckDoingAppointment()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            string appointmentId = await this.GetAppointmentIdAsync(service);

            await service.DoneAsync(appointmentId);

            var appointment = await GetAppointmentAsync(repository, appointmentId);

            Assert.Equal(Status.Done, appointment.Status);
        }

        [Fact]
        public async Task CheckApprovingAppointment()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            string appointmentId = await this.GetAppointmentIdAsync(service);

            await service.ApproveAsync(appointmentId);

            var appointment = await GetAppointmentAsync(repository, appointmentId);

            Assert.Equal(Status.Approved, appointment.Status);
        }

        [Fact]
        public async Task CheckGettingAppointmentsForToday()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            await this.PrepareAppointmentsAsync(service);

            var appointments = await service.GetAllAppointmentsForTodayAsync<TestAppointmentModel>();
            var appointmentsExpected = await repository
                .All()
                .Where(a => a.DateTime == DateTime.UtcNow.Date)
                .To<TestAppointmentModel>()
                .ToListAsync();

            Assert.Equal(3, appointments.Count());
            appointments.SequenceEqual(appointmentsExpected);
        }

        [Fact]
        public async Task CheckGettingAllAppointmentsForCurrentStylistAndAppointmentsForToday()
        {
            var service = this.PrepareService();

            var firstAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");
            var secondAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "12:00", "test comment");
            var thirdAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "13:00", "test comment");

            await service.ApproveAsync(firstAppId);
            await service.ApproveAsync(secondAppId);
            await service.DoneAsync(thirdAppId);

            var stylistAppointments = await
                service.GetAllForStylistAsync<TestAppointmentModel>(this.stylist.Id);
            var appointmentsCount = await
               service.GetAppointmentsForTodayCountAsync(this.stylist.Id);

            Assert.Equal(3, stylistAppointments.Count());
            Assert.Equal(2, appointmentsCount);
        }

        [Fact]
        public async Task CheckGettingUpcommingAppointmentsForCurrentClient()
        {
            var service = this.PrepareService();

            var firstAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");
            var secondAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "12:00", "test comment");
            await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "13:00", "test comment");

            await service.ApproveAsync(firstAppId);
            await service.ApproveAsync(secondAppId);

            var userAppointments = await
                service.GetClientsUpcomingAppointmentsAsync<TestAppointmentModel>(this.client.Id);

            Assert.Equal(3, userAppointments.Count());
        }

        [Fact]
        public async Task CheckGettingStylistFreeHours()
        {
            var service = this.PrepareService();

            await this.PrepareAppointmentsAsync(service);

            var selectedDate = DateTime.UtcNow.ToString("dd'/'MM'/'yyyy");
            var result = await
                service.GetFreeHoursAsync(selectedDate, this.stylist.Id);

            var expected = new string[]
            {
                "10:00",
                "14:00",
                "15:00",
                "16:00",
                "17:00",
                "18:00",
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task CheckGettingAppointmentsRequestsForCurrentStylist()
        {
            var service = this.PrepareService();

            await this.PrepareAppointmentsAsync(service);

            var appointments = await
                service.GetRequestsAsync<TestAppointmentModel>(this.stylist.Id);
            var count = await
                service.GetAppointmentsRequestsCountAsync(this.stylist.Id);

            Assert.Equal(3, appointments.Count());
            Assert.Equal(3, count);
        }

        [Fact]
        public async Task CheckGettingAppointmentsHistoryForCurrentStylist()
        {
            var service = this.PrepareService();

            await this.PrepareAppointmentsAndStatusAsync(service);

            var resultAppointments = await
                service.GetHistoryStylistAsync<TestAppointmentModel>(this.stylist.Id);

            Assert.Equal(3, resultAppointments.Count());
        }

        [Fact]
        public async Task CheckGettingAppointmentsHistoryAllStylists()
        {
            var service = this.PrepareService();

            await this.PrepareAppointmentsAndStatusAsync(service);

            var resultAppointments = await
                service.GetHistoryAllStylistsAsync<TestAppointmentModel>();

            Assert.Equal(3, resultAppointments.Count());
        }

        [Fact]
        public async Task CheckGettingAppointmentsHistoryUser()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            string appId = await this.GetAppointmentIdAsync(service);

            await service.DoneAsync(appId);

            var firstApp = await GetAppointmentAsync(repository, appId);
            firstApp.IsReview = true;

            repository.Update(firstApp);
            await repository.SaveChangesAsync();

            var resultAppointments = await
                service.GetHistoryUserAsync<TestAppointmentModel>(this.client.Id);

            Assert.Single(resultAppointments);
        }

        [Fact]
        public async Task CheckingAppointmentsForReview()
        {
            AppointmentsService service = await this.PrepareAppointmentReviewAsync();

            var hasToReview = await
                service.CheckPastProceduresAsync(this.client.Id);
            var resultAppointments = await
                service.GetAppointmentsToReviewAsync<TestAppointmentModel>(this.client.Id);

            Assert.True(hasToReview);
            Assert.Single(resultAppointments);
        }

        private static async Task<Appointment> GetAppointmentAsync(EfDeletableEntityRepository<Appointment> repository, string firstAppId)
        {
            return await repository.All().FirstOrDefaultAsync(a => a.Id == firstAppId);
        }

        private async Task<AppointmentsService> PrepareAppointmentReviewAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            var pastDate = DateTime.ParseExact("12/10/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            string firstAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, pastDate, "11:00", "test comment");
            string secondAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, pastDate, "12:00", "test comment");
            string thirdAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "13:00", "test comment");

            await service.DoneAsync(firstAppId);
            await service.DoneAsync(secondAppId);
            await service.DoneAsync(thirdAppId);

            var firstApp = await GetAppointmentAsync(repository, firstAppId);
            firstApp.IsReview = true;

            repository.Update(firstApp);
            await repository.SaveChangesAsync();
            return service;
        }

        private async Task PrepareAppointmentsAndStatusAsync(AppointmentsService service)
        {
            string firstAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");
            string secondAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "12:00", "test comment");
            string thirdAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "13:00", "test comment");

            await service.DoneAsync(firstAppId);
            await service.DoneAsync(secondAppId);
            await service.CancelAsync(thirdAppId);
        }

        private async Task PrepareAppointmentsAsync(AppointmentsService service)
        {
            await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");
            await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "12:00", "test comment");
            await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "13:00", "test comment");
        }

        private async Task<string> GetAppointmentIdAsync(AppointmentsService service)
        {
            return await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");
        }

        private AppointmentsService PrepareService()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);
            return service;
        }

        public class TestAppointmentModel : IMapFrom<Appointment>
        {
            public string Id { get; set; }
        }
    }
}
