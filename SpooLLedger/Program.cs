using Microsoft.EntityFrameworkCore;
using SpooLLedger.Infrastructure.Data.DatabaseContext;
using SpooLLedger.Infrastructure.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(opt =>
{
   opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")); 
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
        await DataSeeder.SeedFilamentStock(context);
    }
    catch
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError("An error occurend during migration");
    }
}

// Configure the HTTP request pipeline.

app.UseCors(x=> x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
