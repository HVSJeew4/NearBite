using NearBite.Api.Domain;
using NearBite.Api.Dtos;

namespace NearBite.Api.Repositories;

public interface IListingRepository
{
    // Listings
    Task<IEnumerable<Listing>> GetAllAsync();
    Task<IEnumerable<Listing>> GetAllAsync(ListingFilterDto filter);
    Task<Listing?> GetByIdAsync(int id);
    Task AddAsync(Listing listing);
    Task UpdateAsync(Listing listing);
    Task DeleteAsync(int id);

    // Menu items
    Task<IEnumerable<MenuItem>> GetMenuItemsForListingAsync(int listingId);
    Task<Listing?> GetListingForMenuItemAsync(int listingId);
    Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId);
    Task AddMenuItemAsync(MenuItem menuItem);
    Task UpdateMenuItemAsync(MenuItem menuItem);
    Task DeleteMenuItemAsync(int menuItemId);
}