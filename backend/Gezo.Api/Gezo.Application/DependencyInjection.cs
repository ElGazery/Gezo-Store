using FluentValidation;
using Gezo.Application.Behaviors;
using Gezo.Application.Common.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application
{
    public static class DependencyInjection 
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(
                cfg => { }, typeof(ProductProfile).Assembly);
            services.AddMediatR(typeof(DependencyInjection).Assembly);

            services.AddValidatorsFromAssembly(typeof(ValidationBehavior<,>).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    }
}
