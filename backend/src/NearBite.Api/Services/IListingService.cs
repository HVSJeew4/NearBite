using NearBite.Api.Dtos;

namespace NearBite.Api.Services;

public interface IListingService
{
    // Listings
    Task<IEnumerable<ListingDto>> GetAllAsync();
    Task<IEnumerable<ListingDto>> GetAllAsync(ListingFilterDto filter);
    Task<ListingDto?> GetByIdAsync(int id);
    Task<ListingDto> CreateAsync(CreateListingDto dto);
    Task<ListingDto?> UpdateAsync(int id, UpdateListingDto dto);
    Task<bool> DeleteAsync(int id);

    // Menu items
    Task<IEnumerable<MenuItemDto>?> GetMenuItemsForListingAsync(int listingId);
    Task<MenuItemDto?> GetMenuItemByIdAsync(int menuItemId);
    Task<MenuItemDto?> CreateMenuItemAsync(int listingId, CreateMenuItemDto dto);
    Task<MenuItemDto?> UpdateMenuItemAsync(int menuItemId, UpdateMenuItemDto dto);
    Task<bool> DeleteMenuItemAsync(int menuItemId);
}