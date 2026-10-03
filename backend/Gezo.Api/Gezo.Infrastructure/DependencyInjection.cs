using Gezo.Application.Interfaces.Repositories;
using Gezo.Infrastructure.Persistence;
using Gezo.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("myConnection")));
            services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

            
            return services;
        }
    }
}
