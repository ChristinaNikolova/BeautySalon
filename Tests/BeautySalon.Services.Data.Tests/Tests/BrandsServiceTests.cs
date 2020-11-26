﻿namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Brands;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class BrandsServiceTests
    {
        public BrandsServiceTests()
        {
        }

        [Fact]
        public async Task CheckGettingAllAsSelectListItem()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Brand>(db);
            var service = new BrandsService(repository);

            var firstBrand = new Brand() { Id = Guid.NewGuid().ToString() };
            var secondBrand = new Brand() { Id = Guid.NewGuid().ToString() };
            var thirdBrand = new Brand() { Id = Guid.NewGuid().ToString() };

            await db.Brands.AddAsync(firstBrand);
            await db.Brands.AddAsync(secondBrand);
            await db.Brands.AddAsync(thirdBrand);
            await db.SaveChangesAsync();

            var brands = await service.GetAllAsSelectListItemAsync();

            Assert.Equal(3, brands.Count());
        }

        private static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            return db;
        }
    }
}
