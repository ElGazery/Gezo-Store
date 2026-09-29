using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {

        public Task<T?> GetbyIdAsync(int id);
        public Task<List<T>> GetAllAsync();
        public Task AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
    }
}
