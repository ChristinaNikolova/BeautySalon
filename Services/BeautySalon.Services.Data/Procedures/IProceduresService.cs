﻿namespace BeautySalon.Services.Data.Procedures
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Models;

    public interface IProceduresService
    {
        Task<string> CreateAsync(string name, string description, decimal price, string categoryName, string skinTypeName);

        Task EditAsync(string name, string description, decimal price, string categoryName, string skinTypeName, string id);

        Task DeleteAsync(string id);

        public Task<Procedure> GetByIdAsync(string id);

        Task<IEnumerable<T>> GetAllByCategoryAsync<T>(string categoryId);

        Task<T> GetProcedureDetailsAsync<T>(string id);
    }
}
