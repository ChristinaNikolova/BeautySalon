﻿namespace BeautySalon.Services.Data.Tests.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Repositories;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.Procedures;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ProceduresServiceTests
    {
        private readonly Mock<IRepository<Review>> procedureReviewsRepository;
        private readonly Mock<IRepository<ProcedureProduct>> procedureProductsRepository;
        private readonly Mock<IRepository<ProcedureStylist>> procedureStylistsRepository;
        private readonly Mock<IRepository<SkinProblemProcedure>> skinProblemProceduresRepository;
        private readonly Mock<IRepository<Appointment>> appointmentsRepository;
        private readonly Mock<ICategoriesService> categoriesService;


        public ProceduresServiceTests()
        {
            new MapperInitializationProfile();
            this.procedureReviewsRepository = new Mock<IRepository<Review>>();
            this.procedureProductsRepository = new Mock<IRepository<ProcedureProduct>>();
            this.procedureStylistsRepository = new Mock<IRepository<ProcedureStylist>>();
            this.skinProblemProceduresRepository = new Mock<IRepository<SkinProblemProcedure>>();
            this.appointmentsRepository = new Mock<IRepository<Appointment>>();
            this.categoriesService = new Mock<ICategoriesService>();
        }

        [Fact]
        public async Task CheckCreatingProcedure()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Procedure>(db);
            var skinProblemProceduresRepository = new EfRepository<SkinProblemProcedure>(db);

            var service = new ProceduresService(
                repository,
                this.procedureReviewsRepository.Object,
                this.procedureProductsRepository.Object,
                this.procedureStylistsRepository.Object,
                skinProblemProceduresRepository,
                this.appointmentsRepository.Object,
                this.categoriesService.Object);

            var firstSkinProblem = new SkinProblem()
            {
                Id = "1",
                Name = "firstSkinProblemName",
            };

            var secondSkinProblem = new SkinProblem()
            {
                Id = "2",
                Name = "secondSkinProblemName",
            };

            var thirdSkinProblem = new SkinProblem()
            {
                Id = "3",
                Name = "thirdSkinProblemName",
            };

            SkinProblem[] skinProblems = new SkinProblem[]
            {
                firstSkinProblem,
                secondSkinProblem,
                thirdSkinProblem,
            };

            await db.SkinProblems.AddRangeAsync(skinProblems);
            await db.SaveChangesAsync();

            var skinProblemsAsSelectListItemt = skinProblems
                .Select(sp => new SelectListItem()
                {
                    Value = sp.Id,
                    Text = sp.Name,
                    Selected = true,
                })
                .ToList();

            var procedureId = await service.CreateAsync("procedureName", "procedureDEscription", 12, "1", "1", "Yes", skinProblemsAsSelectListItemt);

            var procedure = await service.GetByIdAsync(procedureId);

            Assert.NotNull(procedureId);
            Assert.True(procedure.IsSensitive);
            Assert.Equal(3, procedure.SkinProblemProcedures.Count());
        }

        [Fact]
        public async Task CheckDeletingProcedure()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Procedure>(db);
            var skinProblemProceduresRepository = new EfRepository<SkinProblemProcedure>(db);
            var procedureStylistsRepository = new EfRepository<ProcedureStylist>(db);
            var procedureProductsRepository = new EfRepository<ProcedureProduct>(db);

            var service = new ProceduresService(
                repository,
                this.procedureReviewsRepository.Object,
                procedureProductsRepository,
                procedureStylistsRepository,
                skinProblemProceduresRepository,
                this.appointmentsRepository.Object,
                this.categoriesService.Object);

            var firstSkinProblem = new SkinProblem()
            {
                Id = "1",
                Name = "firstSkinProblemName",
            };

            var secondSkinProblem = new SkinProblem()
            {
                Id = "2",
                Name = "secondSkinProblemName",
            };

            var thirdSkinProblem = new SkinProblem()
            {
                Id = "3",
                Name = "thirdSkinProblemName",
            };

            SkinProblem[] skinProblems = new SkinProblem[]
            {
                firstSkinProblem,
                secondSkinProblem,
                thirdSkinProblem,
            };

            await db.SkinProblems.AddRangeAsync(skinProblems);
            await db.SaveChangesAsync();

            var skinProblemsAsSelectListItemt = skinProblems
                .Select(sp => new SelectListItem()
                {
                    Value = sp.Id,
                    Text = sp.Name,
                    Selected = true,
                })
                .ToList();

            var procedureId = await service.CreateAsync("procedureName", "procedureDEscription", 12, "1", "1", "Yes", skinProblemsAsSelectListItemt);

            var procedure = await service.GetByIdAsync(procedureId);

            var firstStylist = new ApplicationUser() { Id = "1" };
            var secondStylist = new ApplicationUser() { Id = "2" };

            var firstStylistProcedure = new ProcedureStylist()
            {
                ProcedureId = procedureId,
                StylistId = firstStylist.Id,
            };

            var secondStylistProcedure = new ProcedureStylist()
            {
                ProcedureId = procedureId,
                StylistId = secondStylist.Id,
            };

            var product = new Product() { Id = "1" };

            var procedureProduct = new ProcedureProduct()
            {
                ProcedureId = procedureId,
                ProductId = product.Id,
            };

            await db.Users.AddAsync(firstStylist);
            await db.Users.AddAsync(secondStylist);
            await db.ProcedureStylists.AddAsync(firstStylistProcedure);
            await db.ProcedureStylists.AddAsync(secondStylistProcedure);
            await db.Products.AddAsync(product);
            await db.ProcedureProducts.AddAsync(procedureProduct);
            await db.SaveChangesAsync();

            await service.DeleteAsync(procedureId);

            Assert.True(procedure.IsDeleted);
            Assert.Empty(procedureProductsRepository.All());
            Assert.Empty(procedureStylistsRepository.All());
            Assert.Empty(procedureProductsRepository.All());
        }

        [Fact]
        public async Task CheckUpdatingProcedure()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Procedure>(db);

            var service = new ProceduresService(
                repository,
                this.procedureReviewsRepository.Object,
                this.procedureProductsRepository.Object,
                this.procedureStylistsRepository.Object,
                this.skinProblemProceduresRepository.Object,
                this.appointmentsRepository.Object,
                this.categoriesService.Object);

            var procedure = new Procedure()
            {
                Id = "1",
                Name = "procedureName",
                Description = "procedureDEscription",
                Price = 12,
                CategoryId = "1",
                SkinTypeId = "1",
                IsSensitive = true,
            };

            await db.Procedures.AddAsync(procedure);
            await db.SaveChangesAsync();

            await service.UpdateAsync(procedure.Id, "newName", procedure.Description, 13, "2", "2", "No");

            Assert.Equal("newName", procedure.Name);
            Assert.Equal("procedureDEscription", procedure.Description);
            Assert.Equal(13, procedure.Price);
            Assert.Equal("2", procedure.SkinTypeId);
            Assert.Equal("2", procedure.CategoryId);
            Assert.True(!procedure.IsSensitive);
        }

        [Fact]
        public async Task CheckGettingAllAndGettingAllByCategoryProcedures()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Procedure>(db);

            var service = new ProceduresService(
                repository,
                this.procedureReviewsRepository.Object,
                this.procedureProductsRepository.Object,
                this.procedureStylistsRepository.Object,
                this.skinProblemProceduresRepository.Object,
                this.appointmentsRepository.Object,
                this.categoriesService.Object);

            var firstCategory = new Category()
            {
                Id = "1",
                Name = "firstCategoryName",
            };

            var secondCategory = new Category()
            {
                Id = "2",
                Name = "secondCategoryName",
            };

            var skinType = new SkinType()
            {
                Id = "1",
                Name = "skinTypeName",
            };

            var firstProcedure = new Procedure() { Id = "1", CategoryId = firstCategory.Id, SkinTypeId = skinType.Id };
            var secondProcedure = new Procedure() { Id = "2", CategoryId = firstCategory.Id, SkinTypeId = skinType.Id };
            var thirdProcedure = new Procedure() { Id = "3", CategoryId = secondCategory.Id, SkinTypeId = skinType.Id };

            await db.Procedures.AddAsync(firstProcedure);
            await db.Procedures.AddAsync(secondProcedure);
            await db.Procedures.AddAsync(thirdProcedure);
            await db.Categories.AddAsync(firstCategory);
            await db.Categories.AddAsync(secondCategory);
            await db.SkinTypes.AddAsync(skinType);
            await db.SaveChangesAsync();

            var procedures = await service.GetAllAdministrationAsync<TestProcedureModel>();
            var proceduresCurrentCategory = await service.GetAllByCategoryAsync<TestProcedureModel>(firstCategory.Id, 5, 0);
            var proceduresByCurrentCategoryCount = await service.GetTotalCountProceduresByCategoryAsync(firstCategory.Id);

            Assert.Equal(3, procedures.Count());
            Assert.Equal(2, proceduresCurrentCategory.Count());
            Assert.Equal(2, proceduresByCurrentCategoryCount);
        }

        [Fact]
        public async Task CheckGettingProcedureDetails()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Procedure>(db);

            var service = new ProceduresService(
                repository,
                this.procedureReviewsRepository.Object,
                this.procedureProductsRepository.Object,
                this.procedureStylistsRepository.Object,
                this.skinProblemProceduresRepository.Object,
                this.appointmentsRepository.Object,
                this.categoriesService.Object);

            var firstProcedure = new Procedure() { Id = "1" };
            var secondProcedure = new Procedure() { Id = "2" };

            await db.Procedures.AddAsync(firstProcedure);
            await db.Procedures.AddAsync(secondProcedure);
            await db.SaveChangesAsync();

            var procedure = await service.GetProcedureDetailsAsync<TestProcedureModel>(firstProcedure.Id);

            Assert.Equal(firstProcedure.Id, procedure.Id);
        }

        [Fact]
        public async Task CheckGettingProcedureProducts()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Procedure>(db);
            var procedureProductsRepository = new EfRepository<ProcedureProduct>(db);

            var service = new ProceduresService(
                repository,
                this.procedureReviewsRepository.Object,
                procedureProductsRepository,
                this.procedureStylistsRepository.Object,
                this.skinProblemProceduresRepository.Object,
                this.appointmentsRepository.Object,
                this.categoriesService.Object);

            var procedure = new Procedure() { Id = "1" };

            var firstProduct = new Product() { Id = "1" };
            var secondProduct = new Product() { Id = "2" };
            var thirdProduct = new Product() { Id = "3" };

            var firstProcedureProduct = new ProcedureProduct()
            {
                ProcedureId = procedure.Id,
                ProductId = firstProduct.Id,
            };

            var secondProcedureProduct = new ProcedureProduct()
            {
                ProcedureId = procedure.Id,
                ProductId = secondProduct.Id,
            };

            await db.Procedures.AddAsync(procedure);
            await db.Products.AddAsync(firstProduct);
            await db.Products.AddAsync(secondProduct);
            await db.Products.AddAsync(thirdProduct);
            await db.ProcedureProducts.AddAsync(firstProcedureProduct);
            await db.ProcedureProducts.AddAsync(secondProcedureProduct);
            await db.SaveChangesAsync();

            var procedures = await service.GetProcedureProductsAsync<TestProcedureProductModel>(procedure.Id);

            Assert.Equal(2, procedures.Count());
        }

        [Fact]
        public async Task CheckGettingProcedureReviews()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Procedure>(db);
            var procedureReviewsRepository = new EfRepository<Review>(db);

            var service = new ProceduresService(
                repository,
                procedureReviewsRepository,
                this.procedureProductsRepository.Object,
                this.procedureStylistsRepository.Object,
                this.skinProblemProceduresRepository.Object,
                this.appointmentsRepository.Object,
                this.categoriesService.Object);

            var procedure = new Procedure() { Id = "1" };

            var firstReview = new Review() { Id = "1", ProcedureId = procedure.Id };
            var secondReview = new Review() { Id = "2", ProcedureId = procedure.Id };
            var thirdReview = new Review() { Id = "3" };

            await db.Procedures.AddAsync(procedure);
            await db.Reviews.AddAsync(firstReview);
            await db.Reviews.AddAsync(secondReview);
            await db.Reviews.AddAsync(thirdReview);
            await db.SaveChangesAsync();

            var procedures = await service.GetProcedureReviewsAsync<TestReviewModel>(procedure.Id);

            Assert.Equal(2, procedures.Count());
        }

        [Fact]
        public async Task CheckGettingProcedureStylists()
        {
            ApplicationDbContext db = GetDb();

            var repository = new EfDeletableEntityRepository<Procedure>(db);
            var procedureStylistsRepository = new EfRepository<ProcedureStylist>(db);

            var service = new ProceduresService(
                repository,
                this.procedureReviewsRepository.Object,
                this.procedureProductsRepository.Object,
                procedureStylistsRepository,
                this.skinProblemProceduresRepository.Object,
                this.appointmentsRepository.Object,
                this.categoriesService.Object);

            var procedure = new Procedure() { Id = "1" };

            var firstStylist = new ApplicationUser() { Id = "1" };
            var secondStylist = new ApplicationUser() { Id = "2" };
            var thirdStylist = new ApplicationUser() { Id = "3" };

            var firstStylistProcedure = new ProcedureStylist()
            {
                ProcedureId = procedure.Id,
                StylistId = firstStylist.Id,
            };

            var secondStylistProcedure = new ProcedureStylist()
            {
                ProcedureId = procedure.Id,
                StylistId = secondStylist.Id,
            };

            await db.Procedures.AddAsync(procedure);
            await db.Users.AddAsync(firstStylist);
            await db.Users.AddAsync(secondStylist);
            await db.Users.AddAsync(thirdStylist);
            await db.ProcedureStylists.AddAsync(firstStylistProcedure);
            await db.ProcedureStylists.AddAsync(secondStylistProcedure);
            await db.SaveChangesAsync();

            var procedures = await service.GetProceduresByStylistAsync<TestProcedureStylistModel>(firstStylist.Id);

            Assert.Single(procedures);
        }

        private static ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            return db;
        }

        public class TestProcedureModel : IMapFrom<Procedure>
        {
            public string Id { get; set; }
        }

        public class TestProcedureProductModel : IMapFrom<ProcedureProduct>
        {
            public string Id { get; set; }
        }

        public class TestReviewModel : IMapFrom<Review>
        {
            public string Id { get; set; }
        }

        public class TestProcedureStylistModel : IMapFrom<ProcedureStylist>
        {
            public string Id { get; set; }
        }
    }
}
