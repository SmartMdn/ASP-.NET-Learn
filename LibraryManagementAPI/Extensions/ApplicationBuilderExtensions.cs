using LibraryManagement.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        try
        {
            var context = services.GetRequiredService<LibraryContext>();
            context.Database.Migrate();
            logger.LogInformation("Database migrations applied successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database");
        }
        
        return app;
    }
    
    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Management API v1");
            options.RoutePrefix = string.Empty;
        });
        
        return app;
    }
}
