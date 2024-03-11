using E_Commerce.Core.Interfaces;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure
{
    public static class InfrastructureRegisteration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services,IConfiguration configuration) 
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<ICategoryRepository, CategoryRepository>();
            //services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
