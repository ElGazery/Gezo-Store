using Gezo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Gezo.Infrastructure.Persistence.Seeding;
// it's need a review 
public static class CatalogSeeder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task SeedAsync(
        StoreDbContext context,
        CancellationToken ct = default)
    {
        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await context.Database.BeginTransactionAsync(ct);

            await SeedAsync<Brand>(context, "brands.json", ct);
            await SeedAsync<Category>(context, "categories.json", ct);
            await SeedAsync<Product>(context, "products.json", ct);
            await SeedAsync<ProductImage>(context, "product-images.json", ct);

            await tx.CommitAsync(ct);
        });
    }

    private static async Task SeedAsync<T>(
        StoreDbContext context,
        string fileName,
        CancellationToken ct) where T : class
    {
        if (await context.Set<T>().AnyAsync(ct))
            return; // already seeded

        var path = Path.Combine(
            AppContext.BaseDirectory, "Persistence", "Data", fileName);

        var json = await File.ReadAllTextAsync(path, ct);
        var items = JsonSerializer.Deserialize<List<T>>(json, JsonOptions)
                    ?? throw new InvalidOperationException($"{fileName} is empty or invalid.");

        var entityType = context.Model.FindEntityType(typeof(T))!;
        var table = entityType.GetTableName();
        var schema = entityType.GetSchema() ?? "dbo";

        await context.Set<T>().AddRangeAsync(items, ct);

        await context.Database.ExecuteSqlRawAsync(
            $"SET IDENTITY_INSERT [{schema}].[{table}] ON", ct);
        await context.SaveChangesAsync(ct);
        await context.Database.ExecuteSqlRawAsync(
            $"SET IDENTITY_INSERT [{schema}].[{table}] OFF", ct);
    }
}