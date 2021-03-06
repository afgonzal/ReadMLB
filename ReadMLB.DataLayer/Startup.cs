﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadMLB.DataLayer.Context;
using ReadMLB.DataLayer.Repositories;

namespace ReadMLB.DataLayer
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<FranchiseContext>(options => options.UseSqlServer(config.GetConnectionString("Franchise")), ServiceLifetime.Transient);

            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
