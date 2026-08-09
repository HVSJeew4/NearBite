using NearBite.Api.Dtos;

namespace NearBite.Api.Services;

public interface IListingService
{
    // Listings
    IEnumerable<ListingDto> GetAll();
    IEnumerable<ListingDto> GetAll(ListingFilterDto filter);
    ListingDto? GetById(int id);
    ListingDto Create(CreateListingDto dto);
    ListingDto? Update(int id, UpdateListingDto dto);
    bool Delete(int id);

    // Menu items
    IEnumerable<MenuItemDto>? GetMenuItemsForListing(int listingId);
    MenuItemDto? GetMenuItemById(int menuItemId);
    MenuItemDto? CreateMenuItem(int listingId, CreateMenuItemDto dto);
    MenuItemDto? UpdateMenuItem(int menuItemId, UpdateMenuItemDto dto);
    bool DeleteMenuItem(int menuItemId);
}