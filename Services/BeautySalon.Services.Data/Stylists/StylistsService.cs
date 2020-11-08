namespace BeautySalon.Services.Data.Stylists
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Cloudinary;
    using BeautySalon.Services.Data.Categories;
    using BeautySalon.Services.Data.JobTypes;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class StylistsService : IStylistsService
    {
        private readonly IRepository<ApplicationUser> stylistsRepository;
        private readonly IRepository<ApplicationRole> rolesRepository;
        private readonly ICategoriesService categoriesService;
        private readonly IJobTypesService jobTypesService;
        private readonly ICloudinaryService cloudinaryService;

        public StylistsService(IRepository<ApplicationUser> stylistsepository, IRepository<ApplicationRole> rolesRepository, ICategoriesService categoriesService, IJobTypesService jobTypesService, ICloudinaryService cloudinaryService)
        {
            this.stylistsRepository = stylistsepository;
            this.rolesRepository = rolesRepository;
            this.categoriesService = categoriesService;
            this.jobTypesService = jobTypesService;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<T>> GetAllAdministrationAsync<T>()
        {
            var stylistRoleId = this.GetStylistRoleId();

            var stylists = await this.stylistsRepository
                .All()
                .Where(s => s.Roles.Any(r => r.RoleId == stylistRoleId))
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName)
                .To<T>()
                .ToListAsync();

            return stylists;
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

        public async Task<ApplicationUser> UpdateStylistProfileAsync(string id, string firstName, string lastName, string phoneNumber, string category, string jobType, string descripion, IFormFile newPicture)
        {
            var stylist = await this.GetByIdAsync(id);

            if (newPicture != null)
            {
                var fullName = firstName + " " + lastName;
                var newPictureAsUrl = await this.cloudinaryService.UploudAsync(newPicture, fullName);

                stylist.Picture = newPictureAsUrl;
            }

            stylist.FirstName = firstName;
            stylist.LastName = lastName;
            stylist.PhoneNumber = phoneNumber;
            stylist.CategoryId = category;
            stylist.JobTypeId = jobType;
            stylist.Description = descripion;

            this.stylistsRepository.Update(stylist);
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

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            string stylistRoleId = this.GetStylistRoleId();

            var stylists = await this.stylistsRepository
                .All()
                .Where(s => s.Roles.Any(r => r.RoleId == stylistRoleId))
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName)
                .To<T>()
                .ToListAsync();

            return stylists;
        }

        public async Task<T> GetStylistDetailsAsync<T>(string id)
        {
            var stylists = await this.stylistsRepository
                 .All()
                 .Where(s => s.Id == id)
                 .To<T>()
                 .FirstOrDefaultAsync();

            return stylists;
        }

        public async Task<IEnumerable<T>> SearchByAsync<T>(string categoryId, string criteria)
        {
            string stylistRoleId = this.GetStylistRoleId();

            if (string.IsNullOrWhiteSpace(criteria))
            {
                return await this.FilterSylistsAsync<T>(categoryId, stylistRoleId);
            }

            var criteriaToLower = criteria.ToLower();

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                if (criteriaToLower == GlobalConstants.NameCriteria)
                {
                    return await this.OrderByNameAsync<T>(stylistRoleId);
                }
                else
                {
                    return await this.OrderByRaitingAsync<T>(stylistRoleId);
                }
            }

            if (criteriaToLower == GlobalConstants.NameCriteria)
            {
                return await this.FilterAndOrderByNameAsync<T>(categoryId, stylistRoleId);
            }
            else
            {
                return await this.FilterAndOrderByRaitingAsync<T>(categoryId, stylistRoleId);
            }
        }

        public async Task<T> GetStylistNamesAsync<T>(string id)
        {
            var stylist = await this.stylistsRepository
                .All()
                .Where(s => s.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return stylist;
        }

        public async Task<IEnumerable<T>> GetStylistsByCategoryAsync<T>(string categoryId)
        {
            var stylistNames = await this.stylistsRepository
                .All()
                .Where(s => s.CategoryId == categoryId)
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName)
                .To<T>()
                .ToListAsync();

            return stylistNames;
        }

        public async Task<T> GetStylistDataForUpdateAsync<T>(string id)
        {
            var stylist = await this.stylistsRepository
                .All()
                .Where(s => s.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return stylist;
        }

        private async Task<IEnumerable<T>> FilterAndOrderByRaitingAsync<T>(string categoryId, string stylistRoleId)
        {
            return
            await this.stylistsRepository
            .All()
            .Where(s => s.CategoryId == categoryId && s.Roles.Any(r => r.RoleId == stylistRoleId))
            .OrderByDescending(p => p.AverageRating)
            .To<T>()
            .ToListAsync();
        }

        private async Task<IEnumerable<T>> FilterAndOrderByNameAsync<T>(string categoryId, string stylistRoleId)
        {
            return
                await this.stylistsRepository
                .All()
                .Where(s => s.CategoryId == categoryId && s.Roles.Any(r => r.RoleId == stylistRoleId))
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName)
                .To<T>()
                .ToListAsync();
        }

        private async Task<IEnumerable<T>> OrderByRaitingAsync<T>(string stylistRoleId)
        {
            return await this.stylistsRepository
            .All()
            .Where(s => s.Roles.Any(r => r.RoleId == stylistRoleId))
            .OrderByDescending(p => p.AverageRating)
            .To<T>()
            .ToListAsync();
        }

        private async Task<IEnumerable<T>> OrderByNameAsync<T>(string stylistRoleId)
        {
            return await this.stylistsRepository
             .All()
             .Where(s => s.Roles.Any(r => r.RoleId == stylistRoleId))
             .OrderBy(s => s.FirstName)
             .ThenBy(s => s.LastName)
             .To<T>()
             .ToListAsync();
        }

        private async Task<IEnumerable<T>> FilterSylistsAsync<T>(string categoryId, string stylistRoleId)
        {
            return await this.stylistsRepository
                 .All()
                 .Where(s => s.CategoryId == categoryId && s.Roles.Any(r => r.RoleId == stylistRoleId))
                 .To<T>()
                 .ToListAsync();
        }

        private string GetStylistRoleId()
        {
            return this.rolesRepository
                .All()
                .Where(r => r.Name == GlobalConstants.StylistRoleName)
                .Select(r => r.Id)
                .FirstOrDefault();
        }
    }
}
