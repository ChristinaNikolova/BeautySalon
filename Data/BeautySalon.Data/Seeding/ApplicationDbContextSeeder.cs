namespace BeautySalon.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Seeding.CustomSeeders;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class ApplicationDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(ApplicationDbContextSeeder));
            var seeders = new List<ISeeder>
                          {
                              new RolesSeeder(),
                              new SettingsSeeder(),

                              // new BrandSeeder(),
                              // new CategorySeeder(),
                              // new JobTypeSeeder(),
                              // new SkinTypeSeeder(),
                              // new SkinProblemSeeder(),
                              // new ProductSeeder(),
                              // new ProcedureSeeder(),
                              // new StylistSeeder(),
                              // new UsersSeeder(),
                              // new UsersToRolesSeeder(),
                              // new QuizQuestionAnswerSeeder(),
                              // new ArticleSeeder(),
                              // new TypeCardSeeder(),
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}
