using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AssetManagementService.Infrastructure.Data;

public static class MigrationManager
{
    public static IHost ApplyMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        var dbContexts = new List<DbContext>
        {
            services.GetRequiredService<AssetManagementDbContext>(),
        };

        foreach (var context in dbContexts)
        {
            context.Database.Migrate();
        }

        return host;
    }
}