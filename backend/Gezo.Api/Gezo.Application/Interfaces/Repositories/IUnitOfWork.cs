using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> GenericRepository<T>() where T : class;
        Task<int> SaveChangesAsync();

    }
}
