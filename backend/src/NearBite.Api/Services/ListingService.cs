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

    public IEnumerable<ListingDto> GetAll()
    {
        var listings = _repository.GetAll();
        return listings.Select(MapToDto);
    }

    public IEnumerable<ListingDto> GetAll(ListingFilterDto filter)
    {
        if (filter.SortBy == "distance" && (filter.UserLat == null || filter.UserLng == null))
        {
            throw new ArgumentException("sortBy=distance requires both userLat and userLng.");
        }

        var listings = _repository.GetAll();
        var results = new List<Listing>();

        foreach (var listing in listings)
        {
            if (filter.Cuisine != null &&
                !listing.Cuisine.Equals(filter.Cuisine, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (filter.MaxPrice != null && listing.PriceRange > filter.MaxPrice.Value)
            {
                continue;
            }

            if (filter.IsVeg != null && listing.IsVeg != filter.IsVeg.Value)
            {
                continue;
            }

            if (filter.Search != null &&
                !listing.Name.Contains(filter.Search, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            results.Add(listing);
        }

        switch (filter.SortBy)
        {
            case "price":
                results.Sort((a, b) => a.PriceRange.CompareTo(b.PriceRange));
                break;

            case "distance":
                results.Sort((a, b) =>
                {
                    var distA = CalculateDistance(filter.UserLat!.Value, filter.UserLng!.Value, a.Latitude, a.Longitude);
                    var distB = CalculateDistance(filter.UserLat!.Value, filter.UserLng!.Value, b.Latitude, b.Longitude);
                    return distA.CompareTo(distB);
                });
                break;

            case "rating":
                throw new ArgumentException("sortBy=rating is not supported until reviews are added in Sprint 5.");

            case null:
            case "":
            case "name":
                results.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
                break;

            default:
                throw new ArgumentException($"Unknown sortBy value: {filter.SortBy}");
        }

        return results.Select(MapToDto);
    }

    public ListingDto? GetById(int id)
    {
        var listing = _repository.GetById(id);
        return listing == null ? null : MapToDto(listing);
    }

    public ListingDto Create(CreateListingDto dto)
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

        _repository.Add(listing);

        return MapToDto(listing);
    }

    public ListingDto? Update(int id, UpdateListingDto dto)
    {
        var existing = _repository.GetById(id);
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

        _repository.Update(existing);

        return MapToDto(existing);
    }

    public bool Delete(int id)
    {
        var existing = _repository.GetById(id);
        if (existing == null)
        {
            return false;
        }

        _repository.Delete(id);
        return true;
    }

    // -----------------------------
    // Menu item methods
    // -----------------------------

    public IEnumerable<MenuItemDto>? GetMenuItemsForListing(int listingId)
    {
        var listing = _repository.GetById(listingId);
        if (listing == null)
        {
            return null;
        }

        var items = _repository.GetMenuItemsForListing(listingId);
        return items.Select(MapMenuItemToDto);
    }

    public MenuItemDto? GetMenuItemById(int menuItemId)
    {
        var item = _repository.GetMenuItemById(menuItemId);
        return item == null ? null : MapMenuItemToDto(item);
    }

    public MenuItemDto? CreateMenuItem(int listingId, CreateMenuItemDto dto)
    {
        var listing = _repository.GetById(listingId);
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

        _repository.AddMenuItem(menuItem);

        return MapMenuItemToDto(menuItem);
    }

    public MenuItemDto? UpdateMenuItem(int menuItemId, UpdateMenuItemDto dto)
    {
        var existing = _repository.GetMenuItemById(menuItemId);
        if (existing == null)
        {
            return null;
        }

        existing.Name = dto.Name;
        existing.Price = dto.Price;
        existing.IsVeg = dto.IsVeg;
        existing.PhotoUrl = dto.PhotoUrl;

        _repository.UpdateMenuItem(existing);

        return MapMenuItemToDto(existing);
    }

    public bool DeleteMenuItem(int menuItemId)
    {
        var existing = _repository.GetMenuItemById(menuItemId);
        if (existing == null)
        {
            return false;
        }

        _repository.DeleteMenuItem(menuItemId);
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
    // Distance calculation (Haversine)
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