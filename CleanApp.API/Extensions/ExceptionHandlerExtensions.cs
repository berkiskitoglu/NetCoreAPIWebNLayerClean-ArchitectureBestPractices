using CleanApp.API.ExceptionHandler;
using Microsoft.AspNetCore.Diagnostics;

namespace CleanApp.API.Extensions;

public static class ExceptionHandlerExtensions 
{
        public static IServiceCollection UseGlobalExceptionHandlerExt(this IServiceCollection services)
        {
            services.AddExceptionHandler<CriticalExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
    }
}

