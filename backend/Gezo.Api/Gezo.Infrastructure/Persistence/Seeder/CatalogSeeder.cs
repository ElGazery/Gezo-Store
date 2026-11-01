using System.Text.Json;
using System.Text.Json.Serialization;
using Gezo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Gezo.Infrastructure.Persistence.Seeder;

public static class CatalogSeeder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        // لو property في الـ JSON مالهاش مقابل في الـ entity يرمي error بدل ما يتجاهلها بصمت
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
    };

    public static async Task SeedAsync(
        StoreDbContext context,
        CancellationToken ct = default)
    {
        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await context.Database.BeginTransactionAsync(ct);

            // الترتيب مهم: الأب قبل الابن (Foreign Keys)
            await SeedTableAsync<Brand>(context, "brands.json", ct);
            await SeedTableAsync<Category>(context, "categories.json", ct);
            await SeedTableAsync<Product>(context, "products.json", ct);
            await SeedTableAsync<ProductImage>(context, "product_images.json", ct);

            await tx.CommitAsync(ct);
        });
    }

    private static async Task SeedTableAsync<T>(
        StoreDbContext context,
        string fileName,
        CancellationToken ct) where T : class
    {
        // IgnoreQueryFilters: لو عندك soft-delete filter ميخبيش صفوف ويعمل seed مرتين
        if (await context.Set<T>().IgnoreQueryFilters().AnyAsync(ct))
            return; // already seeded

        var path = Path.Combine(AppContext.BaseDirectory, "Persistence", "Data", fileName);

        if (!File.Exists(path))
            throw new FileNotFoundException($"Seed file not found: {path}", path);

        await using var stream = File.OpenRead(path);
        var items = await JsonSerializer.DeserializeAsync<List<T>>(stream, JsonOptions, ct)
                    ?? throw new InvalidOperationException($"{fileName} is empty or invalid.");

        var entityType = context.Model.FindEntityType(typeof(T))
                         ?? throw new InvalidOperationException($"{typeof(T).Name} is not in the model.");

        var table = entityType.GetTableName();
        var schema = entityType.GetSchema() ?? "dbo";

        // IDENTITY_INSERT مطلوب بس لو الـ Id بيتولد تلقائياً من الداتابيز
        var key = entityType.FindPrimaryKey()!.Properties.Single();
        var hasIdentity = key.ValueGenerated == ValueGenerated.OnAdd;

        context.Set<T>().AddRange(items);

#pragma warning disable EF1002 // table/schema جايين من الـ EF model مش من user input
        try
        {
            if (hasIdentity)
                await context.Database.ExecuteSqlRawAsync(
                    $"SET IDENTITY_INSERT [{schema}].[{table}] ON", ct);

            await context.SaveChangesAsync(ct);
        }
        finally
        {
            if (hasIdentity)
                await context.Database.ExecuteSqlRawAsync(
                    $"SET IDENTITY_INSERT [{schema}].[{table}] OFF", CancellationToken.None);
        }
#pragma warning restore EF1002

        context.ChangeTracker.Clear();
    }
}