using Microsoft.AspNetCore.Mvc;

namespace CleanApp.API.Extensions
{
    public static class ApiConfigurationExtensions
    {
        public static IServiceCollection AddApiConfigurationExt(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            return services;
        }
    }
}
