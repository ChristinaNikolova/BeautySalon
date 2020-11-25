namespace BeautySalon.Services.Data.Tests.Tests
{
    using BeautySalon.Services.Data.Tests.Seed;
    using Xunit;

    public class StylistsServiceTests
    {
        [Fact]
        public void CheckCreatingStylist()
        {
            var stylist = StylistCreator.Create("StylistFirstName", "StylistLastName", "stylistUsername", "stylist@chrisi.de");

            Assert.NotNull(stylist.Id);
        }
    }
}
