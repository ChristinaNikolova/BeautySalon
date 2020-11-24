namespace BeautySalon.Services.Data.Stylists
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Cloudinary;
    using BeautySalon.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class StylistsService : IStylistsService
    {
        private readonly IRepository<ApplicationUser> stylistsRepository;
        private readonly IRepository<ApplicationRole> rolesRepository;
        private readonly IRepository<ProcedureStylist> procedureStylistsRepository;
        private readonly IRepository<Procedure> proceduresRepository;
        private readonly ICloudinaryService cloudinaryService;

        public StylistsService(IRepository<ApplicationUser> stylistsepository, IRepository<ApplicationRole> rolesRepository, IRepository<ProcedureStylist> procedureStylistsRepository, IRepository<Procedure> proceduresRepository, ICloudinaryService cloudinaryService)
        {
            this.stylistsRepository = stylistsepository;
            this.rolesRepository = rolesRepository;
            this.procedureStylistsRepository = procedureStylistsRepository;
            this.proceduresRepository = proceduresRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<T>> GetAllAdministrationAsync<T>()
        {
            var stylistRoleId = await this.GetStylistRoleIdAsync();

            var stylists = await this.stylistsRepository
                .All()
                .Where(s => s.Roles.Any(r => r.RoleId == stylistRoleId))
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName)
                .To<T>()
                .ToListAsync();

            return stylists;
        }

        public async Task<string> AddRoleStylistAsync(string email)
        {
            var stylist = await this.stylistsRepository
                .All()
                .FirstOrDefaultAsync(s => s.Email == email);

            if (stylist == null)
            {
                return null;
            }

            var stylistRole = await this.rolesRepository
                .All()
                .FirstOrDefaultAsync(r => r.Name == GlobalConstants.StylistRoleName);

            var userRole = new IdentityUserRole<string>()
            {
                RoleId = stylistRole.Id,
                UserId = stylist.Id,
            };

            stylist.Roles.Add(userRole);

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

            var areProceduresAlreadyAdded = await this.procedureStylistsRepository
                .All()
                .AnyAsync(ps => ps.StylistId == stylist.Id);

            if (!areProceduresAlreadyAdded)
            {
                await this.AddProcedureToStylistByCreatingStylistAsync(id, category);
            }

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

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            string stylistRoleId = await this.GetStylistRoleIdAsync();

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

        public async Task<IEnumerable<T>> SearchByCategoryAsync<T>(string categoryId)
        {
            string stylistRoleId = await this.GetStylistRoleIdAsync();

            var stylists = await this.stylistsRepository
                .All()
                .Where(s => s.CategoryId == categoryId && s.Roles.Any(r => r.RoleId == stylistRoleId))
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName)
                .To<T>()
                .ToListAsync();

            return stylists;
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

        public async Task<string> GetPictureUrlAsync(string id)
        {
            var pictureUrl = await this.stylistsRepository
                .All()
                .Where(s => s.Id == id)
                .Select(s => s.Picture)
                .FirstOrDefaultAsync();

            return pictureUrl;
        }

        public async Task<T> GetStylistProceduresAsync<T>(string id)
        {
            var stylist = await this.stylistsRepository
                .All()
                .Where(s => s.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return stylist;
        }

        public async Task RemoveProcedureAsync(string stylistId, string procedureId)
        {
            var stylistProcedure = await this.procedureStylistsRepository
                .All()
                .FirstOrDefaultAsync(ps => ps.StylistId == stylistId && ps.ProcedureId == procedureId);

            this.procedureStylistsRepository.Delete(stylistProcedure);
            await this.procedureStylistsRepository.SaveChangesAsync();
        }

        public async Task<bool> AddProcedureToStylistAsync(string id, string procedureId)
        {
            var isAlreadyAdded = await this.procedureStylistsRepository
                .All()
                .AnyAsync(ps => ps.StylistId == id && ps.ProcedureId == procedureId);

            if (isAlreadyAdded)
            {
                return false;
            }

            var procedureStylist = new ProcedureStylist()
            {
                StylistId = id,
                ProcedureId = procedureId,
            };

            await this.procedureStylistsRepository.AddAsync(procedureStylist);
            await this.procedureStylistsRepository.SaveChangesAsync();

            return true;
        }

        public async Task RemoveAllProceduresAsync(string id)
        {
            var procedures = await this.procedureStylistsRepository
                .All()
                .Where(ps => ps.StylistId == id)
                .ToListAsync();

            foreach (var procedure in procedures)
            {
                this.procedureStylistsRepository.Delete(procedure);
            }

            await this.procedureStylistsRepository.SaveChangesAsync();
        }

        private async Task<string> GetStylistRoleIdAsync()
        {
            return await this.rolesRepository
                .All()
                .Where(r => r.Name == GlobalConstants.StylistRoleName)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
        }

        private async Task AddProcedureToStylistByCreatingStylistAsync(string id, string category)
        {
            var procedures = await this.proceduresRepository
                            .All()
                            .Where(p => p.CategoryId == category)
                            .Select(p => p.Id)
                            .ToListAsync();

            foreach (var procedure in procedures)
            {
                var procedureStylist = new ProcedureStylist()
                {
                    StylistId = id,
                    ProcedureId = procedure,
                };

                await this.procedureStylistsRepository.AddAsync(procedureStylist);
            }
        }
    }
}
