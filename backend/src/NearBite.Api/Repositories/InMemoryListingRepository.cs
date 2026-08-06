using NearBite.Api.Domain;

namespace NearBite.Api.Repositories;

public class InMemoryListingRepository : IListingRepository
{
    private readonly List<Listing> _listings;
    private int _nextId;

    public InMemoryListingRepository()
    {
        _listings = new List<Listing>
        {
            new Listing { Id = 1, Name = "Fort Cafe",        Description = "Cozy cafe near the beach",   Cuisine = "Sri Lankan", PriceRange = 2, City = "Negombo", LiveStatus = "Open",   Latitude = 7.2094, Longitude = 79.8358, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Listing { Id = 2, Name = "Green Leaf Kottu", Description = "Best kottu in town",         Cuisine = "Sri Lankan", PriceRange = 1, City = "Negombo", LiveStatus = "Open",   Latitude = 7.2110, Longitude = 79.8380, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Listing { Id = 3, Name = "Sunset Hoppers",   Description = "Traditional hoppers spot",   Cuisine = "Sri Lankan", PriceRange = 1, City = "Negombo", LiveStatus = "Closed", Latitude = 7.2050, Longitude = 79.8400, IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Listing { Id = 4, Name = "The Curry House",  Description = "Rice & curry, home style",   Cuisine = "Sri Lankan", PriceRange = 2, City = "Negombo", LiveStatus = "Busy",   Latitude = 7.2080, Longitude = 79.8410, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Listing { Id = 5, Name = "Bella Italia",     Description = "Wood-fired pizza",           Cuisine = "Italian",    PriceRange = 3, City = "Negombo", LiveStatus = "Open",   Latitude = 7.2130, Longitude = 79.8340, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        _nextId = _listings.Max(l => l.Id) + 1;
    }

    public IEnumerable<Listing> GetAll()
    {
        return _listings;
    }

    public Listing? GetById(int id)
    {
        return _listings.FirstOrDefault(l => l.Id == id);
    }

    public void Add(Listing listing)
    {
        listing.Id = _nextId++;
        listing.CreatedAt = DateTime.UtcNow;
        listing.UpdatedAt = DateTime.UtcNow;
        _listings.Add(listing);
    }

    public void Update(Listing listing)
    {
        var existing = _listings.FirstOrDefault(l => l.Id == listing.Id);
        if (existing == null)
        {
            return;
        }

        existing.Name = listing.Name;
        existing.Description = listing.Description;
        existing.Cuisine = listing.Cuisine;
        existing.PriceRange = listing.PriceRange;
        existing.City = listing.City;
        existing.LiveStatus = listing.LiveStatus;
        existing.Latitude = listing.Latitude;
        existing.Longitude = listing.Longitude;
        existing.IsVeg = listing.IsVeg;
        existing.UpdatedAt = DateTime.UtcNow;
    }

    public void Delete(int id)
    {
        var existing = _listings.FirstOrDefault(l => l.Id == id);
        if (existing == null)
        {
            return;
        }

        _listings.Remove(existing);
    }
}