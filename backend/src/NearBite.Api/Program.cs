using Microsoft.EntityFrameworkCore;
using NearBite.Api.Data;
using NearBite.Api.Domain;
using NearBite.Api.Repositories;
using NearBite.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register application services (DI)
builder.Services.AddScoped<IListingRepository, EfListingRepository>();
builder.Services.AddScoped<IListingService, ListingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Seed the database on first run (dev only)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Listings.Any())
    {
        // (seed block from Step 3 — leave as is)
    }
}

app.Run();