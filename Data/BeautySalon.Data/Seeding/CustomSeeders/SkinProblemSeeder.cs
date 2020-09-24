namespace BeautySalon.Data.Seeding.CustomSeeders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Data.Seeding.Dtos;
    using Newtonsoft.Json;

    public class SkinProblemSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.SkinProblems.Any())
            {
                var skinProblemData = JsonConvert
                    .DeserializeObject<List<SkinProblemDto>>(File.ReadAllText(@"../../Data/BeautySalon.Data/Seeding/Data/SkinProblems.json")).ToList();

                List<SkinProblem> skinProblems = new List<SkinProblem>();

                foreach (var currentSkinProblemData in skinProblemData)
                {
                    var skinProblem = new SkinProblem()
                    {
                        Name = currentSkinProblemData.Name,
                        Description = currentSkinProblemData.Description,
                    };

                    skinProblems.Add(skinProblem);
                }

                await dbContext.SkinProblems.AddRangeAsync(skinProblems);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
