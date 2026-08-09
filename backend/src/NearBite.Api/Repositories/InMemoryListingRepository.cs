using NearBite.Api.Domain;

namespace NearBite.Api.Repositories;

public class InMemoryListingRepository : IListingRepository
{
    private readonly List<Listing> _listings;
    private readonly List<MenuItem> _menuItems;
    private int _nextListingId;
    private int _nextMenuItemId;

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

        _menuItems = new List<MenuItem>
        {
            // Fort Cafe (Listing 1)
            new MenuItem { Id = 1, ListingId = 1, Name = "English Breakfast",    Price = 950,  IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { Id = 2, ListingId = 1, Name = "Avocado Toast",        Price = 750,  IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { Id = 3, ListingId = 1, Name = "Cold Coffee",          Price = 450,  IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            // Green Leaf Kottu (Listing 2)
            new MenuItem { Id = 4, ListingId = 2, Name = "Chicken Kottu",        Price = 650,  IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { Id = 5, ListingId = 2, Name = "Cheese Kottu",         Price = 750,  IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { Id = 6, ListingId = 2, Name = "Egg Kottu",            Price = 550,  IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { Id = 7, ListingId = 2, Name = "Vegetable Kottu",      Price = 500,  IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            // Sunset Hoppers (Listing 3)
            new MenuItem { Id = 8,  ListingId = 3, Name = "Plain Hopper",        Price = 60,   IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { Id = 9,  ListingId = 3, Name = "Egg Hopper",          Price = 90,   IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { Id = 10, ListingId = 3, Name = "String Hopper Plate", Price = 250,  IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            // The Curry House (Listing 4)
            new MenuItem { Id = 11, ListingId = 4, Name = "Rice and Curry",      Price = 500,  IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { Id = 12, ListingId = 4, Name = "Veg Rice and Curry",  Price = 400,  IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { Id = 13, ListingId = 4, Name = "Fish Curry",          Price = 650,  IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            // Bella Italia (Listing 5)
            new MenuItem { Id = 14, ListingId = 5, Name = "Margherita Pizza",    Price = 1200, IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { Id = 15, ListingId = 5, Name = "Pepperoni Pizza",     Price = 1500, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new MenuItem { Id = 16, ListingId = 5, Name = "Spaghetti Carbonara", Price = 1100, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        _nextListingId = _listings.Max(l => l.Id) + 1;
        _nextMenuItemId = _menuItems.Max(m => m.Id) + 1;
    }

    // -----------------------------
    // Listing methods
    // -----------------------------

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
        listing.Id = _nextListingId++;
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
        _menuItems.RemoveAll(m => m.ListingId == id);
    }

    // -----------------------------
    // Menu item methods
    // -----------------------------

    public IEnumerable<MenuItem> GetMenuItemsForListing(int listingId)
    {
        var result = new List<MenuItem>();
        foreach (var item in _menuItems)
        {
            if (item.ListingId == listingId)
            {
                result.Add(item);
            }
        }
        return result;
    }

    public MenuItem? GetMenuItemById(int menuItemId)
    {
        return _menuItems.FirstOrDefault(m => m.Id == menuItemId);
    }

    public void AddMenuItem(MenuItem menuItem)
    {
        menuItem.Id = _nextMenuItemId++;
        menuItem.CreatedAt = DateTime.UtcNow;
        menuItem.UpdatedAt = DateTime.UtcNow;
        _menuItems.Add(menuItem);
    }

    public void UpdateMenuItem(MenuItem menuItem)
    {
        var existing = _menuItems.FirstOrDefault(m => m.Id == menuItem.Id);
        if (existing == null)
        {
            return;
        }

        existing.Name = menuItem.Name;
        existing.Price = menuItem.Price;
        existing.IsVeg = menuItem.IsVeg;
        existing.PhotoUrl = menuItem.PhotoUrl;
        existing.UpdatedAt = DateTime.UtcNow;
    }

    public void DeleteMenuItem(int menuItemId)
    {
        var existing = _menuItems.FirstOrDefault(m => m.Id == menuItemId);
        if (existing == null)
        {
            return;
        }

        _menuItems.Remove(existing);
    }
}