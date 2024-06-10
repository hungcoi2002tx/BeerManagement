using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer;

public static class DataLayerRegister
{
    public static IServiceCollection AddServicesDataLayer(this IServiceCollection services, IConfiguration configration)
    {
        string beerManagementConfiguration = configration.GetConnectionString("MyDatabase");
        services.AddDbContext<BeerManagementContext>(
            opt => opt.UseSqlServer(beerManagementConfiguration));

        return services;
    }
}

