using Gezo.Application.Specifications;
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

        public Task<int> CountWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken );
        public Task<List<T>> GetAllWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken );
        public Task<T?> GetFirstOrDefaultWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken);
    }
}
