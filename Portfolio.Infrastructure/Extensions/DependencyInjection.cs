using Microsoft.Extensions.DependencyInjection;
using Portfolio.Domain.Interfaces;
using Portfolio.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<Iuser,UserRepository>();
            return services;
        }
    }
}
