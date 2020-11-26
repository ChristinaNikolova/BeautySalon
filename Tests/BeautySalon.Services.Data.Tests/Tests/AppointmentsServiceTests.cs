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

    public class AppointmentsServiceTests
    {
        private readonly ApplicationUser stylist;
        private readonly ApplicationUser client;
        private readonly Procedure procedure;

        public AppointmentsServiceTests()
        {
            new MapperInitializationProfile();
            this.stylist = new ApplicationUser()
            {
                Id = "1",
            };
            this.client = new ApplicationUser()
            {
                Id = "1",
            };
            this.procedure = new Procedure()
            {
                Id = "1",
            };
        }

        [Fact]
        public async Task CheckCreatingAppointment()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            var appointmentId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");

            Assert.NotNull(appointmentId);
        }

        [Fact]
        public async Task CheckGettingDetailsAppointment()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            var appointmentId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");

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

            var appointmentId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");

            await service.CancelAsync(appointmentId);

            var appointment = await repository.All().FirstOrDefaultAsync(a => a.Id == appointmentId);

            Assert.Equal(Status.CancelledByStylist, appointment.Status);
        }

        [Fact]
        public async Task CheckDoingAppointment()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            var appointmentId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");

            await service.DoneAsync(appointmentId);

            var appointment = await repository.All().FirstOrDefaultAsync(a => a.Id == appointmentId);

            Assert.Equal(Status.Done, appointment.Status);
        }

        [Fact]
        public async Task CheckApprovingAppointment()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            var appointmentId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");

            await service.ApproveAsync(appointmentId);

            var appointment = await repository.All().FirstOrDefaultAsync(a => a.Id == appointmentId);

            Assert.Equal(Status.Approved, appointment.Status);
        }

        [Fact]
        public async Task CheckGettingAppointmentsForToday()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");
            await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "12:00", "test comment");
            await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "13:00", "test comment");

            var appointments = await service.GetAllAppointmentsForTodayAsync<TestAppointmentModel>();

            Assert.Equal(3, appointments.Count());
        }

        [Fact]
        public async Task CheckGettingAppointmentsCountForTodayCurrentStylist()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            string firstAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");
            string secondAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "12:00", "test comment");
            string thirdAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "13:00", "test comment");

            await service.ApproveAsync(firstAppId);
            await service.ApproveAsync(secondAppId);

            var appointmentsCount = await
                service.GetAppointmentsForTodayCountAsync(this.stylist.Id);

            Assert.Equal(2, appointmentsCount);
        }

        [Fact]
        public async Task CheckGettingAppointmentsForCurrentStylist()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            var firstAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");
            var secondAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "12:00", "test comment");
            var thirdAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "13:00", "test comment");

            await service.ApproveAsync(firstAppId);
            await service.ApproveAsync(secondAppId);
            await service.DoneAsync(thirdAppId);

            var stylistAppointments = await
                service.GetAllForStylistAsync<TestAppointmentModel>(this.stylist.Id);

            Assert.Equal(3, stylistAppointments.Count());
        }

        [Fact]
        public async Task CheckGettingUpcommingAppointmentsForCurrentClient()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

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
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");
            await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "12:00", "test comment");
            await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "13:00", "test comment");

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
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");
            await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "12:00", "test comment");
            await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "13:00", "test comment");

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
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            string firstAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");
            string secondAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "12:00", "test comment");
            string thirdAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "13:00", "test comment");

            await service.DoneAsync(firstAppId);
            await service.DoneAsync(secondAppId);
            await service.CancelAsync(thirdAppId);

            var resultAppointments = await
                service.GetHistoryStylistAsync<TestAppointmentModel>(this.stylist.Id);

            Assert.Equal(3, resultAppointments.Count());
        }

        [Fact]
        public async Task CheckGettingAppointmentsHistoryAllStylists()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(repository);

            var secondStylist = new ApplicationUser() { Id = "2" };

            string firstAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");
            string secondAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "12:00", "test comment");
            string thirdAppId = await service.CreateAsync(this.client.Id, secondStylist.Id, this.procedure.Id, DateTime.UtcNow, "13:00", "test comment");

            await service.DoneAsync(firstAppId);
            await service.DoneAsync(secondAppId);
            await service.CancelAsync(thirdAppId);

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

            string firstAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");
            string secondAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "12:00", "test comment");
            string thirdAppId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "13:00", "test comment");

            await service.DoneAsync(firstAppId);
            await service.DoneAsync(secondAppId);
            await service.CancelAsync(thirdAppId);

            var firstApp = await repository.All().FirstOrDefaultAsync(a => a.Id == firstAppId);
            firstApp.IsReview = true;

            repository.Update(firstApp);
            await repository.SaveChangesAsync();

            var resultAppointments = await
                service.GetHistoryUserAsync<TestAppointmentModel>(this.client.Id);

            Assert.Equal(2, resultAppointments.Count());
        }

        [Fact]
        public async Task CheckGettingAppointmentsForReview()
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

            var firstApp = await repository.All().FirstOrDefaultAsync(a => a.Id == firstAppId);
            firstApp.IsReview = true;

            repository.Update(firstApp);
            await repository.SaveChangesAsync();

            var resultAppointments = await
                service.GetAppointmentsToReviewAsync<TestAppointmentModel>(this.client.Id);

            Assert.Single(resultAppointments);
        }

        [Fact]
        public async Task CheckingAppointmentsForReview()
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

            var firstApp = await repository.All().FirstOrDefaultAsync(a => a.Id == firstAppId);
            firstApp.IsReview = true;

            repository.Update(firstApp);
            await repository.SaveChangesAsync();

            var hasToReview = await
                service.CheckPastProceduresAsync(this.client.Id);

            Assert.True(hasToReview);
        }

        private static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            return db;
        }

        public class TestAppointmentModel : IMapFrom<Appointment>
        {
            public string Id { get; set; }
        }
    }
}
