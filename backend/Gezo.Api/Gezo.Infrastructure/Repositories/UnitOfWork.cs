using Gezo.Application.Interfaces.Repositories;
using Gezo.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Infrastructure.Repositories
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly StoreDbContext _context;
        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> GenericRepository<T>() where T: class
        {
            return new GenericRepository<T>(_context);
        }

      
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        
    }
}
