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

    public class BrandSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Brands.Any())
            {
                var brandsData = JsonConvert
                    .DeserializeObject<List<BrandDto>>(File.ReadAllText(GlobalConstants.BrandSeederPath)).ToList();

                List<Brand> brands = new List<Brand>();

                foreach (var currentBrandData in brandsData)
                {
                    var brand = new Brand()
                    {
                        Name = currentBrandData.Name,
                        Description = currentBrandData.Description,
                        Logo = currentBrandData.Logo,
                    };

                    brands.Add(brand);
                }

                await dbContext.Brands.AddRangeAsync(brands);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
