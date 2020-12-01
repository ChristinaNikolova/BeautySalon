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

    public class TypeCardSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.TypeCards.Any())
            {
                var typeCardsData = JsonConvert
                    .DeserializeObject<List<TypeCardDto>>(File.ReadAllText(GlobalConstants.TypeCardSeederPath))
                    .ToList();

                List<TypeCard> typeCards = new List<TypeCard>();

                foreach (var currentTypeCard in typeCardsData)
                {
                    var typeCard = new TypeCard()
                    {
                        Name = currentTypeCard.Name,
                        Price = currentTypeCard.Price,
                    };

                    typeCards.Add(typeCard);
                }

                await dbContext.TypeCards.AddRangeAsync(typeCards);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
