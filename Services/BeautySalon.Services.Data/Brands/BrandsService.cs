namespace BeautySalon.Services.Data.Brands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class BrandsService : IBrandsService
    {
        private readonly IRepository<Brand> brandsRepository;

        public BrandsService(IRepository<Brand> brandsRepository)
        {
            this.brandsRepository = brandsRepository;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllAsSelectListItemAsync()
        {
            var brands = await this.brandsRepository
                .All()
                .Select(c => new SelectListItem()
                {
                    Value = c.Id,
                    Text = c.Name,
                })
                .ToListAsync();

            return brands;
        }
    }
}
