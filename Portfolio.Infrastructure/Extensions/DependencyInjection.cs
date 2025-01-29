using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Portfolio.Application.Business;
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
            #region User
            services.AddTransient<Iuser, UserBusiness>();
            services.AddTransient<IChangePassword, AccountRepository>();
            services.AddTransient<IUserRepo, UserRepository>();
            #endregion
            return services;
        }
    }
}
