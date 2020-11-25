//using BeautySalon.Data;
//using BeautySalon.Data.Models;
//using BeautySalon.Data.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace BeautySalon.Services.Data.Tests.Tests
//{
//    public class ArticlesServiceTests
//    {
//        private readonly ApplicationUser stylist;
//        private readonly ApplicationUser client;
//        private readonly Procedure procedure;

//        public ArticlesServiceTests()
//        {
//            new MapperInitializationProfile();
//            this.stylist = new ApplicationUser()
//            {
//                Id = "1",
//            };
//            this.client = new ApplicationUser()
//            {
//                Id = "1",
//            };
//            this.procedure = new Procedure()
//            {
//                Id = "1",
//            };
//        }

//        [Fact]
//        public async Task CheckCreatingAppointment()
//        {
//            ApplicationDbContext db = GetDb();

//            var repository = new EfDeletableEntityRepository<Article>(db);
//            var service = new ArticlesService(repository);

//            var appointmentId = await service.CreateAsync(this.client.Id, this.stylist.Id, this.procedure.Id, DateTime.UtcNow, "11:00", "test comment");

//            Assert.NotNull(appointmentId);
//        }
//        private static ApplicationDbContext GetDb()
//        {
//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
//            var db = new ApplicationDbContext(options);
//            return db;
//        }

//        public class TestExerciseModel : IMapFrom<Appointment>
//        {
//            public string Id { get; set; }
//        }
//    }
//}
