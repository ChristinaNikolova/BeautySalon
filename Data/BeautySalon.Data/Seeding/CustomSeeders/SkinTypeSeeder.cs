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

    public class SkinTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.SkinTypes.Any())
            {
                var skinTypesData = JsonConvert
                    .DeserializeObject<List<SkinTypeDto>>(File.ReadAllText(@"../../Data/BeautySalon.Data/Seeding/Data/SkinTypes.json")).ToList();

                List<SkinType> skinTypes = new List<SkinType>();

                foreach (var currentSkinTypeData in skinTypesData)
                {
                    var skinType = new SkinType()
                    {
                        Name = currentSkinTypeData.Name,
                        Description = currentSkinTypeData.Description,
                    };

                    skinTypes.Add(skinType);
                }

                await dbContext.SkinTypes.AddRangeAsync(skinTypes);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
