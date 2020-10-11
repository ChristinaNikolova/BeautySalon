namespace BeautySalon.Data.Seeding.CustomSeeders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Models.Enums;
    using BeautySalon.Data.Seeding.Dtos;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    public class StylistSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!dbContext.Users.Any())
            {
                var stylistsData = JsonConvert
                    .DeserializeObject<List<StylistDto>>(File.ReadAllText(GlobalConstants.StylistSeederPath)).ToList();

                List<ApplicationUser> stylists = new List<ApplicationUser>();
                List<ProcedureStylist> procedureStylists = new List<ProcedureStylist>();

                foreach (var currentStylistData in stylistsData)
                {
                    var category = await dbContext.Categories
                        .FirstOrDefaultAsync(c => c.Name == currentStylistData.Category);

                    var jobType = await dbContext.JobTypes
                        .FirstOrDefaultAsync(jb => jb.Name == currentStylistData.JobType);

                    var stylist = new ApplicationUser()
                    {
                        FirstName = currentStylistData.FirstName,
                        LastName = currentStylistData.LastName,
                        Picture = currentStylistData.Picture,
                        Gender = Enum.Parse<Gender>(currentStylistData.Gender),
                        CategoryId = category.Id,
                        JobTypeId = jobType.Id,
                        UserName = currentStylistData.UserName,
                        PasswordHash = GlobalConstants.SystemPasswordHashed,
                        Email = currentStylistData.Email,
                        EmailConfirmed = true,
                        Description = currentStylistData.Description,
                    };

                    var procedures = dbContext.Procedures
                        .Where(p => p.Category.Name == currentStylistData.Category)
                        .ToList();

                    foreach (var procedure in procedures)
                    {
                        var procedureStylist = new ProcedureStylist()
                        {
                            ProcedureId = procedure.Id,
                            StylistId = stylist.Id,
                        };

                        procedureStylists.Add(procedureStylist);
                    }

                    stylists.Add(stylist);
                }

                await dbContext.ProcedureStylists.AddRangeAsync(procedureStylists);
                await dbContext.Users.AddRangeAsync(stylists);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
