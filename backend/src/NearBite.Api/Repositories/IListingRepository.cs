using NearBite.Api.Domain;

namespace NearBite.Api.Repositories;

public interface IListingRepository
{
    // Listings
    IEnumerable<Listing> GetAll();
    Listing? GetById(int id);
    void Add(Listing listing);
    void Update(Listing listing);
    void Delete(int id);

    // Menu items
    IEnumerable<MenuItem> GetMenuItemsForListing(int listingId);
    MenuItem? GetMenuItemById(int menuItemId);
    void AddMenuItem(MenuItem menuItem);
    void UpdateMenuItem(MenuItem menuItem);
    void DeleteMenuItem(int menuItemId);
}