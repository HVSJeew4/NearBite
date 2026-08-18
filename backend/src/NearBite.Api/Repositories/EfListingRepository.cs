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

    public IEnumerable<Listing> GetAll()
    {
        return _context.Listings.ToList();
    }

    public Listing? GetById(int id)
    {
        return _context.Listings
            .Include(l => l.MenuItems)
            .FirstOrDefault(l => l.Id == id);
    }

    public void Add(Listing listing)
    {
        listing.CreatedAt = DateTime.UtcNow;
        listing.UpdatedAt = DateTime.UtcNow;
        _context.Listings.Add(listing);
        _context.SaveChanges();
    }

    public void Update(Listing listing)
    {
        var existing = _context.Listings.FirstOrDefault(l => l.Id == listing.Id);
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

        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var existing = _context.Listings.FirstOrDefault(l => l.Id == id);
        if (existing == null)
        {
            return;
        }

        _context.Listings.Remove(existing);
        _context.SaveChanges();
        // No manual RemoveAll for menu items — the DB's ON DELETE CASCADE handles it.
    }

    // -----------------------------
    // Menu item methods
    // -----------------------------

    public IEnumerable<MenuItem> GetMenuItemsForListing(int listingId)
    {
        return _context.MenuItems
            .Where(m => m.ListingId == listingId)
            .ToList();
    }

    public MenuItem? GetMenuItemById(int menuItemId)
    {
        return _context.MenuItems.FirstOrDefault(m => m.Id == menuItemId);
    }

    public void AddMenuItem(MenuItem menuItem)
    {
        menuItem.CreatedAt = DateTime.UtcNow;
        menuItem.UpdatedAt = DateTime.UtcNow;
        _context.MenuItems.Add(menuItem);
        _context.SaveChanges();
    }

    public void UpdateMenuItem(MenuItem menuItem)
    {
        var existing = _context.MenuItems.FirstOrDefault(m => m.Id == menuItem.Id);
        if (existing == null)
        {
            return;
        }

        existing.Name = menuItem.Name;
        existing.Price = menuItem.Price;
        existing.IsVeg = menuItem.IsVeg;
        existing.PhotoUrl = menuItem.PhotoUrl;
        existing.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
    }

    public void DeleteMenuItem(int menuItemId)
    {
        var existing = _context.MenuItems.FirstOrDefault(m => m.Id == menuItemId);
        if (existing == null)
        {
            return;
        }

        _context.MenuItems.Remove(existing);
        _context.SaveChanges();
    }
}