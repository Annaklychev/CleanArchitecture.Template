using CleanArchitecture.Migrations;

namespace CleanArchitecture.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseApplicationServices(this WebApplication app)
        {
            app.UseExceptionHandler();

            ApplyMigrations(app);

            if (app.Environment.IsDevelopment())
            {
                app.UseCors();
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            return app;
        }

        private static void ApplyMigrations(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            DatabaseMigrator.Migrate(connectionString);
        }
    }
}
