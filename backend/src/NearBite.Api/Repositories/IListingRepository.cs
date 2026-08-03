using NearBite.Api.Domain;

namespace NearBite.Api.Repositories;

public interface IListingRepository
{
    IEnumerable<Listing> GetAll();
    Listing? GetById(int id);
    void Add(Listing listing);
    void Update(Listing listing);
    void Delete(int id);
}