using Microsoft.AspNetCore.Mvc;
using NearBite.Api.Domain;
using NearBite.Api.Dtos;

namespace NearBite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController : ControllerBase
{
    // Sprint 1: hardcoded sample data. This lives inside the controller for now.
    // In Sprint 2 we move it behind a repository; in Sprint 4 it becomes a database.
    private static readonly List<Listing> _listings = new()
    {
        new Listing { Id = 1, Name = "Fort Cafe", Description = "Cozy cafe near the beach", Cuisine = "Sri Lankan", PriceRange = 2, City = "Negombo", LiveStatus = "Open", Latitude = 7.2094, Longitude = 79.8358, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        new Listing { Id = 2, Name = "Green Leaf Kottu",   Description = "Best kottu in town",       Cuisine = "Sri Lankan", PriceRange = 1, City = "Negombo", LiveStatus = "Open",   Latitude = 7.2110, Longitude = 79.8380, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        new Listing { Id = 3, Name = "Sunset Hoppers",     Description = "Traditional hoppers spot", Cuisine = "Sri Lankan", PriceRange = 1, City = "Negombo", LiveStatus = "Closed", Latitude = 7.2050, Longitude = 79.8400, IsVeg = true,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        new Listing { Id = 4, Name = "The Curry House",    Description = "Rice & curry, home style", Cuisine = "Sri Lankan", PriceRange = 2, City = "Negombo", LiveStatus = "Busy",   Latitude = 7.2080, Longitude = 79.8410, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        new Listing { Id = 5, Name = "Bella Italia",       Description = "Wood-fired pizza",         Cuisine = "Italian",    PriceRange = 3, City = "Negombo", LiveStatus = "Open",   Latitude = 7.2130, Longitude = 79.8340, IsVeg = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
    };

    [HttpGet]
    public ActionResult<IEnumerable<ListingDto>> GetAll()
    {
        var dtos = _listings.Select(l => new ListingDto
        {
            Id = l.Id,
            Name = l.Name,
            Description = l.Description,
            Cuisine = l.Cuisine,
            PriceRange = l.PriceRange,
            City = l.City,
            LiveStatus = l.LiveStatus,
            Latitude = l.Latitude,
            Longitude = l.Longitude,
            IsVeg = l.IsVeg
        });

        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public ActionResult<ListingDto> GetById(int id)
    {
        var listing = _listings.FirstOrDefault(l => l.Id == id);

        if (listing == null)
        {
            return NotFound();
        }

        var dto = new ListingDto
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

        return Ok(dto);
    }
}