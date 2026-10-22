//using Gezo.Infrastructure.Identity;
//using Gezo.Infrastructure.Persistence;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;

//namespace Gezo.Infrastructure.Persistence.Seeding;

//public static class DatabaseSeeder
//{
//    public static async Task SeedAsync(IServiceProvider services)
//    {
//        await using var scope = services.CreateAsyncScope();

//        var sp = scope.ServiceProvider;

//        var context = sp.GetRequiredService<StoreDbContext>();

//        await context.Database.MigrateAsync();

//        var roleManager =
//            sp.GetRequiredService<RoleManager<IdentityRole>>();

//        await IdentitySeeder.SeedRolesAsync(roleManager);

//        await CatalogSeeder.SeedAsync(context);
//    }
//}