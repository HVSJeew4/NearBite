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

    public IEnumerable<ListingDto> GetAll()
    {
        var listings = _repository.GetAll();
        return listings.Select(MapToDto);
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
}