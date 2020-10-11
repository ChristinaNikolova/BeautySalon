﻿namespace BeautySalon.Services.Data.Stylists
{
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.JobTypes;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class StylistsService : IStylistsService
    {
        private readonly IRepository<ApplicationUser> stylistsRepository;
        private readonly IRepository<ApplicationRole> rolesRepository;
        private readonly ICategoriesService categoriesService;
        private readonly IJobTypesService jobTypesService;

        public StylistsService(IRepository<ApplicationUser> stylistsepository, IRepository<ApplicationRole> rolesRepository, ICategoriesService categoriesService, IJobTypesService jobTypesService)
        {
            this.stylistsRepository = stylistsepository;
            this.rolesRepository = rolesRepository;
            this.categoriesService = categoriesService;
            this.jobTypesService = jobTypesService;
        }

        public async Task<string> AddRoleStylistAsync(string username, string email)
        {
            var stylist = await this.stylistsRepository
                .All()
                .FirstOrDefaultAsync(s => s.UserName == username && s.Email == email);

            var stylistRole = await this.rolesRepository
                .All()
                .FirstOrDefaultAsync(r => r.Name == GlobalConstants.StylistRoleName);

            var userRole = new IdentityUserRole<string>()
            {
                RoleId = stylistRole.Id,
                UserId = stylist.Id,
            };

            stylist.Roles.Add(userRole);

            // TODO: Check if it WORKS!!!!
            this.stylistsRepository.Update(stylist);
            await this.stylistsRepository.SaveChangesAsync();

            return stylist.Id;
        }

        public async Task<ApplicationUser> UpdateStylistProfileAsync(string id, string categoryName, string jobTypeName, string descripion)
        {
            var stylist = await this.GetByIdAsync(id);
            var category = await this.categoriesService.GetByNameAsync(categoryName);
            var jobType = await this.jobTypesService.GetByNameAsync(jobTypeName);

            stylist.CategoryId = category.Id;
            stylist.JobTypeId = jobType.Id;
            stylist.Description = descripion;

            await this.stylistsRepository.SaveChangesAsync();

            return stylist;
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            var stylist = await this.stylistsRepository
                .All()
                .FirstOrDefaultAsync(s => s.Id == id);

            return stylist;
        }

        public async Task DeleteAsync(string id)
        {
            var stylist = await this.GetByIdAsync(id);

            stylist.IsDeleted = true;

            this.stylistsRepository.Update(stylist);
            await this.stylistsRepository.SaveChangesAsync();
        }
    }
}
