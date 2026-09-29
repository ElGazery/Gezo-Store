using Gezo.Application.Interfaces.Repositories;
using Gezo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly StoreDbContext _context;
        private readonly DbSet<T> _dbset;
        public GenericRepository(StoreDbContext storeDbContext)
        {
            _context = storeDbContext;
            _dbset = _context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
             _dbset.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }


        public async Task<T?> GetbyIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
             _dbset.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
