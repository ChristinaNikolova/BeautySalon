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
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class ArticleSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Articles.Any())
            {
                var articlesData = JsonConvert
                    .DeserializeObject<List<ArticleDto>>(File.ReadAllText(GlobalConstants.ArticleSeederPath)).ToList();

                List<Article> articles = new List<Article>();

                foreach (var currentArticleData in articlesData)
                {
                    var category = await dbContext.Categories
                        .FirstOrDefaultAsync(c => c.Name == currentArticleData.Category);

                    var stylist = await dbContext.Users
                        .FirstOrDefaultAsync(u => u.FirstName + " " + u.LastName == currentArticleData.Stylist);

                    var article = new Article()
                    {
                        Title = currentArticleData.Title,
                        Content = currentArticleData.Content,
                        Picture = currentArticleData.Picture,
                        CategoryId = category.Id,
                        StylistId = stylist.Id,
                    };

                    articles.Add(article);
                }

                await dbContext.Articles.AddRangeAsync(articles);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
