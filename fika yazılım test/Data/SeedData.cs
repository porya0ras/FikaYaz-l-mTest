namespace fika_yazılım_test.Data;

public static class SeedData
{


    public static async Task InitialAsync(IServiceProvider services)
    {
        await AddTestData(services.GetRequiredService<FikaTestDbContext>());
    }

    public static async Task AddTestData(
        FikaTestDbContext context
    )
    {
        if (!context.Öğrencilar.Any())
        {




           
        }
    }
}
