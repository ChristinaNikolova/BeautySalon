﻿namespace BeautySalon.Services.Data.Stylists
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;

    public interface IStylistsService
    {
        Task<ApplicationUser> UpdateStylistProfileAsync(string id, string categoryName, string jobTypeName, string descripion);

        Task<ApplicationUser> GetByIdAsync(string id);

        Task DeleteAsync(string id);
    }
}
