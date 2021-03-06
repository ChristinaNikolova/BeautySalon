﻿namespace BeautySalon.Services.Data.Procedures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class ProceduresService : IProceduresService
    {
        private readonly IRepository<Procedure> proceduresRepository;
        private readonly IRepository<Review> procedureReviewsRepository;
        private readonly IRepository<ProcedureProduct> procedureProductsRepository;
        private readonly IRepository<SkinProblemProcedure> skinProblemProceduresRespository;
        private readonly IRepository<ProcedureStylist> procedureStylistsRepository;
        private readonly IRepository<Appointment> appointmentsRepository;
        private readonly ICategoriesService categoriesService;

        public ProceduresService(
            IRepository<Procedure> proceduresRepository,
            IRepository<Review> procedureReviewsRepository,
            IRepository<ProcedureProduct> procedureProductsRepository,
            IRepository<ProcedureStylist> procedureStylistsRepository,
            IRepository<SkinProblemProcedure> skinProblemProceduresRepository,
            IRepository<Appointment> appointmentsRepository,
            ICategoriesService categoriesService)
        {
            this.proceduresRepository = proceduresRepository;
            this.procedureReviewsRepository = procedureReviewsRepository;
            this.procedureProductsRepository = procedureProductsRepository;
            this.procedureStylistsRepository = procedureStylistsRepository;
            this.skinProblemProceduresRespository = skinProblemProceduresRepository;
            this.appointmentsRepository = appointmentsRepository;
            this.categoriesService = categoriesService;
        }

        public async Task<string> CreateAsync(string name, string description, decimal price, string categoryId, string skinTypeId, string isSensitive, IList<SelectListItem> skinProblems)
        {
            var procedure = new Procedure()
            {
                Name = name,
                Description = description,
                Price = price,
                CategoryId = categoryId,
            };

            await this.CheckSkinTypeAsync(skinTypeId, isSensitive, procedure, skinProblems);

            await this.proceduresRepository.AddAsync(procedure);
            await this.proceduresRepository.SaveChangesAsync();

            return procedure.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var procedure = await this.GetByIdAsync(id);

            procedure.IsDeleted = true;

            await this.RemoveAllProductsAsync(id);
            await this.RemoveAllSkinProblemsAsync(id);
            await this.RemoveAllStylistsAsync(id);

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

            await this.CheckSkinTypeAsync(skinTypeId, isSensitive, procedure);

            this.proceduresRepository.Update(procedure);
            await this.proceduresRepository.SaveChangesAsync();
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
                .OrderByDescending(pr => pr.CreatedOn)
                .To<T>()
                .ToListAsync();

            return reviews;
        }

        public async Task<IEnumerable<T>> SearchByAsync<T>(string skinTypeId, string criteria)
        {
            var skinCategory = await this.categoriesService
                .GetByNameAsync(GlobalConstants.CategorySkinName);

            var query = this.proceduresRepository
                              .All()
                              .Where(p => p.CategoryId == skinCategory.Id)
                              .AsQueryable();

            if (!string.IsNullOrWhiteSpace(skinTypeId))
            {
                query = query.Where(p => p.SkinTypeId == skinTypeId);
            }

            var criteriaToLower = criteria.ToLower();

            if (criteriaToLower == GlobalConstants.PriceCriteria)
            {
                query = query.OrderBy(p => p.Price);
            }
            else if (criteriaToLower == GlobalConstants.RatingCriteria)
            {
                query = query.OrderByDescending(p => p.AverageRating);
            }

            var procedures = await query
                .To<T>()
                .ToListAsync();

            return procedures;
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

        public async Task<string> GetProcedureIdByNameAsync(string procedureName)
        {
            var procedureId = await this.proceduresRepository
                .All()
                .Where(p => p.Name == procedureName)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

            return procedureId;
        }

        public async Task<bool> AddProductToProcedureAsync(string id, string productId)
        {
            var isAlreadyAdded = await this.procedureProductsRepository
                .All()
                .AnyAsync(pp => pp.ProductId == productId && pp.ProcedureId == id);

            if (isAlreadyAdded)
            {
                return false;
            }

            var procedureProduct = new ProcedureProduct()
            {
                ProductId = productId,
                ProcedureId = id,
            };

            await this.procedureProductsRepository.AddAsync(procedureProduct);
            await this.procedureProductsRepository.SaveChangesAsync();

            return true;
        }

        public async Task RemoveProductAsync(string productId, string procedureId)
        {
            var procedureProduct = await this.procedureProductsRepository
               .All()
               .FirstOrDefaultAsync(pp => pp.ProductId == productId && pp.ProcedureId == procedureId);

            this.procedureProductsRepository.Delete(procedureProduct);
            await this.procedureProductsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetSmartSearchProceduresAsync<T>(string clientSkinTypeId, string skinSensitive, string stylistId)
        {
            var isSkinSensitive = skinSensitive.ToLower().Contains("yes".ToLower());

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

        public async Task<IEnumerable<T>> GetPerfectProceduresForSkinTypeAsync<T>(bool isSkinSensitive, string skinTypeId)
        {
            var procedures = await this.proceduresRepository
                .All()
                .Where(p => p.IsSensitive == isSkinSensitive
                    && p.SkinTypeId == skinTypeId)
                .OrderBy(p => p.Name)
                .To<T>()
                .ToListAsync();

            return procedures;
        }

        public async Task AddProcedureReviewsAsync(string appoitmentId, string content, int points)
        {
            var appointment = await this.appointmentsRepository
                .All()
                .FirstOrDefaultAsync(a => a.Id == appoitmentId);

            await this.CalculateAverageRaitingAsync(points, appointment);

            appointment.IsReview = true;

            var review = new Review()
            {
                ProcedureId = appointment.ProcedureId,
                ClientId = appointment.ClientId,
                Content = content,
                Points = points,
                Date = DateTime.UtcNow,
                AppointmentId = appoitmentId,
            };

            this.appointmentsRepository.Update(appointment);
            await this.procedureReviewsRepository.AddAsync(review);
            await this.procedureReviewsRepository.SaveChangesAsync();
            await this.appointmentsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetProceduresUseProductAsync<T>(string productId)
        {
            var procedures = await this.proceduresRepository
                .All()
                .Where(p => p.ProcedureProducts.Any(pp => pp.ProductId == productId))
                .OrderBy(p => p.Name)
                .To<T>()
                .ToListAsync();

            return procedures;
        }

        public async Task<string> GetIdByNameAsync(string name)
        {
            var id = await this.proceduresRepository
                .All()
                .Where(p => p.Name == name)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

            return id;
        }

        private async Task CalculateAverageRaitingAsync(int points, Appointment appointment)
        {
            var procedure = await this.proceduresRepository
             .All()
             .FirstOrDefaultAsync(p => p.Id == appointment.ProcedureId);

            var totalReviews = await this.procedureReviewsRepository
                .All()
                .Where(pr => pr.ProcedureId == appointment.ProcedureId)
                .CountAsync();

            var totalPoints = await this.procedureReviewsRepository
                .All()
                .Where(pr => pr.ProcedureId == appointment.ProcedureId)
                .SumAsync(pr => pr.Points);

            procedure.AverageRating = (double)(totalPoints + points) / (totalReviews + 1);

            this.proceduresRepository.Update(procedure);
            await this.proceduresRepository.SaveChangesAsync();
        }

        private async Task CheckSkinTypeAsync(string skinTypeId, string isSensitive, Procedure procedure, IList<SelectListItem> skinProblems = null)
        {
            if (!skinTypeId.StartsWith(GlobalConstants.StartDropDownDefaultMessage))
            {
                procedure.SkinTypeId = skinTypeId;
                procedure.IsSensitive = isSensitive.ToLower() == "Yes".ToLower();

                if (skinProblems != null)
                {
                    await this.GetSkinProblemsAsync(procedure, skinProblems);
                }
            }
        }

        private async Task GetSkinProblemsAsync(Procedure procedure, IList<SelectListItem> skinProblems)
        {
            var selectedSkinProblems = skinProblems
                .Where(sp => sp.Selected == true)
                .Select(sp => sp.Value)
                .ToList();

            foreach (var skinProblem in selectedSkinProblems)
            {
                var skinProblemProcedure = new SkinProblemProcedure()
                {
                    SkinProblemId = skinProblem,
                    ProcedureId = procedure.Id,
                };

                await this.skinProblemProceduresRespository.AddAsync(skinProblemProcedure);
            }
        }

        private async Task RemoveAllProductsAsync(string id)
        {
            var products = await this.procedureProductsRepository
                .All()
                .Where(pp => pp.ProcedureId == id)
                .ToListAsync();

            foreach (var product in products)
            {
                this.procedureProductsRepository.Delete(product);
            }

            await this.procedureProductsRepository.SaveChangesAsync();
        }

        private async Task RemoveAllSkinProblemsAsync(string id)
        {
            var skinProblems = await this.skinProblemProceduresRespository
                .All()
                .Where(spp => spp.ProcedureId == id)
                .ToListAsync();

            foreach (var skinProblem in skinProblems)
            {
                this.skinProblemProceduresRespository.Delete(skinProblem);
            }

            await this.skinProblemProceduresRespository.SaveChangesAsync();
        }

        private async Task RemoveAllStylistsAsync(string id)
        {
            var stylists = await this.procedureStylistsRepository
                .All()
                .Where(ps => ps.ProcedureId == id)
                .ToListAsync();

            foreach (var stylist in stylists)
            {
                this.procedureStylistsRepository.Delete(stylist);
            }

            await this.procedureStylistsRepository.SaveChangesAsync();
        }
    }
}
