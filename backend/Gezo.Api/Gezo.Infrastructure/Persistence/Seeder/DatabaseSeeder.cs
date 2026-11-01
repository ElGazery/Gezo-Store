using Gezo.Domain.Entities;
using Gezo.Infrastructure.Persistence.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Gezo.Infrastructure.Persistence.Seeding;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        IServiceProvider services,
        CancellationToken ct = default)
    {
        await using var scope = services.CreateAsyncScope();
        var sp = scope.ServiceProvider;

        var context = sp.GetRequiredService<StoreDbContext>();
        await context.Database.MigrateAsync(ct);

        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
        await IdentitySeeder.SeedRolesAsync(roleManager);

        var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
        await IdentitySeeder.SeedAdminUserAsync(userManager);

        await CatalogSeeder.SeedAsync(context, ct);
    }
}
