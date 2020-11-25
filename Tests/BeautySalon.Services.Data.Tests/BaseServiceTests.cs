namespace BeautySalon.Services.Data.Tests
{
    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Moq;

    public class BaseServiceTests
    {
        protected ApplicationDbContext context;
        protected Mock<UserManager<ApplicationUser>> userManager;

        protected BaseServiceTests()
        {
            this.context = InMemoryDatabase.Get();
            this.userManager = UserManagerMock.New;
        }
    }
}
