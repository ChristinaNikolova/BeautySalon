namespace BeautySalon.Data.Seeding.CustomSeeders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Seeding.Dtos;
    using Newtonsoft.Json;

    public class JobTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.JobTypes.Any())
            {
                var jobTypesData = JsonConvert
                    .DeserializeObject<List<JobTypeDto>>(File.ReadAllText(GlobalConstants.JobTypeSeederPath)).ToList();

                List<JobType> jobTypes = new List<JobType>();

                foreach (var currentJobTypeData in jobTypesData)
                {
                    var jobType = new JobType()
                    {
                        Name = currentJobTypeData.Name,
                    };

                    jobTypes.Add(jobType);
                }

                await dbContext.JobTypes.AddRangeAsync(jobTypes);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
