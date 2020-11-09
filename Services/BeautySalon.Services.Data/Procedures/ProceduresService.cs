﻿namespace BeautySalon.Services.Data.Procedures
{
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
        private readonly IRepository<ProcedureStylist> procedureStylistsRepository;

        private readonly ICategoriesService categoriesService;
        private readonly ISkinTypesService skinTypesService;

        public ProceduresService(
            IRepository<Procedure> proceduresRepository,
            IRepository<ProcedureReview> procedureReviewsRepository,
            IRepository<ProcedureProduct> procedureProductsRepository,
            IRepository<ProcedureStylist> procedureStylistsRepository,
            ICategoriesService categoriesService,
            ISkinTypesService skinTypesService)
        {
            this.proceduresRepository = proceduresRepository;
            this.procedureReviewsRepository = procedureReviewsRepository;
            this.procedureProductsRepository = procedureProductsRepository;
            this.procedureStylistsRepository = procedureStylistsRepository;
            this.categoriesService = categoriesService;
            this.skinTypesService = skinTypesService;
        }

        public async Task<string> CreateAsync(string name, string description, decimal price, string categoryId, string skinTypeId, string isSensitive)
        {
            var procedure = new Procedure()
            {
                Name = name,
                Description = description,
                Price = price,
                CategoryId = categoryId,
            };

            CheckSkinType(skinTypeId, isSensitive, procedure);

            await this.proceduresRepository.AddAsync(procedure);
            await this.proceduresRepository.SaveChangesAsync();

            return procedure.Id;
        }

        public async Task<T> GetProcedureDataForUpdateAsync<T>(string id)
        {
            var procedure = await this.proceduresRepository
                .All()
                .Where(p => p.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return procedure;
        }

        public async Task DeleteAsync(string id)
        {
            var procedure = await this.GetByIdAsync(id);

            procedure.IsDeleted = true;

            this.proceduresRepository.Update(procedure);
            await this.proceduresRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, string name, string description, decimal price, string categoryId, string skinTypeId, string isSensitive)
        {
            var procedure = await this.GetByIdAsync(id);

            procedure.Name = name;
            procedure.Description = description;
            procedure.Price = price;
            procedure.CategoryId = categoryId;

            CheckSkinType(skinTypeId, isSensitive, procedure);

            this.proceduresRepository.Update(procedure);
            await this.proceduresRepository.SaveChangesAsync();
        }

        private static void CheckSkinType(string skinTypeId, string isSensitive, Procedure procedure)
        {
            if (!skinTypeId.StartsWith(GlobalConstants.StartDropDownDefaultMessage))
            {
                procedure.SkinTypeId = skinTypeId;
                procedure.IsSensitive = isSensitive == "Yes" ? true : false;
            }
        }

        public async Task<IEnumerable<T>> GetAllAdministrationAsync<T>()
        {
            var procedures = await this.proceduresRepository
                .All()
                .OrderBy(p => p.Category.Name)
                .ThenBy(p => p.SkinType.Name)
                .ThenBy(p => p.Name)
                .To<T>()
                .ToListAsync();

            return procedures;
        }

        public async Task<IEnumerable<T>> GetAllByCategoryAsync<T>(string categoryId, int take, int skip)
        {
            var procedures = await this.proceduresRepository
                 .All()
                 .Where(p => p.CategoryId == categoryId)
                 .OrderBy(p => p.Price)
                 .Skip(skip)
                 .Take(take)
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

        public async Task<IEnumerable<T>> GetProcedureProductsAsync<T>(string id)
        {
            var products = await this.procedureProductsRepository
                .All()
                .Where(pp => pp.ProcedureId == id)
                .OrderBy(pp => pp.Product.Name)
                .To<T>()
                .ToListAsync();

            return products;
        }

        public async Task<IEnumerable<T>> GetProcedureReviewsAsync<T>(string id)
        {
            var reviews = await this.procedureReviewsRepository
                .All()
                .Where(pr => pr.ProcedureId == id)
                .OrderByDescending(pr => pr.Date)
                .Take(GlobalConstants.DefaultProcedureReviewsToDisplay)
                .To<T>()
                .ToListAsync();

            return reviews;
        }

        public async Task<IEnumerable<T>> SearchByAsync<T>(string skinTypeId, string criteria)
        {
            var skinCategory = await this.categoriesService
                .GetByNameAsync(GlobalConstants.CategorySkinName);

            if (string.IsNullOrWhiteSpace(criteria))
            {
                return await this.FilterByCriteriaAsync<T>(skinTypeId, skinCategory.Id);
            }

            var criteriaToLower = criteria.ToLower();

            if (string.IsNullOrWhiteSpace(skinTypeId))
            {
                if (criteriaToLower == GlobalConstants.PriceCriteria)
                {
                    return await this.OrderByPriceAsync<T>(skinCategory.Id);
                }
                else
                {
                    return await this.OrderByRaitingAsync<T>(skinCategory.Id);
                }
            }

            if (criteriaToLower == GlobalConstants.PriceCriteria)
            {
                return await this.FilterAndOrderByPriceAsync<T>(skinTypeId, skinCategory.Id);
            }
            else
            {
                return await this.FilterAndOrderByRaitingAsync<T>(skinTypeId, skinCategory.Id);
            }
        }

        public async Task<int> GetTotalCountProceduresByCategoryAsync(string categoryId)
        {
            var proceduresCount = await this.proceduresRepository
                .All()
                .Where(p => p.CategoryId == categoryId)
                .CountAsync();

            return proceduresCount;
        }

        public async Task<IEnumerable<T>> GetProceduresByStylistAsync<T>(string stylistId)
        {
            var procedureNames = await this.procedureStylistsRepository
                .All()
                .Where(ps => ps.StylistId == stylistId)
                .OrderBy(ps => ps.Procedure.Name)
                .To<T>()
                .ToListAsync();

            return procedureNames;
        }

        public async Task<IEnumerable<T>> GetSmartSearchProceduresAsync<T>(string clientSkinTypeId, string skinSensitive, string stylistId)
        {
            var isSkinSensitive = skinSensitive.Contains("yes".ToLower()) ? true : false;

            var procedures = await this.procedureStylistsRepository
                .All()
                .Where(ps => ps.StylistId == stylistId
                && ps.Procedure.SkinTypeId == clientSkinTypeId
                && ps.Procedure.IsSensitive == isSkinSensitive)
                .OrderBy(ps => ps.Procedure.Name)
                .To<T>()
                .ToListAsync();

            return procedures;
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
