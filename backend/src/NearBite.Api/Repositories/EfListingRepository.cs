using Microsoft.EntityFrameworkCore;
using NearBite.Api.Data;
using NearBite.Api.Domain;
using NearBite.Api.Dtos;

namespace NearBite.Api.Repositories;

public class EfListingRepository : IListingRepository
{
    private readonly AppDbContext _context;

    public EfListingRepository(AppDbContext context)
    {
        _context = context;
    }

    // -----------------------------
    // Listing methods
    // -----------------------------

    public async Task<IEnumerable<Listing>> GetAllAsync()
    {
        return await _context.Listings.ToListAsync();
    }

    public async Task<IEnumerable<Listing>> GetAllAsync(ListingFilterDto filter)
    {
        var query = _context.Listings.AsQueryable();

        if (filter.Cuisine != null)
        {
            var cuisine = filter.Cuisine.ToLower();
            query = query.Where(l => l.Cuisine.ToLower() == cuisine);
        }

        if (filter.MaxPrice != null)
        {
            query = query.Where(l => l.PriceRange <= filter.MaxPrice.Value);
        }

        if (filter.IsVeg != null)
        {
            query = query.Where(l => l.IsVeg == filter.IsVeg.Value);
        }

        if (filter.Search != null)
        {
            var search = filter.Search.ToLower();
            query = query.Where(l => l.Name.ToLower().Contains(search));
        }

        // Sorting
        query = filter.SortBy switch
        {
            "price" => query.OrderBy(l => l.PriceRange),
            "name" or null or "" => query.OrderBy(l => l.Name),
            _ => query
        };

        return await query.ToListAsync();
    }

    public async Task<Listing?> GetByIdAsync(int id)
    {
        return await _context.Listings
            .Include(l => l.MenuItems)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task AddAsync(Listing listing)
    {
        listing.CreatedAt = DateTime.UtcNow;
        listing.UpdatedAt = DateTime.UtcNow;
        _context.Listings.Add(listing);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Listing listing)
    {
        var existing = await _context.Listings.FirstOrDefaultAsync(l => l.Id == listing.Id);
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

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Listings.FirstOrDefaultAsync(l => l.Id == id);
        if (existing == null)
        {
            return;
        }

        _context.Listings.Remove(existing);
        await _context.SaveChangesAsync();
    }

    // -----------------------------
    // Menu item methods
    // -----------------------------

    public async Task<IEnumerable<MenuItem>> GetMenuItemsForListingAsync(int listingId)
    {
        return await _context.MenuItems
            .Where(m => m.ListingId == listingId)
            .ToListAsync();
    }

    public async Task<Listing?> GetListingForMenuItemAsync(int listingId)
    {
        return await _context.Listings.FirstOrDefaultAsync(l => l.Id == listingId);
    }

    public async Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId)
    {
        return await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == menuItemId);
    }

    public async Task AddMenuItemAsync(MenuItem menuItem)
    {
        menuItem.CreatedAt = DateTime.UtcNow;
        menuItem.UpdatedAt = DateTime.UtcNow;
        _context.MenuItems.Add(menuItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMenuItemAsync(MenuItem menuItem)
    {
        var existing = await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == menuItem.Id);
        if (existing == null)
        {
            return;
        }

        existing.Name = menuItem.Name;
        existing.Price = menuItem.Price;
        existing.IsVeg = menuItem.IsVeg;
        existing.PhotoUrl = menuItem.PhotoUrl;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteMenuItemAsync(int menuItemId)
    {
        var existing = await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == menuItemId);
        if (existing == null)
        {
            return;
        }

        _context.MenuItems.Remove(existing);
        await _context.SaveChangesAsync();
    }
}