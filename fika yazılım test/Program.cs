using fika_yazılım_test;
using fika_yazılım_test.Data;
using fika_yazılım_test.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc();

builder.Services.AddDbContext<FikaTestDbContext>(options => options.UseInMemoryDatabase("fikadb"));
builder.Services.AddTransient<IAppVersionService, AppVersionService>();


Task.Run(async () =>
{
    await Task.Delay(1000);

    using (var scope = builder.Services.BuildServiceProvider().CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            await SeedData.InitialAsync(services);
        }
        catch (Exception ex)
        {
            var Logger = services.GetRequiredService<ILogger<Program>>();
            Logger.LogError(ex, "Error in SeedData.");
        }
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapControllers();
app.Run();
