using Microsoft.EntityFrameworkCore;
using NearBite.Api.Data;
using NearBite.Api.Domain;
using NearBite.Api.Domain.Enums;
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
        var listings = new List<Listing>
        {
            new Listing { Name = "Fort Cafe",        Description = "Cozy cafe near the beach",   Cuisine = "Sri Lankan", PriceRange = 2, City = "Negombo", LiveStatus = "Open",   Latitude = 7.2094, Longitude = 79.8358, IsVeg = false, SubmissionStatus = SubmissionStatus.Approved, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Listing { Name = "Green Leaf Kottu", Description = "Best kottu in town",         Cuisine = "Sri Lankan", PriceRange = 1, City = "Negombo", LiveStatus = "Open",   Latitude = 7.2110, Longitude = 79.8380, IsVeg = false, SubmissionStatus = SubmissionStatus.Approved, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Listing { Name = "Sunset Hoppers",   Description = "Traditional hoppers spot",   Cuisine = "Sri Lankan", PriceRange = 1, City = "Negombo", LiveStatus = "Closed", Latitude = 7.2050, Longitude = 79.8400, IsVeg = true,  SubmissionStatus = SubmissionStatus.Approved, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Listing { Name = "The Curry House",  Description = "Rice & curry, home style",   Cuisine = "Sri Lankan", PriceRange = 2, City = "Negombo", LiveStatus = "Busy",   Latitude = 7.2080, Longitude = 79.8410, IsVeg = false, SubmissionStatus = SubmissionStatus.Approved, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Listing { Name = "Bella Italia",     Description = "Wood-fired pizza",           Cuisine = "Italian",    PriceRange = 3, City = "Negombo", LiveStatus = "Open",   Latitude = 7.2130, Longitude = 79.8340, IsVeg = false, SubmissionStatus = SubmissionStatus.Approved, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        db.Listings.AddRange(listings);
        db.SaveChanges();

        var menuItems = new List<MenuItem>
        {
            // Fort Cafe
            new MenuItem { ListingId = listings[0].Id, Name = "English Breakfast", Price = 950, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { ListingId = listings[0].Id, Name = "Avocado Toast",     Price = 750, IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { ListingId = listings[0].Id, Name = "Cold Coffee",       Price = 450, IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            // Green Leaf Kottu
            new MenuItem { ListingId = listings[1].Id, Name = "Chicken Kottu",     Price = 650, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { ListingId = listings[1].Id, Name = "Cheese Kottu",      Price = 750, IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { ListingId = listings[1].Id, Name = "Egg Kottu",         Price = 550, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { ListingId = listings[1].Id, Name = "Vegetable Kottu",   Price = 500, IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            // Sunset Hoppers
            new MenuItem { ListingId = listings[2].Id, Name = "Plain Hopper",         Price = 60,  IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { ListingId = listings[2].Id, Name = "Egg Hopper",           Price = 90,  IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { ListingId = listings[2].Id, Name = "String Hopper Plate",  Price = 250, IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            // The Curry House
            new MenuItem { ListingId = listings[3].Id, Name = "Rice and Curry",     Price = 500, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { ListingId = listings[3].Id, Name = "Veg Rice and Curry", Price = 400, IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { ListingId = listings[3].Id, Name = "Fish Curry",         Price = 650, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            // Bella Italia
            new MenuItem { ListingId = listings[4].Id, Name = "Margherita Pizza",    Price = 1200, IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { ListingId = listings[4].Id, Name = "Pepperoni Pizza",     Price = 1500, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { ListingId = listings[4].Id, Name = "Spaghetti Carbonara", Price = 1100, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        db.MenuItems.AddRange(menuItems);
        db.SaveChanges();
    }
}

app.Run();