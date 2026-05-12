using System.Reflection;
using App.Repositories;
using App.Repositories.Products;
using App.Services.Products;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace App.Services.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }

}

