namespace CleanApp.API.Extensions
{
    public static class ConfigurationPipelineExtensions
    {
        public static IApplicationBuilder UseConfigurationPipelineExt(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerExt();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            return app;
        }
    }
}
