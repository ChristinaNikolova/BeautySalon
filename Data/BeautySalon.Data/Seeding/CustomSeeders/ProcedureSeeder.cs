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

    public class ProcedureSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Procedures.Any())
            {
                var proceduresData = JsonConvert
                    .DeserializeObject<List<ProcedureDto>>(File.ReadAllText(GlobalConstants.ProcedureSeederPath)).ToList();

                List<Procedure> procedures = new List<Procedure>();
                List<ProcedureProduct> procedureProducts = new List<ProcedureProduct>();
                List<SkinProblemProcedure> skinProblemProcedures = new List<SkinProblemProcedure>();

                foreach (var currentProcedureData in proceduresData)
                {
                    var procedure = new Procedure()
                    {
                        Name = currentProcedureData.Name,
                        Description = currentProcedureData.Description,
                        Price = currentProcedureData.Price,
                    };

                    var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == currentProcedureData.Category);

                    if (category != null)
                    {
                        procedure.CategoryId = category.Id;
                    }

                    var skinType = await dbContext.SkinTypes.FirstOrDefaultAsync(s => s.Name == currentProcedureData.SkinType);

                    if (skinType != null)
                    {
                        procedure.SkinTypeId = skinType.Id;
                    }

                    if (currentProcedureData.SkinType != null)
                    {
                        foreach (var currentProblem in currentProcedureData.SkinProblems)
                        {
                            var skinProblem = await dbContext.SkinProblems.FirstOrDefaultAsync(sp => sp.Name == currentProblem);

                            var skinProblemProcedure = new SkinProblemProcedure()
                            {
                                SkinProblemId = skinProblem.Id,
                                ProcedureId = procedure.Id,
                            };

                            skinProblemProcedures.Add(skinProblemProcedure);
                        }
                    }

                    if (currentProcedureData.Products != null)
                    {
                        foreach (var currentProduct in currentProcedureData.Products)
                        {
                            var product = await dbContext.Products.FirstOrDefaultAsync(sp => sp.Name == currentProduct);

                            var procedureProduct = new ProcedureProduct()
                            {
                                ProductId = product.Id,
                                ProcedureId = procedure.Id,
                            };

                            procedureProducts.Add(procedureProduct);
                        }
                    }

                    procedures.Add(procedure);
                }

                await dbContext.ProcedureProducts.AddRangeAsync(procedureProducts);
                await dbContext.SkinProblemProcedures.AddRangeAsync(skinProblemProcedures);
                await dbContext.Procedures.AddRangeAsync(procedures);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
