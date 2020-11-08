namespace BeautySalon.Services.Data.JobTypes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class JobTypesService : IJobTypesService
    {
        private readonly IRepository<JobType> jobTypesRepository;

        public JobTypesService(IRepository<JobType> jobTypesRepository)
        {
            this.jobTypesRepository = jobTypesRepository;
        }

        public async Task<string> CreateAsync(string name)
        {
            var jobType = new JobType()
            {
                Name = name,
            };

            await this.jobTypesRepository.AddAsync(jobType);
            await this.jobTypesRepository.SaveChangesAsync();

            return jobType.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var jobType = await this.GetByIdAsync(id);

            jobType.IsDeleted = true;

            this.jobTypesRepository.Update(jobType);
            await this.jobTypesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string name, string id)
        {
            var jobType = await this.GetByIdAsync(id);

            jobType.Name = name;

            this.jobTypesRepository.Update(jobType);
            await this.jobTypesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetAllAsSelectListItemAsync()
        {
            var jobTypes = await this.jobTypesRepository
               .All()
               .Select(c => new SelectListItem()
               {
                   Value = c.Id,
                   Text = c.Name,
               })
               .ToListAsync();

            return jobTypes;
        }

        public async Task<JobType> GetByIdAsync(string id)
        {
            return await this.jobTypesRepository
               .All()
               .FirstOrDefaultAsync(jt => jt.Id == id);
        }

        public async Task<JobType> GetByNameAsync(string name)
        {
            return await this.jobTypesRepository
               .All()
               .FirstOrDefaultAsync(jt => jt.Name == name);
        }
    }
}
