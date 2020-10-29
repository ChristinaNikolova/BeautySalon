namespace BeautySalon.Services.Data.Procedures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.SkinTypes;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ProceduresService : IProceduresService
    {
        // TODO: add products to procedure by create/edit

        // TODO: add SkinProblems to procedure by create/edit
        private readonly IRepository<Procedure> proceduresRepository;
        private readonly IRepository<ProcedureReview> procedureReviewsRepository;
        private readonly IRepository<ProcedureProduct> procedureProductsRepository;

        private readonly ICategoriesService categoriesService;
        private readonly ISkinTypesService skinTypesService;

        public ProceduresService(
            IRepository<Procedure> proceduresRepository,
            IRepository<ProcedureReview> procedureReviewsRepository,
            IRepository<ProcedureProduct> procedureProductsRepository,
            ICategoriesService categoriesService,
            ISkinTypesService skinTypesService)
        {
            this.proceduresRepository = proceduresRepository;
            this.procedureReviewsRepository = procedureReviewsRepository;
            this.procedureProductsRepository = procedureProductsRepository;
            this.categoriesService = categoriesService;
            this.skinTypesService = skinTypesService;
        }

        public async Task<string> CreateAsync(string name, string description, decimal price, string categoryName, string skinTypeName)
        {
            var category = await this.categoriesService.GetByNameAsync(categoryName);
            var skinType = await this.skinTypesService.GetByNameAsync(skinTypeName);

            var procedure = new Procedure()
            {
                Name = name,
                Description = description,
                Price = price,
                CategoryId = category.Id,
                SkinTypeId = skinType.Id,
            };

            await this.proceduresRepository.AddAsync(procedure);
            await this.proceduresRepository.SaveChangesAsync();

            return procedure.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var procedure = await this.GetByIdAsync(id);

            procedure.IsDeleted = true;

            this.proceduresRepository.Update(procedure);
            await this.proceduresRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string name, string description, decimal price, string categoryName, string skinTypeName, string id)
        {
            var procedure = await this.GetByIdAsync(id);
            var category = await this.categoriesService.GetByNameAsync(categoryName);
            var skinType = await this.skinTypesService.GetByNameAsync(skinTypeName);

            procedure.Name = name;
            procedure.Description = description;
            procedure.Price = price;
            procedure.CategoryId = category.Id;
            procedure.SkinTypeId = skinType.Id;

            this.proceduresRepository.Update(procedure);
            await this.proceduresRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllByCategoryAsync<T>(string categoryId)
        {
            var procedures = await this.proceduresRepository
                 .All()
                 .Where(p => p.CategoryId == categoryId)
                 .OrderBy(p => p.Price)
                 .To<T>()
                 .ToListAsync();

            return procedures;
        }

        public async Task<Procedure> GetByIdAsync(string id)
        {
            return await this.proceduresRepository
                .All()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<T> GetProcedureDetailsAsync<T>(string id)
        {
            var procedure = await this.proceduresRepository
                .All()
                .Where(p => p.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return procedure;
        }

        public IEnumerable<T> GetProcedureProducts<T>(string id)
        {
            var products = this.procedureProductsRepository
                .All()
                .Where(pp => pp.ProcedureId == id)
                .OrderBy(pp => pp.Product.Name)
                .To<T>()
                .ToList();

            return products;
        }

        public IEnumerable<T> GetProcedureReviews<T>(string id)
        {
            var reviews = this.procedureReviewsRepository
                .All()
                .Where(pr => pr.ProcedureId == id)
                .OrderByDescending(pr => pr.Date)
                .Take(GlobalConstants.DefaultProcedureReviewsToDisplay)
                .To<T>()
                .ToList();

            return reviews;
        }

        public async Task<IEnumerable<T>> SearchByAsync<T>(string skinTypeId, string criteria)
        {
            var skinCategory = await this.categoriesService
                .GetByNameAsync("Skin Care");

            if (string.IsNullOrWhiteSpace(criteria))
            {
                return await this.FilterByCriteriaAsync<T>(skinTypeId, skinCategory.Id);
            }

            var criteriaToLower = criteria.ToLower();

            if (string.IsNullOrWhiteSpace(skinTypeId))
            {
                if (criteriaToLower == "price")
                {
                    return await this.OrderByPriceAsync<T>(skinCategory.Id);
                }
                else
                {
                    return await this.OrderByRaitingAsync<T>(skinCategory.Id);
                }
            }

            if (criteriaToLower == "price")
            {
                return await this.FilterAndOrderByPriceAsync<T>(skinTypeId, skinCategory.Id);
            }
            else
            {
                return await this.FilterAndOrderByRaitingAsync<T>(skinTypeId, skinCategory.Id);
            }
        }

        private async Task<IEnumerable<T>> FilterAndOrderByRaitingAsync<T>(string skinTypeId, string categoryId)
        {
            return
            await this.proceduresRepository
            .All()
            .Where(p => p.SkinTypeId == skinTypeId && p.CategoryId == categoryId)
            .OrderByDescending(p => p.AverageRating)
            .To<T>()
            .ToListAsync();
        }

        private async Task<IEnumerable<T>> FilterAndOrderByPriceAsync<T>(string skinTypeId, string categoryId)
        {
            return
                await this.proceduresRepository
                .All()
                .Where(p => p.SkinTypeId == skinTypeId && p.CategoryId == categoryId)
                .OrderBy(p => p.Price)
                .To<T>()
                .ToListAsync();
        }

        private async Task<IEnumerable<T>> OrderByRaitingAsync<T>(string categoryId)
        {
            return await this.proceduresRepository
            .All()
            .Where(p => p.CategoryId == categoryId)
            .OrderByDescending(p => p.AverageRating)
            .To<T>()
            .ToListAsync();
        }

        private async Task<IEnumerable<T>> OrderByPriceAsync<T>(string categoryId)
        {
            return await this.proceduresRepository
             .All()
             .Where(p => p.CategoryId == categoryId)
             .OrderBy(p => p.Price)
             .To<T>()
             .ToListAsync();
        }

        private async Task<IEnumerable<T>> FilterByCriteriaAsync<T>(string skinTypeId, string categoryId)
        {
            return await this.proceduresRepository
                 .All()
                 .Where(p => p.SkinTypeId == skinTypeId && p.CategoryId == categoryId)
                 .To<T>()
                 .ToListAsync();
        }
    }
}
