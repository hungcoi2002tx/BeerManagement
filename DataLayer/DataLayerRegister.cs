using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer;

public static class DataLayerRegister
{
    public static IServiceCollection AddServicesDbcontext(this IServiceCollection services, IConfiguration configration)
    {
        string beerManagementConfiguration = configration.GetConnectionString("MyDatabase") ?? throw new Exception(" Can not get connecttion string ");
        services.AddDbContext<DbContext, BeerManagementContext>(
            opt => opt.UseSqlServer(beerManagementConfiguration));

        return services;
    }

    public static IServiceCollection AddServicesDataLayer(this IServiceCollection services)
    {
        var assembly = Assembly.GetAssembly(typeof(DataLayerRegister));
        var classes = assembly.ExportedTypes
           .Where(a => !a.Name.StartsWith("I") && a.Name.EndsWith("Repository") && (a.Name != "Repository"));
        foreach (Type implement in classes)
        {
            foreach (var @interface in implement.GetInterfaces())
            {
                services.AddScoped(@interface, implement);
            }
        }

        return services;
    }
}

