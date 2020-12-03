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
    using BeautySalon.Services.Data.Cards;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Services.Data.Users;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class AppointmentsServiceTests : BaseServiceTests
    {
        private readonly ApplicationUser stylist;
        private readonly ApplicationUser client;
        private readonly Procedure procedure;
        private readonly Category category;

        private readonly Mock<IUsersService> usersService;
        private readonly Mock<ICardsService> cardsService;
        private readonly Mock<IProceduresService> proceduresService;
        private readonly Mock<ICategoriesService> categoriesService;

        public AppointmentsServiceTests()
        {
            this.usersService = new Mock<IUsersService>();
            this.cardsService = new Mock<ICardsService>();
            this.proceduresService = new Mock<IProceduresService>();
            this.categoriesService = new Mock<ICategoriesService>();

            this.stylist = new ApplicationUser() { Id = "10" };
            this.client = new ApplicationUser() { Id = "1" };
            this.category = new Category() { Id = "1" };
            this.procedure = new Procedure() { Id = "1", Name = "Procedure Name", CategoryId = this.category.Id };
        }

        [Fact]
        public async Task CheckCreatingAppointmentAsync()
        {
            var service = this.PrepareService();

            string appointmentId = await this.GetAppointmentIdAsync(service);

            Assert.NotNull(appointmentId);
        }

        [Fact]
        public async Task CheckGettingDetailsAppointmentAsync()
        {
            var service = this.PrepareService();

            string appointmentId = await this.GetAppointmentIdAsync(service);

            var resultAppointment = await service.GetDetailsAsync<TestAppointmentModel>(appointmentId);
            var expectedAppointment = await service.GetByIdAsync<TestAppointmentModel>(appointmentId);

            Assert.True(resultAppointment.Id.Equals(expectedAppointment.Id));
        }

        [Fact]
        public async Task CheckCancelingAppointmentAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(
                repository,
                this.usersService.Object,
                this.cardsService.Object,
                this.proceduresService.Object,
                this.categoriesService.Object);

            string appointmentId = await this.GetAppointmentIdAsync(service);

            await service.CancelAsync(appointmentId);

            var appointment = await GetAppointmentAsync(repository, appointmentId);

            Assert.Equal(Status.CancelledByStylist, appointment.Status);
        }

        [Fact]
        public async Task CheckDoingAppointmentAsync()
        {
            ApplicationDbContext db = GetDb();

            var service = PreperaAppointmentServiceWithAllDependencies(db);

            var appointment = new Appointment()
            {
                Id = "1",
                ProcedureId = this.procedure.Id,
                ClientId = this.client.Id,
            };

            await db.Procedures.AddAsync(this.procedure);
            await db.Categories.AddAsync(this.category);
            await db.Appointments.AddAsync(appointment);
            await db.SaveChangesAsync();

            await service.DoneAsync(appointment.Id);

            Assert.Equal(Status.Done, appointment.Status);
        }

        [Fact]
        public async Task CheckApprovingAppointmentAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(
                 repository,
                 this.usersService.Object,
                 this.cardsService.Object,
                 this.proceduresService.Object,
                 this.categoriesService.Object);

            string appointmentId = await this.GetAppointmentIdAsync(service);

            await service.ApproveAsync(appointmentId);

            var appointment = await GetAppointmentAsync(repository, appointmentId);

            Assert.Equal(Status.Approved, appointment.Status);
        }

        [Fact]
        public async Task CheckGettingAppointmentsForTodayAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var service = new AppointmentsService(
                repository,
                this.usersService.Object,
                this.cardsService.Object,
                this.proceduresService.Object,
                this.categoriesService.Object);

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
        public async Task CheckGettingAllAppointmentsForCurrentStylistAndAppointmentsForTodayAsync()
        {
            ApplicationDbContext db = GetDb();

            var service = PreperaAppointmentServiceWithAllDependencies(db);

            await db.Procedures.AddAsync(this.procedure);
            await db.Categories.AddAsync(this.category);
            await db.SaveChangesAsync();

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
        public async Task CheckGettingUpcommingAppointmentsForCurrentClientAsync()
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
        public async Task CheckGettingStylistFreeHoursAsync()
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
        public async Task CheckGettingAppointmentsRequestsForCurrentStylistAsync()
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
        public async Task CheckGettingAppointmentsHistoryForCurrentStylistAsync()
        {
            ApplicationDbContext db = GetDb();

            var service = PreperaAppointmentServiceWithAllDependencies(db);

            await db.Procedures.AddAsync(this.procedure);
            await db.Categories.AddAsync(this.category);
            await db.SaveChangesAsync();

            await this.PrepareAppointmentsAndStatusAsync(service);

            var resultAppointments = await
                service.GetHistoryStylistAsync<TestAppointmentModel>(this.stylist.Id);

            Assert.Equal(3, resultAppointments.Count());
        }

        [Fact]
        public async Task CheckGettingAppointmentsHistoryAllStylistsAsync()
        {
            ApplicationDbContext db = GetDb();

            var service = PreperaAppointmentServiceWithAllDependencies(db);

            await db.Procedures.AddAsync(this.procedure);
            await db.Categories.AddAsync(this.category);
            await db.SaveChangesAsync();

            await this.PrepareAppointmentsAndStatusAsync(service);

            var resultAppointments = await
                service.GetHistoryAllStylistsAsync<TestAppointmentModel>();

            Assert.Equal(3, resultAppointments.Count());
        }

        [Fact]
        public async Task CheckGettingAppointmentsHistoryUserAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);

            var usersService = new Mock<IUsersService>();
            var cardsService = new Mock<ICardsService>();

            var categoriesRepository = new EfDeletableEntityRepository<Category>(db);

            var categoryService = new CategoriesService(categoriesRepository);

            var proceduresRepository = new EfDeletableEntityRepository<Procedure>(db);
            var procedureReviewsRepository = new EfDeletableEntityRepository<Review>(db);
            var procedureProductsRepository = new EfRepository<ProcedureProduct>(db);
            var procedureStylistsRepository = new EfRepository<ProcedureStylist>(db);
            var skinProblemProcedureRepository = new EfRepository<SkinProblemProcedure>(db);

            var proceduresService = new ProceduresService(
                proceduresRepository,
                procedureReviewsRepository,
                procedureProductsRepository,
                procedureStylistsRepository,
                skinProblemProcedureRepository,
                repository,
                categoryService);

            var service = new AppointmentsService(
                repository,
                usersService.Object,
                cardsService.Object,
                proceduresService,
                categoryService);

            await db.Procedures.AddAsync(this.procedure);
            await db.Categories.AddAsync(this.category);
            await db.SaveChangesAsync();

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
        public async Task CheckingAppointmentsForReviewAsync()
        {
            AppointmentsService service = await this.PrepareAppointmentReviewAsync();

            var hasToReview = await
                service.CheckPastProceduresAsync(this.client.Id);
            var resultAppointments = await
                service.GetAppointmentsToReviewAsync<TestAppointmentModel>(this.client.Id);

            Assert.True(hasToReview);
            Assert.Single(resultAppointments);
        }

        private static AppointmentsService PreperaAppointmentServiceWithAllDependencies(ApplicationDbContext db)
        {
            var repository = new EfDeletableEntityRepository<Appointment>(db);
            var usersService = new Mock<IUsersService>();
            var cardsService = new Mock<ICardsService>();

            var categoriesRepository = new EfDeletableEntityRepository<Category>(db);

            var categoryService = new CategoriesService(categoriesRepository);

            var proceduresRepository = new EfDeletableEntityRepository<Procedure>(db);
            var procedureReviewsRepository = new EfDeletableEntityRepository<Review>(db);
            var procedureProductsRepository = new EfRepository<ProcedureProduct>(db);
            var procedureStylistsRepository = new EfRepository<ProcedureStylist>(db);
            var skinProblemProcedureRepository = new EfRepository<SkinProblemProcedure>(db);

            var proceduresService = new ProceduresService(
                proceduresRepository,
                procedureReviewsRepository,
                procedureProductsRepository,
                procedureStylistsRepository,
                skinProblemProcedureRepository,
                repository,
                categoryService);

            var service = new AppointmentsService(
                repository,
                usersService.Object,
                cardsService.Object,
                proceduresService,
                categoryService);

            return service;
        }

        private static async Task<Appointment> GetAppointmentAsync(EfDeletableEntityRepository<Appointment> repository, string firstAppId)
        {
            return await repository.All().FirstOrDefaultAsync(a => a.Id == firstAppId);
        }

        private async Task<AppointmentsService> PrepareAppointmentReviewAsync()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Appointment>(db);

            var usersService = new Mock<IUsersService>();
            var cardsService = new Mock<ICardsService>();

            var categoriesRepository = new EfDeletableEntityRepository<Category>(db);

            var categoryService = new CategoriesService(categoriesRepository);

            var proceduresRepository = new EfDeletableEntityRepository<Procedure>(db);
            var procedureReviewsRepository = new EfDeletableEntityRepository<Review>(db);
            var procedureProductsRepository = new EfRepository<ProcedureProduct>(db);
            var procedureStylistsRepository = new EfRepository<ProcedureStylist>(db);
            var skinProblemProcedureRepository = new EfRepository<SkinProblemProcedure>(db);

            var proceduresService = new ProceduresService(
                proceduresRepository,
                procedureReviewsRepository,
                procedureProductsRepository,
                procedureStylistsRepository,
                skinProblemProcedureRepository,
                repository,
                categoryService);

            var service = new AppointmentsService(
                repository,
                usersService.Object,
                cardsService.Object,
                proceduresService,
                categoryService);

            await db.Procedures.AddAsync(this.procedure);
            await db.Categories.AddAsync(this.category);
            await db.SaveChangesAsync();

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
            var service = new AppointmentsService(
                repository,
                this.usersService.Object,
                this.cardsService.Object,
                this.proceduresService.Object,
                this.categoriesService.Object);

            return service;
        }

        public class TestAppointmentModel : IMapFrom<Appointment>
        {
            public string Id { get; set; }
        }
    }
}
