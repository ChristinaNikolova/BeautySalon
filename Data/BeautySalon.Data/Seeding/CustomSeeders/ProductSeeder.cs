namespace BeautySalon.Data.Seeding.CustomSeeders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;
    using BeautySalon.Data.Seeding.Dtos;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class ProductSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Products.Any())
            {
                var productsData = JsonConvert
                    .DeserializeObject<List<ProductDto>>(File.ReadAllText(@"../../Data/BeautySalon.Data/Seeding/Data/Products.json")).ToList();

                List<Product> products = new List<Product>();

                foreach (var currentProductData in productsData)
                {
                    var brand = await dbContext.Brands.FirstOrDefaultAsync(b => b.Name == currentProductData.Brand);
                    var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == currentProductData.Category);

                    var product = new Product()
                    {
                        Name = currentProductData.Name,
                        Description = currentProductData.Description,
                        Picture = currentProductData.Picture,
                        Price = currentProductData.Price,
                        Quantity = currentProductData.Quantity,
                        BrandId = brand.Id,
                        CategoryId = category.Id,
                    };

                    products.Add(product);
                }

                await dbContext.Products.AddRangeAsync(products);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
