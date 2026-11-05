using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Exceptions;
using SpooLLedger.Infrastructure;
using SpooLLedger.Infrastructure.Data.DatabaseContext;
using SpooLLedger.Infrastructure.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWindowsService();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<DataContext>(opt =>
{
   var configured = builder.Configuration.GetConnectionString("DefaultConnection")
      ?? "Data Source=stock.db";
   var sqlite = new SqliteConnectionStringBuilder(configured);
   if (!Path.IsPathRooted(sqlite.DataSource))
   {
      sqlite.DataSource = Path.Combine(builder.Environment.ContentRootPath, sqlite.DataSource);
   }

   opt.UseSqlite(sqlite.ToString());
});

builder.Services.AddCors();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync();
        await DataSeeder.SeedFilamentTypes(context);
        await DataSeeder.SeedFilamentColors(context);
        await DataSeeder.SeedFilamentStock(context);
        await DataSeeder.SeedPricing(context);
        await DataSeeder.SeedCustomers(context);
        await DataSeeder.SeedItems(context);
        await DataSeeder.SeedOrders(context);
        await DataSeeder.SeedOrderItems(context);
        await services.GetRequiredService<IOrderRepository>().ApplyOutstandingFilamentAsync(CancellationToken.None);
    }
    catch
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError("An error occurend during migration");
    }
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseCors(x=> x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
