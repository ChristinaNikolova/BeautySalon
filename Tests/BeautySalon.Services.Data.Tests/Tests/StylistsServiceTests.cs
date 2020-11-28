namespace BeautySalon.Services.Data.Tests.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Cloudinary;
    using BeautySalon.Services.Data.Stylists;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class StylistsServiceTests : BaseServiceTests
    {
        private readonly Mock<IFormFile> mockPicture;

        private readonly Mock<IRepository<ApplicationRole>> rolesRepository;
        private readonly Mock<IRepository<ProcedureStylist>> procedureStylistsRepository;
        private readonly Mock<IRepository<Procedure>> proceduresRepository;
        private readonly Mock<ICloudinaryService> cloudinaryService;

        public StylistsServiceTests()
        {
            this.rolesRepository = new Mock<IRepository<ApplicationRole>>();
            this.procedureStylistsRepository = new Mock<IRepository<ProcedureStylist>>();
            this.proceduresRepository = new Mock<IRepository<Procedure>>();
            this.cloudinaryService = new Mock<ICloudinaryService>();
            this.mockPicture = new Mock<IFormFile>();
        }

        [Fact]
        public async Task CheckAddingRoleToExistingNonStylist()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(db);
            var service = new StylistsService(
                repository,
                rolesRepository,
                this.procedureStylistsRepository.Object,
                this.proceduresRepository.Object,
                this.cloudinaryService.Object);

            var role = new ApplicationRole()
            {
                Id = "1",
                Name = GlobalConstants.StylistRoleName,
            };

            await db.Roles.AddAsync(role);
            await db.SaveChangesAsync();

            var stylistId = await service.AddRoleStylistAsync("test@mail.com");

            Assert.Null(stylistId);
        }

        [Fact]
        public async Task CheckUpdatingStylist()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var procedureStylistsRepostory = new EfRepository<ProcedureStylist>(db);
            var proceduresRepostory = new EfDeletableEntityRepository<Procedure>(db);

            var service = new StylistsService(
                repository,
                this.rolesRepository.Object,
                procedureStylistsRepostory,
                proceduresRepostory,
                this.cloudinaryService.Object);

            var category = new Category()
            {
                Id = "1",
                Name = "Hair",
            };

            var newCategory = new Category()
            {
                Id = "2",
                Name = "Makeup",
            };

            var jobType = new JobType()
            {
                Id = "1",
                Name = "Stylist",
            };

            var firstProcedure = new Procedure()
            {
                Id = "1",
                CategoryId = newCategory.Id,
            };

            var secondProcedure = new Procedure()
            {
                Id = "2",
                CategoryId = newCategory.Id,
            };

            var stylist = new ApplicationUser()
            {
                Id = "1",
                Email = "test@email.com",
                FirstName = "stylistFirstName",
                LastName = "stylistLastName",
                Description = "stylistTestDescription",
                CategoryId = category.Id,
                JobTypeId = jobType.Id,
            };

            await db.Users.AddAsync(stylist);
            await db.Categories.AddAsync(category);
            await db.Categories.AddAsync(newCategory);
            await db.Procedures.AddAsync(firstProcedure);
            await db.Procedures.AddAsync(secondProcedure);
            await db.JobTypes.AddAsync(jobType);
            await db.SaveChangesAsync();

            var picture = this.mockPicture.Object;

            var updatedStylist = await service.UpdateStylistProfileAsync(stylist.Id, "newFirstName", stylist.LastName, "123", newCategory.Id, stylist.JobType.Id, "newStylistDescription", picture);

            var pictureAsUrl = await service.GetPictureUrlAsync(updatedStylist.Id);

            Assert.Equal("newFirstName", updatedStylist.FirstName);
            Assert.Equal(stylist.LastName, updatedStylist.LastName);
            Assert.Equal("123", updatedStylist.PhoneNumber);
            Assert.Equal(newCategory.Name, updatedStylist.Category.Name);
            Assert.Equal(stylist.JobType.Name, updatedStylist.JobType.Name);
            Assert.Equal("newStylistDescription", updatedStylist.Description);
            Assert.Equal(pictureAsUrl, updatedStylist.Picture);
            Assert.Equal(2, updatedStylist.StylistProcedures.Count());
        }

        [Fact]
        public async Task CheckGettingAllUsersWithRoleStylist()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(db);

            var service = new StylistsService(
                repository,
                rolesRepository,
                this.procedureStylistsRepository.Object,
                this.proceduresRepository.Object,
                this.cloudinaryService.Object);

            var firstStylist = new ApplicationUser() { Id = "1", Email = "firstStylist@mail.com" };
            var secondStylist = new ApplicationUser() { Id = "2", Email = "secondStylist@mail.com" };
            var thirdStylist = new ApplicationUser() { Id = "3", Email = "thirdStylist@mail.com" };

            var role = new ApplicationRole()
            {
                Id = "1",
                Name = GlobalConstants.StylistRoleName,
            };

            await db.Users.AddAsync(firstStylist);
            await db.Users.AddAsync(secondStylist);
            await db.Users.AddAsync(thirdStylist);
            await db.Roles.AddAsync(role);
            await db.SaveChangesAsync();

            await service.AddRoleStylistAsync(firstStylist.Email);
            await service.AddRoleStylistAsync(secondStylist.Email);
            await service.AddRoleStylistAsync(thirdStylist.Email);

            var stylists = await service.GetAllAsync<TestStylistModel>();

            Assert.Equal(3, stylists.Count());
            Assert.Contains(firstStylist.Roles, r => r.RoleId == role.Id);
            Assert.Contains(secondStylist.Roles, r => r.RoleId == role.Id);
            Assert.Contains(thirdStylist.Roles, r => r.RoleId == role.Id);
        }

        [Fact]
        public async Task CheckGettingStylistsDetails()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(db);

            var service = new StylistsService(
                repository,
                rolesRepository,
                this.procedureStylistsRepository.Object,
                this.proceduresRepository.Object,
                this.cloudinaryService.Object);

            var stylist = new ApplicationUser() { Id = "1" };

            await db.Users.AddAsync(stylist);
            await db.SaveChangesAsync();

            var stylistDetails = await service.GetStylistDetailsAsync<TestStylistModel>(stylist.Id);

            Assert.Equal(stylist.Id, stylistDetails.Id);
        }

        [Fact]
        public async Task CheckSearchingStylistByCategory()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var rolesRepository = new EfDeletableEntityRepository<ApplicationRole>(db);

            var service = new StylistsService(
                repository,
                rolesRepository,
                this.procedureStylistsRepository.Object,
                this.proceduresRepository.Object,
                this.cloudinaryService.Object);

            var firstCategory = new Category()
            {
                Id = "1",
                Name = "Hair",
            };

            var secondCategory = new Category()
            {
                Id = "2",
                Name = "Makeup",
            };

            var firstStylist = new ApplicationUser() { Id = "1", Email = "firstStylist@mail.com", CategoryId = firstCategory.Id };
            var secondStylist = new ApplicationUser() { Id = "2", Email = "secondStylist@mail.com", CategoryId = firstCategory.Id };
            var thirdStylist = new ApplicationUser() { Id = "3", Email = "thirdStylist@mail.com", CategoryId = secondCategory.Id };

            var role = new ApplicationRole()
            {
                Id = "1",
                Name = GlobalConstants.StylistRoleName,
            };

            await db.Users.AddAsync(firstStylist);
            await db.Users.AddAsync(secondStylist);
            await db.Users.AddAsync(thirdStylist);
            await db.Categories.AddAsync(firstCategory);
            await db.Categories.AddAsync(secondCategory);
            await db.Roles.AddAsync(role);
            await db.SaveChangesAsync();

            await service.AddRoleStylistAsync(firstStylist.Email);
            await service.AddRoleStylistAsync(secondStylist.Email);
            await service.AddRoleStylistAsync(thirdStylist.Email);

            var stylists = await service.SearchByCategoryAsync<TestStylistModel>(firstCategory.Id);

            Assert.Equal(2, stylists.Count());
        }

        [Fact]
        public async Task CheckAddingExistingProcedureStyToStylist()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var procedureStylistsRepository = new EfRepository<ProcedureStylist>(db);

            var service = new StylistsService(
                repository,
                this.rolesRepository.Object,
                procedureStylistsRepository,
                this.proceduresRepository.Object,
                this.cloudinaryService.Object);

            var stylist = new ApplicationUser() { Id = "1" };
            var procedure = new Procedure() { Id = "1" };

            var stylistProcedure = new ProcedureStylist()
            {
                StylistId = stylist.Id,
                ProcedureId = procedure.Id,
            };

            await db.Users.AddAsync(stylist);
            await db.Procedures.AddAsync(procedure);
            await db.ProcedureStylists.AddAsync(stylistProcedure);
            await db.SaveChangesAsync();

            var isAdded = await service.AddProcedureToStylistAsync(stylist.Id, procedure.Id);

            Assert.True(!isAdded);
        }

        [Fact]
        public async Task CheckAddingNonExistingProcedureStyToStylist()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var procedureStylistsRepository = new EfRepository<ProcedureStylist>(db);

            var service = new StylistsService(
                repository,
                this.rolesRepository.Object,
                procedureStylistsRepository,
                this.proceduresRepository.Object,
                this.cloudinaryService.Object);

            var stylist = new ApplicationUser() { Id = "1" };
            var procedure = new Procedure() { Id = "1" };

            await db.Users.AddAsync(stylist);
            await db.Procedures.AddAsync(procedure);
            await db.SaveChangesAsync();

            var isAdded = await service.AddProcedureToStylistAsync(stylist.Id, procedure.Id);

            Assert.True(isAdded);
        }

        [Fact]
        public async Task CheckRemovingProcedure()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<ApplicationUser>(db);
            var procedureStylistsRepository = new EfRepository<ProcedureStylist>(db);

            var service = new StylistsService(
                repository,
                this.rolesRepository.Object,
                procedureStylistsRepository,
                this.proceduresRepository.Object,
                this.cloudinaryService.Object);

            var firstStylist = new ApplicationUser() { Id = "1" };
            var secondStylist = new ApplicationUser() { Id = "2" };
            var firstProcedure = new Procedure() { Id = "1" };
            var secondProcedure = new Procedure() { Id = "2" };

            await db.Users.AddAsync(firstStylist);
            await db.Users.AddAsync(secondStylist);
            await db.Procedures.AddAsync(firstProcedure);
            await db.Procedures.AddAsync(secondProcedure);
            await db.SaveChangesAsync();

            await service.AddProcedureToStylistAsync(firstStylist.Id, firstProcedure.Id);
            await service.AddProcedureToStylistAsync(secondStylist.Id, firstProcedure.Id);
            await service.AddProcedureToStylistAsync(secondStylist.Id, secondProcedure.Id);

            await service.RemoveProcedureAsync(firstStylist.Id, firstProcedure.Id);
            await service.RemoveAllProceduresAsync(secondStylist.Id);

            Assert.Empty(firstStylist.StylistProcedures);
            Assert.Empty(secondStylist.StylistProcedures);
        }

        public class TestStylistModel : IMapFrom<ApplicationUser>
        {
            public string Id { get; set; }
        }
    }
}
