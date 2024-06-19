using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Business;

public static class BusinessLayerRegister
{
    public static IServiceCollection AddServicesBusinessLayer(this IServiceCollection services)
    {
        var assembly = Assembly.GetAssembly(typeof(BusinessLayerRegister));
        var classes = assembly.ExportedTypes.Where(a => !a.Name.StartsWith("I") && a.Name.EndsWith("Service"));

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
