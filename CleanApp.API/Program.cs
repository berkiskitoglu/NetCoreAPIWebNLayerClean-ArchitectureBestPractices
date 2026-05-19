using App.Application.Extensions;
using App.Bus;
using App.Persistence.Extensions;
using CleanApp.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllerWithFiltersExt()
    .AddSwaggerGenExt()
    .UseGlobalExceptionHandlerExt()
    .AddCachingExt()
    .AddApiConfigurationExt();

builder.Services
    .AddRepositories(builder.Configuration)
    .AddServices(builder.Configuration)
    .AddBusExt(builder.Configuration);

var app = builder.Build();

app.UseConfigurationPipelineExt();

app.MapControllers();

app.Run();
