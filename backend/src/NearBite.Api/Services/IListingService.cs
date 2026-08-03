using NearBite.Api.Dtos;

namespace NearBite.Api.Services;

public interface IListingService
{
    IEnumerable<ListingDto> GetAll();
    ListingDto? GetById(int id);
    ListingDto Create(CreateListingDto dto);
    ListingDto? Update(int id, UpdateListingDto dto);
    bool Delete(int id);
}