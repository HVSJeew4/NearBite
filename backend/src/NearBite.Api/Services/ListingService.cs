using NearBite.Api.Domain;
using NearBite.Api.Dtos;
using NearBite.Api.Repositories;

namespace NearBite.Api.Services;

public class ListingService : IListingService
{
    private readonly IListingRepository _repository;

    public ListingService(IListingRepository repository)
    {
        _repository = repository;
    }

    // -----------------------------
    // Listing methods
    // -----------------------------

    public async Task<IEnumerable<ListingDto>> GetAllAsync()
    {
        var listings = await _repository.GetAllAsync();
        return listings.Select(MapToDto);
    }

    public async Task<IEnumerable<ListingDto>> GetAllAsync(ListingFilterDto filter)
    {
        // Cross-property validation the service is responsible for.
        if (filter.SortBy == "distance" && (filter.UserLat == null || filter.UserLng == null))
        {
            throw new ArgumentException("sortBy=distance requires both userLat and userLng.");
        }
        if (filter.SortBy == "rating")
        {
            throw new ArgumentException("sortBy=rating is not supported until reviews are added in Sprint 5.");
        }
        if (filter.SortBy != null && filter.SortBy != "" &&
            filter.SortBy != "name" && filter.SortBy != "price" && filter.SortBy != "distance")
        {
            throw new ArgumentException($"Unknown sortBy value: {filter.SortBy}");
        }

        // Fetch filtered results from repository. If sorting by distance, we get
        // an unsorted result set from the DB and do the sort in memory (Haversine
        // isn't easily translatable to SQL without PostGIS).
        var listings = (await _repository.GetAllAsync(filter)).ToList();

        if (filter.SortBy == "distance")
        {
            listings = listings
                .OrderBy(l => CalculateDistance(filter.UserLat!.Value, filter.UserLng!.Value, l.Latitude, l.Longitude))
                .ToList();
        }

        return listings.Select(MapToDto);
    }

    public async Task<ListingDto?> GetByIdAsync(int id)
    {
        var listing = await _repository.GetByIdAsync(id);
        return listing == null ? null : MapToDto(listing);
    }

    public async Task<ListingDto> CreateAsync(CreateListingDto dto)
    {
        var listing = new Listing
        {
            Name = dto.Name,
            Description = dto.Description,
            Cuisine = dto.Cuisine,
            PriceRange = dto.PriceRange,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            IsVeg = dto.IsVeg,
            City = "Negombo",
            LiveStatus = "Closed"
        };

        await _repository.AddAsync(listing);

        return MapToDto(listing);
    }

    public async Task<ListingDto?> UpdateAsync(int id, UpdateListingDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            return null;
        }

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.Cuisine = dto.Cuisine;
        existing.PriceRange = dto.PriceRange;
        existing.Latitude = dto.Latitude;
        existing.Longitude = dto.Longitude;
        existing.IsVeg = dto.IsVeg;
        existing.LiveStatus = dto.LiveStatus;

        await _repository.UpdateAsync(existing);

        return MapToDto(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        await _repository.DeleteAsync(id);
        return true;
    }

    // -----------------------------
    // Menu item methods
    // -----------------------------

    public async Task<IEnumerable<MenuItemDto>?> GetMenuItemsForListingAsync(int listingId)
    {
        var listing = await _repository.GetListingForMenuItemAsync(listingId);
        if (listing == null)
        {
            return null;
        }

        var items = await _repository.GetMenuItemsForListingAsync(listingId);
        return items.Select(MapMenuItemToDto);
    }

    public async Task<MenuItemDto?> GetMenuItemByIdAsync(int menuItemId)
    {
        var item = await _repository.GetMenuItemByIdAsync(menuItemId);
        return item == null ? null : MapMenuItemToDto(item);
    }

    public async Task<MenuItemDto?> CreateMenuItemAsync(int listingId, CreateMenuItemDto dto)
    {
        var listing = await _repository.GetListingForMenuItemAsync(listingId);
        if (listing == null)
        {
            return null;
        }

        var menuItem = new MenuItem
        {
            ListingId = listingId,
            Name = dto.Name,
            Price = dto.Price,
            IsVeg = dto.IsVeg,
            PhotoUrl = dto.PhotoUrl
        };

        await _repository.AddMenuItemAsync(menuItem);

        return MapMenuItemToDto(menuItem);
    }

    public async Task<MenuItemDto?> UpdateMenuItemAsync(int menuItemId, UpdateMenuItemDto dto)
    {
        var existing = await _repository.GetMenuItemByIdAsync(menuItemId);
        if (existing == null)
        {
            return null;
        }

        existing.Name = dto.Name;
        existing.Price = dto.Price;
        existing.IsVeg = dto.IsVeg;
        existing.PhotoUrl = dto.PhotoUrl;

        await _repository.UpdateMenuItemAsync(existing);

        return MapMenuItemToDto(existing);
    }

    public async Task<bool> DeleteMenuItemAsync(int menuItemId)
    {
        var existing = await _repository.GetMenuItemByIdAsync(menuItemId);
        if (existing == null)
        {
            return false;
        }

        await _repository.DeleteMenuItemAsync(menuItemId);
        return true;
    }

    // -----------------------------
    // Mapping helpers
    // -----------------------------

    private static ListingDto MapToDto(Listing listing)
    {
        return new ListingDto
        {
            Id = listing.Id,
            Name = listing.Name,
            Description = listing.Description,
            Cuisine = listing.Cuisine,
            PriceRange = listing.PriceRange,
            City = listing.City,
            LiveStatus = listing.LiveStatus,
            Latitude = listing.Latitude,
            Longitude = listing.Longitude,
            IsVeg = listing.IsVeg
        };
    }

    private static MenuItemDto MapMenuItemToDto(MenuItem menuItem)
    {
        return new MenuItemDto
        {
            Id = menuItem.Id,
            ListingId = menuItem.ListingId,
            Name = menuItem.Name,
            Price = menuItem.Price,
            IsVeg = menuItem.IsVeg,
            PhotoUrl = menuItem.PhotoUrl
        };
    }

    // -----------------------------
    // Distance calculation (Haversine) — still needed for sortBy=distance
    // -----------------------------

    private static double CalculateDistance(double lat1, double lng1, double lat2, double lng2)
    {
        const double earthRadiusKm = 6371.0;

        var dLat = ToRadians(lat2 - lat1);
        var dLng = ToRadians(lng2 - lng1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLng / 2) * Math.Sin(dLng / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return earthRadiusKm * c;
    }

    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}