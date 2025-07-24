using System;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<Application.ILiteDbRepo, Infrastructure.LiteDbRepo>();

        return services;
    }
}
