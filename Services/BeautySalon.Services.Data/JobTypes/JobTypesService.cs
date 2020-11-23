namespace BeautySalon.Services.Data.JobTypes
{
    using System.Collections.Generic;
    using System.Linq;
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
    }
}
